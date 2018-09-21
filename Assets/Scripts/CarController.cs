using UnityEngine;
using System.Collections.Generic;

public class CarController : MonoBehaviour, IMovable {

    struct State
    {
        public State(Vector3 pos)
        {
            position = pos;
        }

        public Vector3 position;
    }
    List<State> stateList;

    float lerpTime;
    float currentLerpTime;
    float perc = 1;

    Vector3 startPos;
    Vector3 endPos;
    Vector3 firstPos;

    bool firstInput;

    public bool carInput = false;

    #region interface 정의
    public void SaveState()
    {
        stateList.Add(new State(transform.position));
    }

    public void LoadState()
    {
        if (stateList.Count != 0)
        {
            transform.position = stateList[stateList.Count - 1].position;
            endPos = stateList[stateList.Count - 1].position;
            stateList.RemoveAt(stateList.Count - 1);
        }
    }
    #endregion

    void Start ()
    {
        endPos = gameObject.transform.position;
        firstPos = gameObject.transform.position;

        stateList = new List<global::CarController.State>();
    }
    
	void Update () {

        if (Time.timeScale == 0)
            return;

        startPos = gameObject.transform.position;
        
        if (carInput)
        {
            if (perc == 1)
            {
                lerpTime = 1;
                currentLerpTime = 0;
                firstInput = true;
            }
        }

        if (carInput && gameObject.transform.position == endPos && gameObject.tag == "LeftCar")
        {
            endPos = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z);
        }
        else if (carInput && gameObject.transform.position == endPos && gameObject.tag == "RightCar")
        {
            endPos = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
        }

        if (firstInput == true)
        {
            currentLerpTime += Time.deltaTime * 5;
            perc = currentLerpTime / lerpTime;

            if (perc > 0.8)
            {
                perc = 1;
            }

            gameObject.transform.position = Vector3.Lerp(startPos, endPos, perc);

            if (Mathf.Round(perc) == 1)
            {
                carInput = false;
            }
        }

        if (gameObject.tag == "LeftCar" && startPos.x <= -10.0f)
        {
            gameObject.transform.position = new Vector3(firstPos.x, transform.position.y, transform.position.z);
            endPos = new Vector3(firstPos.x, transform.position.y, transform.position.z);
        }

        if (gameObject.tag == "RightCar" && startPos.x >= 10.0f)
        {
            gameObject.transform.position = new Vector3(firstPos.x, transform.position.y, transform.position.z);
            endPos = new Vector3(firstPos.x, transform.position.y, transform.position.z);
        }
    }
    

    public void CarInput()
    {
        carInput = true;
        
    }
    
}
