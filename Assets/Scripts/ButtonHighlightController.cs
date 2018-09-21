using UnityEngine;
using System.Collections;

public class ButtonHighlightController : MonoBehaviour {

    Animator anim;

    void Start ()
    {
        anim = gameObject.GetComponent<Animator>();
    }
    
    public void HighlightAnim()
    {
        anim.SetTrigger("Highlight");
    }

    public void blinkAnim()
    {
        if (anim != null)
        {
            anim.SetBool("Blink", true);
        }
    }

    public void stopBlinkAnim()
    {
        StartCoroutine("StopBlink");
    }

    IEnumerator StopBlink()
    {
        yield return null;

        if (anim != null)
        {
            anim.SetBool("Blink", false);
        }
    }
}
