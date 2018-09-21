using UnityEngine;
using System;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

public partial class DataManager
{
    public static DataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new DataManager();
            }

            return _instance;
        }
    }
    
    /// <summary> Load savedata from GPGS cloud </summary>
    public void Activate()
    {
        Load();
    }

    public void AddDataStageClear(string stageName, int score)
    {
        if (_playerData.stageDictionary.ContainsKey(stageName))
        {
            if (_playerData.stageDictionary[stageName] < score)
                _playerData.stageDictionary[stageName] = score;
            else
                return;
        }   
        else
        {
            _playerData.stageDictionary.Add(stageName, score);
        }

        Save();
    }

    public int GetStageScore(string stageName)
    {
        if (_playerData.stageDictionary.ContainsKey(stageName))
            return _playerData.stageDictionary[stageName];
        else
            return -1;
    }

    public int GetClearedStageCount()
    {
        return _playerData.stageDictionary.Count;
    }

    public string GetCurChapterName()
    {
        return currentChapter.ToString();
    }

    public PlayerData GetPlayerData()
    {
        return _playerData;
    }

    public void SetPlayerData(PlayerData p)
    {
        _playerData = p;
    }
}

public partial class DataManager
{
    private static DataManager _instance;
    private DataManager() { }

    public readonly string SaveFileName = "CodeIslandSave";
    public readonly int stagesPerChapter = 8;
    public ChapterEnum currentChapter;

    private PlayerData _playerData = new PlayerData();
    private TimeSpan _collapsedTimeTillLastSave;

    private void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");

        if (_collapsedTimeTillLastSave == TimeSpan.FromMilliseconds(0))
        {
            _playerData.TotalPlayTime = new TimeSpan(0, 0, (int)Time.time);
            _collapsedTimeTillLastSave = _playerData.TotalPlayTime;
        }
        else
        {
            _playerData.TotalPlayTime = new TimeSpan(0, 0, (int)Time.time - _collapsedTimeTillLastSave.Seconds);
            _collapsedTimeTillLastSave = _playerData.TotalPlayTime;
        }

        bf.Serialize(file, _playerData);
 
        file.Close();


#if UNITY_ANDROID && !UNITY_EDITOR
        GPGSManager.Instance.OpenSavedGame(SaveFileName);
#endif
        Debug.Log("Data Saved");
    }

    private void Load()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        GPGSManager.Instance.OpenSavedGame(SaveFileName, isLoad: true, loadFailCB: ()=>
        {
            if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
                PlayerData data = (PlayerData)bf.Deserialize(file);
                file.Close();

                this._playerData.stageDictionary = data.stageDictionary;
                Debug.Log("Data Loaded : " + (_playerData.stageDictionary.ContainsKey("Stage_1") ? 1 : 0));
            }
        });
#elif UNITY_EDITOR
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            this._playerData.stageDictionary = data.stageDictionary;
            Debug.Log("Data Loaded : " + (_playerData.stageDictionary.ContainsKey("Stage_1") ? 1 : 0));
        }
#endif
    }

    private void UnlockNextStage()
    {

    }
}
