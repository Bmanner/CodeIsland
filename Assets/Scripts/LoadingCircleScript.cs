using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadingCircleScript : MonoBehaviour {

    Transform loadingCircle;

    int angle;

	// Use this for initialization
	void Start () {
        loadingCircle = transform.Find("LoadingCircle");
        angle = 0;
	}
	
	// Update is called once per frame
	void Update () {
        angle += 5;

        if (angle < 360)
            loadingCircle.transform.rotation = Quaternion.Euler(0, 0, angle);
        else
            angle = 0;
	}
}
