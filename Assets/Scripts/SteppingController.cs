using UnityEngine;
using System.Collections.Generic;

public class SteppingController : MonoBehaviour, IMovable {

    struct State
    {
        public State(Vector3 pos, int inputCount, int warningCount, bool isDrop)
        {
            position = pos;
            inputCnt = inputCount;
            warningCnt = warningCount;
            this.isDrop = isDrop;
        }

        public Vector3 position;
        public int inputCnt;
        public int warningCnt;
        public bool isDrop;
    }
    List<State> stateList;

    int inputCount = 0;
    int warningCount = 1;
    bool isDrop;

    [HideInInspector] public bool IsDrop
    {
        get
        {
            if (playerBounceScript.OnMove)
                return false;
            else
                return isDrop;
        }
    }
    [HideInInspector] public bool isWarning;

    public int interval;

    Player playerBounceScript;

    Vector3 pos;

    #region interface 정의
    public void SaveState()
    {
        stateList.Add(new State(transform.position, inputCount, warningCount, isDrop));
    }

    public void LoadState()
    {
        if (stateList.Count != 0)
        {
            transform.position = stateList[stateList.Count - 1].position;
            inputCount = stateList[stateList.Count - 1].inputCnt;
            warningCount = stateList[stateList.Count - 1].warningCnt;
            isDrop = stateList[stateList.Count - 1].isDrop;
            stateList.RemoveAt(stateList.Count - 1);
        }
    }
    #endregion

    void Start()
    {
        pos = gameObject.transform.position;

        stateList = new List<global::SteppingController.State>();
    }

    void Update()
    {
        if (isDrop == true)
        {
            if (pos.y >= -1.0f)
            {
                pos = new Vector3(transform.position.x, transform.position.y - 0.1f, transform.position.z);
                gameObject.transform.position = pos;
            }
        }
        else
        {
            if (pos.y <= -0.3f)
            {
                pos = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
                gameObject.transform.position = pos;
            }
        }

    }

    void updownStepping()
    {
        if (inputCount != 0 && inputCount % interval == 0)
        {
            isDrop = true;
            //gameObject.GetComponent<BoxCollider>().enabled = false;

            //if (playerBounceScript != null)
            //    playerBounceScript.IsStepping = false;
        }
        else
        {
            isDrop = false;
            //gameObject.GetComponent<BoxCollider>().enabled = true;
        }

        if (warningCount != 0 && warningCount % interval == 0)
        {
            isWarning = true;
        }
        else
        {
            isWarning = false;
        }
    }

    public void SteppingInput()
    {
        warningCount += 1;
        inputCount += 1;
        updownStepping();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (playerBounceScript == null)
                playerBounceScript = other.GetComponent<Player>();
        }
    }
}
