using UnityEngine;
using System.Collections;

public class AnimationController : MonoBehaviour {

    Animator anim;
    public GameObject thePlayer;

	void Start ()
    {
        anim = gameObject.GetComponent<Animator>();
	}
	

	void Update ()
    {
        Player bounceScript = thePlayer.GetComponent<Player>();

        if (bounceScript.justJump == true && bounceScript.isOver == false)
        {
            anim.SetBool("Jump", true);
        }
        else
        {
            anim.SetBool("Jump", false);
        }
        
        if (bounceScript.rightInput || bounceScript.rightCollideInput)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        if (bounceScript.leftInput || bounceScript.leftCollideInput)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, -90, 0);
        }
        if (bounceScript.upInput || bounceScript.upCollideInput)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        if (bounceScript.downInput || bounceScript.downCollideInput)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
    
    public void DeadAnim()
    {
        anim.SetTrigger("Dead");
    }

    public void TrapDeadAnim()
    {
        anim.SetTrigger("TrapDead");
    }
}
