/*
 * 테스트용 이동 스크립트
*/

using UnityEngine;
using System.Collections;

public class TestingMove : MonoBehaviour {

    Vector3 chrPos;
	
	void Update () {

        if (Input.GetButtonDown("right"))
        {
            chrPos = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
            gameObject.transform.position = new Vector3(chrPos.x, chrPos.y, chrPos.z);
        }
        if (Input.GetButtonDown("left"))
        {
            chrPos = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z);
            gameObject.transform.position = new Vector3(chrPos.x, chrPos.y, chrPos.z);
        }
        if (Input.GetButtonDown("up"))
        {
            chrPos = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1);
            gameObject.transform.position = new Vector3(chrPos.x, chrPos.y, chrPos.z);
        }
        if (Input.GetButtonDown("down"))
        {
            chrPos = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1);
            gameObject.transform.position = new Vector3(chrPos.x, chrPos.y, chrPos.z);
        }
        
    }
}
