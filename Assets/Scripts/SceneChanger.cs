using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour {

    string thisScene;
    public AudioClip selectSound;
    AudioSource source;
    float alpha;

    void Start()
    {
        source = GetComponent<AudioSource>();

        thisScene = SceneManager.GetActiveScene().name;

        if (thisScene == "Chapter_Select" || thisScene == "Stage_Select" || thisScene == "Staff")
        {
            source.PlayOneShot(selectSound, 0.6f);
        }
        
        if (thisScene == "Staff")
        {
            if (Social.localUser.authenticated) // 로그인이 되어 있는지 확인
            {
                // unlock achievement (achievement_5)
                Social.ReportProgress("CgkI0Njd1fQSEAIQCg", 100.0f, (bool success) => {
                    // handle success or failure
                });
            }
        }
    }

    public void LoadStaffScene()
    {
        SceneManager.LoadScene("Staff");
    }

    public void StartGame() {
        SceneManager.LoadScene("Chapter_Select");
    }

    public void StartStage()
    {
        SceneManager.LoadScene("Stage_1");
    }

    public void StopClicked() {
        SceneManager.LoadScene("Stage_Select");
    }
    
 /*
    void FadeIn()
    {
        fadeImage.GetComponent<CanvasRenderer>().SetAlpha(0.1f);
        fadeImage.CrossFadeAlpha(1f, 0.4f, false);

        StartCoroutine(FadeInterval());
    }   
    

    IEnumerator FadeInterval()
    {
        yield return new WaitForSeconds(0.4f);

        alpha = fadeImage.GetComponent<CanvasRenderer>().GetAlpha();

        Debug.Log(alpha);

        if (alpha >= 0.9f)
        {
            SceneManager.LoadScene("Chapter_Select");
        }
    }
  */
}
