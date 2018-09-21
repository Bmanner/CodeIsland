using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    public GameObject chickenPlayer;
    public float cameraHeight = 6.5f;

    public int RightSideBorder;
    public int LeftSideBorder;
    public int UpperSideBorder;

    Vector3 cameraPos;

    Camera cam;

    void Start()
    {
        cam = gameObject.GetComponent<Camera>();
    }

    void LateUpdate () {
        Vector3 targetPos = chickenPlayer.transform.position;

        if (targetPos.x < LeftSideBorder)
            targetPos.x = LeftSideBorder;
        else if (targetPos.x > RightSideBorder)
            targetPos.x = RightSideBorder;

        if (targetPos.z > UpperSideBorder)
            targetPos.z = UpperSideBorder;

        cameraPos = Vector3.Lerp(gameObject.transform.position, targetPos - new Vector3(0, 0, 1.5f), Time.deltaTime * 3);
        gameObject.transform.position = new Vector3(cameraPos.x, cameraHeight, cameraPos.z);
	}
}
