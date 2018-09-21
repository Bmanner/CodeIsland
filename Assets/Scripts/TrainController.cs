using UnityEngine;
using System.Collections.Generic;

public class TrainController : MonoBehaviour, IMovable {

    struct State
    {
        public State(bool isRed, int trCount, int redCount)
        {
            isRedLightOn = isRed;
            trainCnt = trCount;
            redCnt = redCount;
        }

        public bool isRedLightOn;
        public int trainCnt;
        public int redCnt;
    }
    List<State> stateList;

    int trainCount = 0;
    int redCount = 1;
    public bool isRedlight;
    public bool isTrain;

    public int interval;

    Vector3 pos;
    Transform ParentRedLight;
    Transform redLight;

    #region interface 정의
    public void SaveState()
    {
        stateList.Add(new global::TrainController.State(isRedlight, trainCount, redCount));
    }

    public void LoadState()
    {
        if (stateList.Count != 0)
        {
            isRedlight = stateList[stateList.Count - 1].isRedLightOn;
            trainCount = stateList[stateList.Count - 1].trainCnt;
            redCount = stateList[stateList.Count - 1].redCnt;

            redLight.gameObject.SetActive(isRedlight);

            stateList.RemoveAt(stateList.Count - 1);
        }
    }
    #endregion

    void Start()
    {
        pos = gameObject.transform.position;
        redLight = gameObject.transform.parent.GetChild(3).transform.GetChild(1).transform.GetChild(0);
        Debug.Log(redLight);
        //redLight = ParentRedLight.transform.FindChild("RedLight");
        //redLight = GameObject.Find("ParentRedLight").transform.FindChild("RedLight");

        stateList = new List<global::TrainController.State>();
    }

    void Update()
    {
        if (isTrain == true)
        {
            pos = new Vector3(transform.position.x - 2.0f, transform.position.y, transform.position.z);
            gameObject.transform.position = pos;
        }
        else
        {
            pos = new Vector3(30, transform.position.y, transform.position.z);
            gameObject.transform.position = pos;
        }
    }

    void RedInterval ()
    {
        
        if (redCount != 0 && redCount % interval == 0)
        {
            isRedlight = true;
            redLight.gameObject.SetActive(true);
        }
        else
        {
            isRedlight = false;
            redLight.gameObject.SetActive(false);
        }
        
    }

    void TrainInterval()
    {
        if (trainCount != 0 && trainCount % interval == 0)
        {
            isTrain = true;
        }
        else
        {
            isTrain = false;
        }
       
    }

    public void TrainInput()
    {
        redCount += 1;
        RedInterval();
        trainCount += 1;
        TrainInterval();
    }
}
