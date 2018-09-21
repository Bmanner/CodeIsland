using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArrowGunController : MonoBehaviour, IMovable {
    struct State
    {
        public State(bool isAlert, int shootCnt, int alertCnt)
        {
            isAlertOn = isAlert;
            this.shootCnt = shootCnt;
            this.alertCnt = alertCnt;
        }

        public bool isAlertOn;
        public int shootCnt;
        public int alertCnt;
    }
    List<State> stateList;

    readonly string SHOOTCOROUTINE = "ShootArrow";

    int shootCount = 0;
    int alertCount = 1;
    public bool isAlertOn;
    public bool isInShooting;

    public int interval;
    public float ArrowSpeed;

    GameObject arrow;

    //Transform alert;

    #region interface 정의
    public void SaveState()
    {
        stateList.Add(new State(isAlertOn, shootCount, alertCount));
    }

    public void LoadState()
    {
        if (stateList.Count != 0)
        {
            isAlertOn = stateList[stateList.Count - 1].isAlertOn;
            shootCount = stateList[stateList.Count - 1].shootCnt;
            alertCount = stateList[stateList.Count - 1].alertCnt;

            //alert.gameObject.SetActive(isAlertOn);

            stateList.RemoveAt(stateList.Count - 1);
        }
    }
    #endregion

    void Start()
    {
        arrow = transform.Find("obj_arrow").gameObject;
        //alert = gameObject.transform.parent.GetChild(3).transform.GetChild(1).transform.GetChild(0);
        
        stateList = new List<State>();
    }
    /*
    void Update()
    {
        if (isInShooting == true)
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
    */
    void AlertInterval()
    {
        if (alertCount != 0 && alertCount % interval == 0)
        {
            isAlertOn = true;
        }
        else
        {
            isAlertOn = false;
        }
    }

    public void ArrowInput()
    {
        alertCount += 1;
        AlertInterval();
        shootCount += 1;

        if (shootCount != 0 && shootCount % interval == 0)
        {
            if (arrow.activeSelf)
            {
                StopCoroutine(SHOOTCOROUTINE);
                arrow.transform.localPosition = Vector3.zero;
                arrow.SetActive(false);
            }
            
            StartCoroutine(SHOOTCOROUTINE);
            shootCount = 0;
        }
    }

    IEnumerator ShootArrow()
    {
        float pos = 0f;
        arrow.SetActive(true);

        while (arrow.activeSelf && pos < 50)
        {
            if (transform.rotation.y == 0)
                arrow.transform.position += Vector3.left * ArrowSpeed * Time.deltaTime;
            else if (transform.rotation.y == 90)
                arrow.transform.position += Vector3.up * ArrowSpeed * Time.deltaTime;
            else if (transform.rotation.y == 180)
                arrow.transform.position += Vector3.right * ArrowSpeed * Time.deltaTime;
            else
                arrow.transform.position += Vector3.down * ArrowSpeed * Time.deltaTime;

            pos += ArrowSpeed * Time.deltaTime;

            yield return null;
        }

        arrow.transform.localPosition = Vector3.zero;
        arrow.SetActive(false);
    }
}
