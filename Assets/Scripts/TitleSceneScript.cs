using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleSceneScript : MonoBehaviour {
    

    void Awake()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (!Social.localUser.authenticated)
        {
            if (!DataManager.Instance.doneLoadSeq)
            {
                var canvas = GameObject.Find("Canvas");
                var loadingPanel = canvas.transform.Find("LoadingPanel");
                loadingPanel.gameObject.SetActive(true);
            }

            GPGSManager.Instance.InitializeGPGS();
            GPGSManager.Instance.LoginGPGS();
        }
#elif UNITY_EDITOR
        DataManager.Instance.Activate();
#endif
    }
    
}
