using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HistoryManager : MonoBehaviour
{

    public struct HistoryInfo
    {
        public HistoryInfo(UserScriptInfo[] scriptInfo, int repeatCnt)
        {
            this.scriptInfo = scriptInfo;
            this.repeatCount = repeatCnt;
        }

        public int repeatCount;
        public UserScriptInfo[] scriptInfo;
    }

    private static HistoryManager _instance;
    public static HistoryManager Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = GameObject.FindObjectOfType(typeof(HistoryManager)) as HistoryManager;
                if (!_instance)
                    Debug.LogError("There needs to be one active script on a GameObject in your scene.");
            }

            return _instance;
        }
    }

    public SimpleObjectPool historyObjPool;

    ButtonEvent buttonEvent;

    Transform contentPanel;

    List<GameObject> historyLineList;
    List<UserScriptInfo> scriptHistoryList;
    List<short> numOfScriptInLineN; // N번째 줄의 블록 갯수
    List<int> rptCntInLineN; // N번째 줄의 반복 횟수

    // Use this for initialization
    void Start()
    {
        contentPanel = transform.Find("History/Viewport/Content");

        buttonEvent = GameObject.Find("Canvas/ButtonEvent").GetComponent<ButtonEvent>();

        historyLineList = new List<GameObject>();
        scriptHistoryList = new List<UserScriptInfo>();
        numOfScriptInLineN = new List<short>();
        rptCntInLineN = new List<int>();
    }

    public void RecordScriptHistory(List<UserScriptInfo> scriptList, int repeatCount)
    {
        scriptHistoryList.AddRange(scriptList);

        numOfScriptInLineN.Add((short)(scriptList.Count));
        rptCntInLineN.Add(repeatCount);

        //Draw in Panel
        var historyLine = historyObjPool.GetObject();
        var HLController = historyLine.GetComponent<HistoryLineController>();

        historyLine.transform.SetParent(contentPanel);
        historyLineList.Add(historyLine);

        HLController.Initialize(scriptList, repeatCount, historyLineList.Count, buttonEvent);
    }

    public HistoryInfo RemoveLastHistory()
    {
        if (scriptHistoryList.Count != 0)
        {
            UserScriptInfo[] scriptArray = new UserScriptInfo[numOfScriptInLineN[numOfScriptInLineN.Count - 1]];
            scriptHistoryList.CopyTo(scriptHistoryList.Count - numOfScriptInLineN[numOfScriptInLineN.Count - 1], scriptArray, 0, numOfScriptInLineN[numOfScriptInLineN.Count - 1]);

            var historyInfo = new HistoryInfo(scriptArray, rptCntInLineN[rptCntInLineN.Count - 1]);

            scriptHistoryList.RemoveRange(scriptHistoryList.Count - numOfScriptInLineN[numOfScriptInLineN.Count - 1], numOfScriptInLineN[numOfScriptInLineN.Count - 1]);
            numOfScriptInLineN.RemoveAt(numOfScriptInLineN.Count - 1);
            rptCntInLineN.RemoveAt(rptCntInLineN.Count - 1);

            historyObjPool.ReturnObject(historyLineList[historyLineList.Count - 1]);
            historyLineList.RemoveAt(historyLineList.Count - 1);

            return historyInfo;
        }

        return new HistoryInfo(null, 1);
    }

    public int GetUsedNumOfScript()
    {
        return scriptHistoryList.Count;
    }
}
