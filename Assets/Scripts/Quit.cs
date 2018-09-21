using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class Quit : MonoBehaviour
{
    String thisScene;
    Transform QuitPanel;
    Transform BlackCurtain;

    void Start()
    {
        thisScene = SceneManager.GetActiveScene().name;

        if (thisScene == "Chapter_Select" || thisScene == "Start")
        {
            QuitPanel = GameObject.Find("ParentQuitPanel").transform.Find("QuitPanel");
            BlackCurtain = GameObject.Find("ParentBlackCurtain").transform.Find("BlackCurtain");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            thisScene = SceneManager.GetActiveScene().name;

            if (thisScene == "Start")
            {
                ToggleQuitPanel();
            }
            else if (thisScene == "Stage_Select")
            {
                SceneManager.LoadScene("Chapter_Select");
            }
            else if (thisScene == "Chapter_Select" || thisScene == "Staff")
            {
                SceneManager.LoadScene("Start");
            }
        }
    }

    public void BackClicked()
    {
        thisScene = SceneManager.GetActiveScene().name;

        if (thisScene == "Stage_Select")
        {
            SceneManager.LoadScene("Chapter_Select");
        }
        else if (thisScene == "Chapter_Select" || thisScene == "Staff")
        {
            SceneManager.LoadScene("Start");
        }
    }

    public void YesClicked()
    {
        Debug.Log("종료");
        Application.Quit();
    }

    public void NoClicked()
    {
        ToggleQuitPanel();
    }

    public void ToggleQuitPanel()
    {
        if (QuitPanel.gameObject.activeSelf == true)
        {
            QuitPanel.gameObject.SetActive(false);
            BlackCurtain.gameObject.SetActive(false);
        }
        else
        {
            QuitPanel.gameObject.SetActive(true);
            BlackCurtain.gameObject.SetActive(true);
        }

    }

    public void PrivacyPolicy()
    {
        Application.OpenURL("http://blog.naver.com/adayofdream/220999613182");
    }
}
