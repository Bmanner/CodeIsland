using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GasiController : MonoBehaviour, IMovable {

    struct State
    {
        public State(bool isAlert, int trCount, int redCount)
        {
            isRedLightOn = isAlert;
            gasiCnt = trCount;
            alertCnt = redCount;
        }

        public bool isRedLightOn;
        public int gasiCnt;
        public int alertCnt;
    }
    List<State> stateList;

    int gasiCount = 1;
    int alertCount = 2;

    public bool isAlert;
    public bool isTrain;

    public int interval;
    public int startIntervalCount = 1;

    Vector3 pos;
    GameObject gasiOn;

    #region interface 정의
    public void SaveState()
    {
        stateList.Add(new State(isAlert, gasiCount, alertCount));
    }

    public void LoadState()
    {
        if (stateList.Count != 0)
        {
            isAlert = stateList[stateList.Count - 1].isRedLightOn;
            gasiCount = stateList[stateList.Count - 1].gasiCnt;
            alertCount = stateList[stateList.Count - 1].alertCnt;

            if (gasiCount != 0 && gasiCount % interval == 0)
            {
                gasiOn.SetActive(true);
                gasiOn.transform.localPosition = new Vector3(0, 0, 0);
            }
            else
            {
                gasiOn.transform.localPosition = new Vector3(0, -9, 0);
                gasiOn.SetActive(false);
            }

            stateList.RemoveAt(stateList.Count - 1);
        }
    }
    #endregion

    void Start()
    {
        pos = gameObject.transform.position;
        gasiOn = transform.Find("gasi_on").gameObject;

        gasiCount = startIntervalCount;
        alertCount = startIntervalCount + 1;
         
        AlertInterval();

        if (gasiCount == 0)
        {
            gasiOn.SetActive(true);
            gasiOn.transform.localPosition = new Vector3(0, 0, 0);
        }

        stateList = new List<State>();
    }

    void AlertInterval()
    {

        if (alertCount != 0 && alertCount % interval == 0)
        {
            isAlert = true;
        }
        else
        {
            isAlert = false;
        }

    }

    IEnumerator GasiInterval()
    {
        if (gasiCount != 0 && gasiCount % interval == 0)
        {
            gasiOn.SetActive(true);

            while(gasiOn.transform.localPosition.y < 0)
            {
                gasiOn.transform.localPosition= new Vector3(0, gasiOn.transform.localPosition.y + Time.deltaTime * 72f, 0);
                yield return null;
            }
            gasiOn.transform.localPosition = new Vector3(0, 0, 0);
        }
        else
        {
            while (gasiOn.transform.localPosition.y > -9)
            {
                gasiOn.transform.localPosition = new Vector3(0, gasiOn.transform.localPosition.y - Time.deltaTime * 72f, 0);
                yield return null;
            }
            gasiOn.transform.localPosition = new Vector3(0, -9, 0);

            gasiOn.SetActive(false);
        }
    }

    public void GasiInput()
    {
        alertCount += 1;
        AlertInterval();
        gasiCount += 1;
        StartCoroutine("GasiInterval");
    }
}
