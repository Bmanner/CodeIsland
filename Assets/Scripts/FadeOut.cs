using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour {

    public Image fadeImage;
    float alpha;
    Transform fade;

    void Start()
    {
        fade = GameObject.Find("ParentFadeImage").transform.Find("FadeImage");

        fadeImage.GetComponent<CanvasRenderer>().SetAlpha(0.9f);
        fadeImage.CrossFadeAlpha(0.0f, 0.5f, false);

        StartCoroutine(FadeInterval());
    }
   

    IEnumerator FadeInterval()
    {
        yield return new WaitForSeconds(0.5f);

        alpha = fadeImage.GetComponent<CanvasRenderer>().GetAlpha();
        

        if (alpha <= 0.1f)
        {
            fade.gameObject.SetActive(false);
        }
    }
}
