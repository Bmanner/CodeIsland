using UnityEngine;
using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi.SavedGame;
using System;
using GooglePlayGames.BasicApi;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

public partial class GPGSManager {

    public static GPGSManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GPGSManager();
            }

            return _instance;
        }
    }
    
    /// <summary> 현재 로그인 중인지 체크 </summary>
    public bool bLogin
    {
        get;
        set;
    }

    /// <summary> GPGS를 초기화 합니다. </summary>
    public void InitializeGPGS()
    {
        bLogin = false;

        PlayGamesPlatform.Activate();
    }

    /// <summary> GPGS를 로그인 합니다. </summary>
    public void LoginGPGS()
    {
        // 로그인이 안되어 있으면
        if (!Social.localUser.authenticated)
        {
            Social.localUser.Authenticate(LoginCallBackGPGS);
        }
    }

    /// <summary> GPGS Login Callback </summary>
    public void LoginCallBackGPGS(bool result)
    {
        bLogin = result;

        // Execute load routine
        DataManager.Instance.Activate();
    }

    /// <summary> GPGS를 로그아웃 합니다. </summary>
    public void LogoutGPGS()
    {
        // 로그인이 되어 있으면
        if (Social.localUser.authenticated)
        {
            ((GooglePlayGames.PlayGamesPlatform)Social.Active).SignOut();
            bLogin = false;
        }
    }

    /// <summary> GPGS에서 자신의 프로필 이미지를 가져옵니다. </summary>
    public Texture2D GetImageGPGS()
    {
        if (Social.localUser.authenticated)
            return Social.localUser.image;
        else
            return null;
    }

    /// <summary> GPGS 에서 사용자 이름을 가져옵니다. </summary>
    public string GetNameGPGS()
    {
        if (Social.localUser.authenticated)
            return Social.localUser.userName;
        else
            return null;
    }

    public void OpenSavedGame(string filename, bool isLoad = false, LoadDelegate loadFailCB = null)
    {
        _loadFailCallback = loadFailCB;

        if(!bLogin)
            LoginGPGS();

        if (bLogin)
        {
            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

            if (isLoad)
                savedGameClient.OpenWithAutomaticConflictResolution(filename, DataSource.ReadCacheOrNetwork,
                ConflictResolutionStrategy.UseLongestPlaytime, OnSavedGameOpenedForLoad);
            else // is Save
                savedGameClient.OpenWithAutomaticConflictResolution(filename, DataSource.ReadCacheOrNetwork,
                ConflictResolutionStrategy.UseLongestPlaytime, OnSavedGameOpenedForSave);
        }
        else if(isLoad && loadFailCB != null)
        {
            LoadRoutineEndCallBack(success: false);
        }
    }

    public void OnSavedGameOpenedForLoad(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            // handle reading or writing of saved game.
            // if this is the first time use cloud
            if (game.TotalTimePlayed == TimeSpan.FromMilliseconds(0))
            {
                // do nothing
            }
            else // load from cloud
            {
                ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
                savedGameClient.ReadBinaryData(game, OnSavedGameDataRead);
            }
            LoadRoutineEndCallBack(success: true);
        }
        else
        {
            // handle error
            LoadRoutineEndCallBack(success: false);
        }
    }

    public void OnSavedGameOpenedForSave(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            // handle reading or writing of saved game.
            PlayerData progressInfo = DataManager.Instance.GetPlayerData();

            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream m = new MemoryStream();
            bf.Serialize(m, progressInfo);
            m.Flush();
                
            byte[] binaryInfo = m.ToArray();

            SaveGame(game, binaryInfo, progressInfo.TotalPlayTime);
        }
        else
        {
            // handle error
            Debug.Log("#####OnSavedGamedOpenedForSave Error : " + status);
        }
    }

    void OnSavedGameDataRead(SavedGameRequestStatus status, byte[] data)
    {
        // 바이트 데이터를 게임 데이터로 변환시켜서 저장한다. MemoryStream사용.
        // 충돌 있을 경우에 대한 처리
        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream m = new MemoryStream(data);
        var deserializedData = bf.Deserialize(m) as PlayerData;
        m.Flush();

        if (deserializedData is PlayerData)
        {
            // 클라우드의 세이브 데이터가 로컬의 데이터보다 TotalPlayTime이 길 경우 클라우드의 세이브 데이터 사용
            if (deserializedData.stageDictionary.Count > DataManager.Instance.GetPlayerData().stageDictionary.Count)
            {
                DataManager.Instance.SetPlayerData(deserializedData);
            }
        }
    }

    void SaveGame(ISavedGameMetadata game, byte[] savedData, TimeSpan totalPlaytime)
    {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

        SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder();
        builder = builder
            .WithUpdatedPlayedTime(totalPlaytime)
            .WithUpdatedDescription("Saved game at " + DateTime.Now);

        var savedImage = GetScreenshot();
        if (savedImage != null)
        {
            // This assumes that savedImage is an instance of Texture2D
            // and that you have already called a function equivalent to
            // getScreenshot() to set savedImage
            // NOTE: see sample definition of getScreenshot() method below
            byte[] pngData = savedImage.EncodeToPNG();
            builder = builder.WithUpdatedPngCoverImage(pngData);
        }
        SavedGameMetadataUpdate updatedMetadata = builder.Build();
        savedGameClient.CommitUpdate(game, updatedMetadata, savedData, OnSavedGameWritten);
    }

    public void OnSavedGameWritten(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            // handle reading or writing of saved game.
        }
        else
        {
            Debug.Log("#####OnSavedGameWritten called but Error : " + status);
            // handle error
        }
    }

    public Texture2D GetScreenshot()
    {
        // Create a 2D texture that is 1024x700 pixels from which the PNG will be extracted
        Texture2D screenShot = new Texture2D(200, 140);

        // Takes the screenshot from top left hand corner of screen and maps to top
        // left hand corner of screenShot texture
        screenShot.ReadPixels(
            new Rect(0, 0, Screen.width, (Screen.width / 1024) * 700), 0, 0);
        return screenShot;
    }

}

public partial class GPGSManager
{
    private static GPGSManager _instance;
    private GPGSManager() { }

    public delegate void LoadDelegate();
    private LoadDelegate _loadFailCallback;
    
    void LoadRoutineEndCallBack(bool success)
    {
        var curStageName = SceneManager.GetActiveScene().name;
        if(curStageName == "Start")
        {
            var loadingPanel = GameObject.Find("Canvas/LoadingPanel");
            loadingPanel.SetActive(false);
        }
        if (!success)
            _loadFailCallback();
    }
}