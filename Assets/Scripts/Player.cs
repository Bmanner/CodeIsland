using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Player : MonoBehaviour, IMovable
{
    struct State
    {
        public State(bool[] beadsState, int beadNum, Vector3 pos, Quaternion rot)
        {
            beadActive = beadsState;
            beadCnt = beadNum;
            position = pos;
            rotation = rot;
        }

        public bool[] beadActive;
        public int beadCnt;
        public Vector3 position;
        public Quaternion rotation;
    }
    List<State> stateList;

    #region 멤버 변수 선언부
    float lerpTime;
    float currentLerpTime;
    float perc = 1;

    float elapsedTime;

    Vector3 startPos;
    Vector3 endPos;

    bool firstInput;
    public bool justJump;
    public bool isOver;
    public bool OnMove
    {
        get
        {
            if (perc < 1 || upInput || downInput || rightInput || leftInput)
                return true;
            else
                return false;
        }
    }
    bool isWater = false;
    bool isStepping = false;
    public bool IsStepping
    {
        get { return isStepping; }
        set { isStepping = value; }
    }

    public bool isGasiAlertOn;
    public bool isArrowAlertOn;

    public bool upInput;
    public bool downInput;
    public bool leftInput;
    public bool rightInput;
    
    public bool collideInput;
    public bool upCollideInput;
    public bool downCollideInput;
    public bool leftCollideInput;
    public bool rightCollideInput;

    public int beadCount = 0;

    GameObject panel;
    PanelController panelScript;

    public GameObject playerObject;
    GameObject[] beads;

    AudioSource audioSource;
    public float audioVolume = 1.0f;
    public AudioClip jumpSound;
    public AudioClip deadSound;
    public AudioClip pickSound; // 오디오 클립 추가

    SteppingController stepScript;

    Transform trap_triggerd;

    public GameObject Alert;
    ArrowGunController arrowGunController;
    GasiController gasiController;
    ButtonEvent buttonEvent;

    GameObject mainCamera;
    Camera cam;
    #endregion

    #region interface 정의
    public void SaveState()
    {
        bool[] beadsState = new bool[3];
        for (int i = 0; i < beads.Length; i++)
            beadsState[i] = beads[i].activeInHierarchy;

        stateList.Add(new State(beadsState, beadCount, transform.position, playerObject.transform.rotation));
    }

    public void LoadState()
    {
        if (stateList.Count != 0)
        {
            var boxCollider = gameObject.GetComponent<BoxCollider>();
            boxCollider.enabled = false;

            transform.position = stateList[stateList.Count - 1].position;
            playerObject.transform.rotation = stateList[stateList.Count - 1].rotation;
            endPos = stateList[stateList.Count - 1].position;

            if (beadCount != stateList[stateList.Count - 1].beadCnt)
            {
                beadCount = stateList[stateList.Count - 1].beadCnt;
                panelScript.CubeController(beadCount);

                for (int i = 0; i < beads.Length; i++)
                    beads[i].SetActive(stateList[stateList.Count - 1].beadActive[i]);
            }

            boxCollider.enabled = true;

            stateList.RemoveAt(stateList.Count - 1);
        }
    }
    #endregion

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        panel = GameObject.Find("PanelController");
        Debug.Log(panel);
        panelScript = panel.GetComponent<PanelController>();
        buttonEvent = GameObject.Find("Canvas/ButtonEvent").GetComponent<ButtonEvent>();

        beads = GameObject.FindGameObjectsWithTag("Bead");

        isOver = false;
        isGasiAlertOn = false;
        isArrowAlertOn = false;
        Time.timeScale = 1;

        elapsedTime = 0f;

        stateList = new List<global::Player.State>();

        mainCamera = GameObject.Find("Main Camera");
        cam = mainCamera.GetComponent<Camera>();
    }

    void Update()
    {
        if (Time.timeScale == 0)
            return;

        //Game Timer
        if (!isOver)
        {
            elapsedTime += Time.deltaTime;
            panelScript.UpdateElapseTime(elapsedTime);
        }
        else
            buttonEvent.StopAllCoroutines();

        startPos = gameObject.transform.position;

        if (upInput || downInput || leftInput || rightInput || collideInput)
        {
            if (perc == 1)
            {
                lerpTime = 1;
                currentLerpTime = 0;
                audioSource.PlayOneShot(jumpSound, audioVolume);
                justJump = true;
                firstInput = true;
            }
        }

        if (!isOver)
        {
            if (rightInput && gameObject.transform.position == endPos)
            {
                endPos = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
            }
            if (leftInput && gameObject.transform.position == endPos)
            {
                endPos = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z);
            }
            if (upInput && gameObject.transform.position == endPos)
            {
                endPos = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1);
            }
            if (downInput && gameObject.transform.position == endPos)
            {
                endPos = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1);
            }
            if (collideInput && gameObject.transform.position == endPos) // 충돌 감지 시 제자리 점프
            {
                endPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            }
        }

        if (isWater && !isStepping && Mathf.Round(perc) == 1)
        {
            endPos = new Vector3(transform.position.x, transform.position.y - 0.15f, transform.position.z);
        }

        currentLerpTime += Time.deltaTime * 5;
        perc = currentLerpTime / lerpTime;

        if (perc > 0.8)
        {
            perc = 1;
        }

        if (firstInput)
        {
            gameObject.transform.position = Vector3.Lerp(startPos, endPos, perc);
        }

        if (Mathf.Round(perc) == 1 || isOver == true)
        {
            justJump = false;
            upInput = false;
            downInput = false;
            leftInput = false;
            rightInput = false;
            collideInput = false;
            upCollideInput = false;
            downCollideInput = false;
            leftCollideInput = false;
            rightCollideInput = false;
        }
    }

    public void UpInput()
    {
        upInput = true;
    }

    public void DownInput()
    {
        downInput = true;
    }

    public void LeftInput()
    {
        leftInput = true;
    }

    public void RightInput()
    {
        rightInput = true;
    }

    public void UpCollideInput()
    {
        collideInput = true;
        upCollideInput = true;
    }

    public void DownCollideInput()
    {
        collideInput = true;
        downCollideInput = true;
    }

    public void LeftCollideInput()
    {
        collideInput = true;
        leftCollideInput = true;
    }

    public void RightCollideInput()
    {
        collideInput = true;
        rightCollideInput = true;
    }

    void OnTriggerEnter(Collider other)
    {
        AnimationController anim = playerObject.GetComponent<AnimationController>();

        if (other.gameObject.CompareTag("Bead"))
        {
            other.gameObject.SetActive(false);

            audioSource.PlayOneShot(pickSound, audioVolume);

            beadCount += 1;

            panelScript.CubeController(beadCount);

        }

        else if (other.gameObject.CompareTag("SendingPoint"))
        {
            isOver = true;
            panelScript.OnClearPanel(beadCount, elapsedTime, HistoryManager.Instance.GetUsedNumOfScript());
            Debug.Log("clear");
        }

        else if (other.gameObject.CompareTag("LeftCar") || other.gameObject.CompareTag("RightCar") || other.gameObject.CompareTag("Train"))
        {
            //Time.timeScale = 0.6f;

            StartCoroutine("DeadDirection");

            isOver = true;
            anim.DeadAnim();
            audioSource.PlayOneShot(deadSound, audioVolume);
            Debug.Log("Game Over by Car");
        }

        else if (other.gameObject.CompareTag("Trap"))
        {
            //Time.timeScale = 0.6f;

            StartCoroutine("DeadDirection");

            trap_triggerd = other.transform.parent.GetChild(1);

            other.gameObject.SetActive(false);
            trap_triggerd.gameObject.SetActive(true);

            isOver = true;
            anim.TrapDeadAnim();
            audioSource.PlayOneShot(deadSound, audioVolume);
            Debug.Log("Game Over by Trap");
        }
        else if (other.gameObject.CompareTag("GasiOn"))
        {
            //Time.timeScale = 0.6f;

            StartCoroutine("DeadDirection");

            isOver = true;
            anim.DeadAnim();
            audioSource.PlayOneShot(deadSound, audioVolume);
            Debug.Log("Game Over by Gasi");
        }

        else if (other.gameObject.CompareTag("Stepping"))
            stepScript = other.GetComponent<SteppingController>();

        else if (other.gameObject.CompareTag("Arrow"))
        {
            //Time.timeScale = 0.6f;

            StartCoroutine("DeadDirection");

            isOver = true;
            anim.TrapDeadAnim();
            audioSource.PlayOneShot(deadSound, audioVolume);
            Debug.Log("Game Over by Arrow");
        }
    }

    void OnTriggerStay(Collider other)
    {

        if (other.gameObject.CompareTag("SteppingStone"))
            isStepping = true;
        /* LEGACY
        if (other.gameObject.CompareTag("Stepping"))
        {
            isStepping = !stepScript.IsDrop;
        }
        */
        if (other.CompareTag("ArrowGun"))
        {
            if (arrowGunController == null)
                arrowGunController = other.GetComponent<ArrowGunController>();

            if (arrowGunController.isAlertOn)
            {
                Alert.SetActive(true);
                isArrowAlertOn = true;
            }
            else
            {
                Alert.SetActive(false);
                isArrowAlertOn = false;
            }
        }

        if (other.CompareTag("Gasi"))
        {
            if (gasiController == null)
                gasiController = other.GetComponent<GasiController>();

            if (gasiController.isAlert)
            {
                Alert.SetActive(true);
                isGasiAlertOn = true;
            }
            else
            {
                Alert.SetActive(false);
                isGasiAlertOn = false;
            }
        }

        if (other.gameObject.CompareTag("Water"))
        {
            if (!isStepping)
            {
                isWater = true;
            }
        }

    }

    void OnTriggerExit(Collider other)
    {
        PanelController panelScript = panel.GetComponent<PanelController>();

        if (other.gameObject.CompareTag("SteppingStone"))
        {
            isStepping = false;
        }
        
        if (other.CompareTag("ArrowGun"))
        {
            arrowGunController = null;
            isArrowAlertOn = false;
        }

        if (other.CompareTag("Gasi"))
        {
            gasiController = null;
            isGasiAlertOn = false;
        }

        if (other.gameObject.CompareTag("Water"))
        {
            isWater = false;

            if (!isStepping)
            {
                //Time.timeScale = 0.6f;

                StartCoroutine("DeadDirection");

                isOver = true;
                audioSource.PlayOneShot(deadSound, audioVolume);
                Debug.Log("Game Over by Water");
            }
        }
        // 콜라이더에서 나갈 때엔 항상 꺼준다.
        Alert.SetActive(false);
    }

    IEnumerator DeadDirection()
    {
        mainCamera.GetComponent<CameraFollow>().enabled = false;

        float dtTimer = 0f;
        float originalCamSize = cam.orthographicSize;
        Vector3 originalCamPos = mainCamera.transform.position;

        while(dtTimer <= 1.2f)
        {
            cam.orthographicSize = Mathf.Lerp(originalCamSize, 4f, dtTimer * 1.2f);

            mainCamera.transform.position = Vector3.Lerp(originalCamPos, transform.position + new Vector3(0, originalCamPos.y, -5.5f), dtTimer * 1.2f);

            dtTimer += Time.deltaTime;

            yield return null;
        }

        panelScript.OnFailPanel(elapsedTime, HistoryManager.Instance.GetUsedNumOfScript());
    }

}
