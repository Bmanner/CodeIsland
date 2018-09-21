using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text;
using UnityEngine.Events;

public class ButtonEvent : MonoBehaviour
{
    #region 멤버 변수 선언부
    public AudioClip selectSound;
    public AudioClip selectSound2;
    AudioSource source;
    public float audioVolume = 1.0f;

    public GameObject UpBtn;
    public GameObject DownBtn;
    public GameObject LeftBtn;
    public GameObject RightBtn;
    public GameObject StopBtn;
    public GameObject IfBtn;

    GameObject thePlayer;
    public GameObject train;

    public Sprite[] RepeatSprite;
    public Sprite[] IfBlockSprite;
    public Sprite ifBtn_arrow;
    public Sprite ifBtn_notArrow;
    public Sprite ifBtn_gasi;
    public Sprite ifBtn_notGasi;

    public Material UIGrayscaleMat;

    public float moveInterval = 0.7f;
    public float rayLength = 1.0f;

    Transform upRay;
    Transform downRay;
    Transform leftRay;
    Transform rightRay;
    Transform forwardRay;

    GameObject[] steppings;
    GameObject[] trains;
    GameObject[] arrows;
    GameObject[] gasis;
    GameObject[] leftCars;
    GameObject[] rightCars;

    Player bounceScript;
    List<GasiController> gasiControllerList = new List<GasiController>();
    List<ArrowGunController> arrowGunControllerList = new List<ArrowGunController>();
    List<TrainController> trainControllerList = new List<TrainController>();
    List<SteppingController> steppingControllerList = new List<SteppingController>();
    List<CarController> carControllerList = new List<CarController>();

    List<IMovable> movableObjs;
    
    [HideInInspector]
    public bool isPlaying = false;
    bool gasiAlertOn = false;
    bool ifnotred = true;
    int repeatCount = 1;

    ifType curIfMode = ifType.None;

    GameObject QueuePanel;
    GameObject btnIns;

    Button ffBtn;
    Button undoBtn;
    Image playBtn;
    /// <summary> 이프 버튼(이미지)가 들어가있는 아이 </summary>
    List<GameObject> ifBtnList;

    //Transform repeatBtns;
    Transform ifBtn;

    List<UserScriptInfo> scriptList;

    ButtonHighlightController buttonScript;
    #endregion
    void Start()
    {
        source = GetComponent<AudioSource>();
        scriptList = new List<UserScriptInfo>();
        ifBtnList = new List<GameObject>();
        movableObjs = new List<IMovable>();

        QueuePanel = GameObject.Find("Canvas/QueuePanel");
        playBtn = GameObject.Find("Canvas/QueuePanel/PlayBtn").GetComponent<Image>();
        ffBtn = GameObject.Find("Canvas/TopPanel/DoubleBtn").GetComponent<Button>();
        undoBtn = GameObject.Find("Canvas/QueuePanel/UndoBtn").GetComponent<Button>();

        ffBtn.interactable = false;

        steppings = GameObject.FindGameObjectsWithTag("Stepping");
        trains = GameObject.FindGameObjectsWithTag("Train");
        arrows = GameObject.FindGameObjectsWithTag("ArrowGun");
        gasis = GameObject.FindGameObjectsWithTag("Gasi");
        leftCars = GameObject.FindGameObjectsWithTag("LeftCar");
        rightCars = GameObject.FindGameObjectsWithTag("RightCar");

        upRay = GameObject.Find("Ray").transform.Find("UpRay");
        downRay = GameObject.Find("Ray").transform.Find("DownRay");
        leftRay = GameObject.Find("Ray").transform.Find("LeftRay");
        rightRay = GameObject.Find("Ray").transform.Find("RightRay");
        forwardRay = GameObject.Find("PlayerObject").transform.Find("ForwardRay");

        thePlayer = GameObject.Find("ParentPlayer");

        //repeatBtns = GameObject.Find("ParentRepeatBtn").transform.FindChild("RepeatBtns");

        bounceScript = thePlayer.GetComponent<Player>();
        movableObjs.Add(bounceScript);

        foreach (var car in leftCars)
        {
            carControllerList.Add(car.GetComponent<CarController>());
            movableObjs.Add(car.GetComponent<CarController>());
        }
        foreach (var car in rightCars)
        {
            carControllerList.Add(car.GetComponent<CarController>());
            movableObjs.Add(car.GetComponent<CarController>());
        }
        foreach (var train in trains)
        {
            trainControllerList.Add(train.GetComponent<TrainController>());
            movableObjs.Add(train.GetComponent<TrainController>());
        }
        foreach (var arrow in arrows)
        {
            arrowGunControllerList.Add(arrow.GetComponent<ArrowGunController>());
            movableObjs.Add(arrow.GetComponent<ArrowGunController>());
        }
        foreach (var gasi in gasis)
        {
            gasiControllerList.Add(gasi.GetComponent<GasiController>());
            movableObjs.Add(gasi.GetComponent<GasiController>());
        }
        foreach (var stepping in steppings)
        {
            steppingControllerList.Add(stepping.GetComponent<SteppingController>());
            movableObjs.Add(stepping.GetComponent<SteppingController>());
        }

        SetButtonEvent();
    }
    /*
    void Update ()
    {
        Debug.DrawRay(forwardRay.position, forwardRay.forward * 1.0f, Color.green);

        RaycastHit hitObject;

        if (Physics.Raycast(forwardRay.position, forwardRay.forward, out hitObject, rayLength))
        {
            if (hitObject.transform.tag == "Stepping")
            {
                SteppingController steppingScript = hitObject.transform.GetComponent<SteppingController>();

                if (steppingScript.isWarning)
                {
                    ifnotstepping = true;
                    //Debug.Log("ifnotstepping");
                }
                else
                {
                    ifnotstepping = false;
                    //Debug.Log("ifstepping");
                }
            }
            else if (hitObject.transform.tag == "SteppingStone")
            {
                ifnotstepping = false;
            }
            else if (hitObject.transform.tag == "Track")
            {
                TrainController trainScript = hitObject.transform.FindChild("Train").GetComponent<TrainController>();

                if (trainScript.isRedlight)
                {
                    ifnotred = false;
                }
                else
                {
                    ifnotred = true;
                }
            }
        }
        else
        {
            ifnotstepping = true;
            ifnotred = true;
        }
        
    }
    */

    // 명령어 입력 시 명령어 표시창에 선택된 명령어를 표시해준다.
    void DrawButton()
    {
        GameObject btnIns;
        if (scriptList[scriptList.Count - 1].direction == direction.up)
            btnIns = Instantiate(UpBtn, QueuePanel.transform) as GameObject;
        else if (scriptList[scriptList.Count - 1].direction == direction.down)
            btnIns = Instantiate(DownBtn, QueuePanel.transform) as GameObject;
        else if (scriptList[scriptList.Count - 1].direction == direction.left)
            btnIns = Instantiate(LeftBtn, QueuePanel.transform) as GameObject;
        else if (scriptList[scriptList.Count - 1].direction == direction.right)
            btnIns = Instantiate(RightBtn, QueuePanel.transform) as GameObject;
        else// (scriptList[scriptList.Count - 1].direction == direction.stop)
            btnIns = Instantiate(StopBtn, QueuePanel.transform) as GameObject;

        RectTransform rt = btnIns.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2((scriptList.Count - 1) * 208 - 416, 0);
        if(scriptList[scriptList.Count - 1].ifType != ifType.None)
            rt.sizeDelta *= 0.9f;

        // 생성된 버튼에 몇번 째 명령어인지 데이터 기록
        UserScriptInfo info = scriptList[scriptList.Count - 1];

        scriptList[info.index].clonedBtn = btnIns;

        // 입력된 Move명령어에 대해 눌렀을 시 삭제되는 터치 이벤트를 붙인다
        btnIns.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (isPlaying)
                return;

            // 현재 이프문 편집 중인데 이프문 바깥의 스크립트 삭제할 경우엔 편집중이던 이프 블록 펑~
            if(curIfMode != ifType.None && info.belongingIfBlock != ifBtnList[ifBtnList.Count - 1])
            {
                int repeatAmount = scriptList.Count;
                for(int k = repeatAmount - 1; k >= 0; k--)
                {
                    if(scriptList[k].belongingIfBlock == ifBtnList[ifBtnList.Count - 1])
                    {
                        Destroy(scriptList[k].clonedBtn);
                        scriptList.Remove(scriptList[k]);
                    }
                }
                
                Destroy(ifBtnList[ifBtnList.Count - 1]);
                ifBtnList.RemoveAt(ifBtnList.Count - 1);

                curIfMode = ifType.None;
            }

            Destroy(info.clonedBtn);
            //info.clonedBtn = null;

            if (info.ifType != ifType.None)
            {
                ifType tempIfType = info.ifType;
                if ( curIfMode == ifType.None && 
                    ((info.index == 0 && scriptList.Count > 1 && scriptList[1].ifType != tempIfType) ||
                    (info.index == 0 && scriptList.Count == 1) ||
                    (info.index != 0 && scriptList[info.index - 1].ifType != tempIfType && info.index == scriptList.Count - 1) ||
                    (info.index != 0 && scriptList[info.index - 1].ifType != tempIfType && scriptList[info.index + 1].ifType != tempIfType)) )
                {
                    ifBtnList.Remove(info.belongingIfBlock);
                    Destroy(info.belongingIfBlock);
                    info.belongingIfBlock = null;
                }
                else
                {
                    AdjustIfBlockLength(info.belongingIfBlock.GetComponent<Image>(), true);
                }
            }

            scriptList.RemoveAt(info.index);

            // 중간에 있는 명령어가 없어졌을 경우 뒤에있는 명령어들의 위치를 앞으로 당겨준다
            if (scriptList.Count != info.index)
            {
                GameObject ifBlockTemp = null;

                for (int i = info.index; i < scriptList.Count; i++)
                {
                    RectTransform btnRT = scriptList[i].clonedBtn.GetComponent<RectTransform>();
                    btnRT.anchoredPosition = new Vector2(i * 208 - 416, 0);

                    scriptList[i].index = i;

                    if (scriptList[i].ifType != ifType.None && scriptList[i].belongingIfBlock != info.belongingIfBlock &&
                        scriptList[i].belongingIfBlock != ifBlockTemp)
                    {
                        ifBlockTemp = scriptList[i].belongingIfBlock;
                        RectTransform r = ifBlockTemp.GetComponent<RectTransform>();
                        r.anchoredPosition = new Vector2(r.anchoredPosition.x - 208, 0);
                    }
                }
            }

            // 서로 같은 종류의 다른 이프문 두개 사이에 있는 스크립트가 없어졌을 경우, 이 두 이프문을 하나로 이어준다.
            UserScriptInfo tempInfo = null;
            foreach (UserScriptInfo item in scriptList)
            {
                if (tempInfo == null)
                    tempInfo = item;
                else
                {
                    if (item.ifType != ifType.None && tempInfo.ifType != ifType.None &&
                        item.ifType == tempInfo.ifType && item.belongingIfBlock != tempInfo.belongingIfBlock)
                    {
                        ifBtnList.Remove(item.belongingIfBlock);
                        Destroy(item.belongingIfBlock);
                        item.belongingIfBlock = tempInfo.belongingIfBlock;

                        AdjustIfBlockLength(tempInfo.belongingIfBlock.GetComponent<Image>(), false);
                    }

                    tempInfo = item;
                }
            }
        });

        // ifBtn 그리는 로직
        if (curIfMode != ifType.None && ifBtnList.Count != 0)
        {
            ifBtnList[ifBtnList.Count - 1].GetComponent<Image>().sprite = IfBlockSprite[ifModeCount()];
            scriptList[info.index].belongingIfBlock = ifBtnList[ifBtnList.Count - 1];
        }
    }

    int ifModeCount()
    {
        int count = 0;

        int listLength = scriptList.Count - 1;

        for (int i = 0; i < listLength; i++)
        {
            if (scriptList[listLength - 1 - i].ifType == curIfMode)
                count++;
            else
                break;
        }

        return count;
    }

    void AdjustIfBlockLength(Image image, bool shorter)
    {
        for (int i = 0; i < IfBlockSprite.Length; i++)
        {
            if( (shorter && i == 0) || (!shorter && i == IfBlockSprite.Length - 1) )
                continue;

            if (image.sprite == IfBlockSprite[i])
            {
                image.sprite = IfBlockSprite[i + (shorter ? -1 : 1)];
                i += 5;
            }
        }
    }

    void ClearButton()
    {
        GameObject[] clones = GameObject.FindGameObjectsWithTag("ButtonClone");
        GameObject[] ifBtns = GameObject.FindGameObjectsWithTag("IfBtn");

        for (int i = 0; i < clones.Length; i++)
        {
            Destroy(clones[i]);
        }

        for (int i = 0; i < ifBtns.Length; i++)
        {
            Destroy(ifBtns[i]);
        }
    }

    void HighlightButton(GameObject gameObj)
    {
        ButtonHighlightController highlightScript = gameObj.GetComponent<ButtonHighlightController>();
        highlightScript.HighlightAnim();
    }

    void UpClicked()
    {
        source.PlayOneShot(selectSound2, audioVolume);
        if (!isPlaying && scriptList.Count < 5)
        {
            scriptList.Add(new UserScriptInfo(direction.up, curIfMode, scriptList.Count));
            DrawButton();
        }
    }

    void DownClicked()
    {
        source.PlayOneShot(selectSound2, audioVolume);
        if (!isPlaying && scriptList.Count < 5)
        {
            scriptList.Add(new UserScriptInfo(direction.down, curIfMode, scriptList.Count));
            DrawButton();
        }
    }

    void LeftClicked()
    {
        source.PlayOneShot(selectSound2, audioVolume);
        if (!isPlaying && scriptList.Count < 5)
        {
            scriptList.Add(new UserScriptInfo(direction.left, curIfMode, scriptList.Count));
            DrawButton();
        }
    }

    void RightClicked()
    {
        source.PlayOneShot(selectSound2, audioVolume);
        if (!isPlaying && scriptList.Count < 5)
        {
            scriptList.Add(new UserScriptInfo(direction.right, curIfMode, scriptList.Count));
            DrawButton();
        }
    }

    void StopClicked()
    {
        source.PlayOneShot(selectSound2, audioVolume);
        if (!isPlaying && scriptList.Count < 5)
        {
            scriptList.Add(new UserScriptInfo(direction.stop, curIfMode, scriptList.Count));
            DrawButton();
        }
    }

    public void IfGasiClicked()
    {
        InitIfMode(ifType.ifGasi);
    }

    public void IfnotGasiClicked()
    {
        InitIfMode(ifType.ifNotGasi);
    }

    public void IfArrowClicked()
    {
        InitIfMode(ifType.ifArrow);
    }

    public void IfnotArrowClicked()
    {
        InitIfMode(ifType.ifNotArrow);
    }
    // 아래 클릭관련 함수들은 에디터에서 직접 연결하여 사용 중
    public void PlayClicked()
    {
        Play();
    }

    /*
    public void RepeatClicked ()
    {
        AudioSource.PlayClipAtPoint(selectSound, transform.position);
        if (repeatBtns.gameObject.activeSelf == true)
        {
            repeatBtns.gameObject.SetActive(false);
        }
        else
        {
            repeatBtns.gameObject.SetActive(true);
        }
    }
    */

    public void Repeat1Clicked()
    {
        source.PlayOneShot(selectSound2, audioVolume);
        if (!isPlaying)
        {
            playBtn.sprite = RepeatSprite[1];
            this.repeatCount = 1;
        }
    }

    public void Repeat2Clicked()
    {
        source.PlayOneShot(selectSound2, audioVolume);
        if (!isPlaying)
        {
            playBtn.sprite = RepeatSprite[2];
            this.repeatCount = 2;
        }
    }

    public void Repeat3Clicked()
    {
        source.PlayOneShot(selectSound2, audioVolume);
        if (!isPlaying)
        {
            playBtn.sprite = RepeatSprite[3];
            this.repeatCount = 3;
        }
    }

    public void Repeat4Clicked()
    {
        source.PlayOneShot(selectSound2, audioVolume);
        if (!isPlaying)
        {
            playBtn.sprite = RepeatSprite[4];
            this.repeatCount = 4;
        }
    }

    public void Repeat5Clicked()
    {
        source.PlayOneShot(selectSound2, audioVolume);
        if (!isPlaying)
        {
            playBtn.sprite = RepeatSprite[5];
            this.repeatCount = 5;
        }
    }

    private void Play()
    {
        // 이프블록 편집중이거나 다른 명령어가 실행중일 때엔 플레이가 되지 않도록 함
        if (!isPlaying && curIfMode == ifType.None)
        {
            if (scriptList.Count < 1)
            {
                Debug.Log("1개 이상 입력해야 함.");
            }
            else
            {
                isPlaying = true;

                HistoryManager.Instance.RecordScriptHistory(scriptList, repeatCount);
                // Save all movable object's state
                foreach (var obj in movableObjs)
                    obj.SaveState();

                StartCoroutine(MoveInput());
            }
        }
    }

    IEnumerator MoveInput()
    {
        ffBtn.interactable = true;
        undoBtn.interactable = false;

        for (int r = 0; r < repeatCount; r++)
        {
            playBtn.sprite = RepeatSprite[repeatCount - r - 1];

            RaycastHit hit;

            for (int t = 0; t < trainControllerList.Count; t++)
            {
                trainControllerList[t].isTrain = false;
            }

            if(r > 0)
            {
                foreach (var script in scriptList)
                    script.clonedBtn.GetComponent<Image>().material = null;
            }

            bool satisfyCondition = true;

            for (int i = 0; i < scriptList.Count; i++)
            {
                var originalDir = scriptList[i].direction;
                Transform dir;

                if (originalDir == direction.up)
                    dir = upRay;
                else if (originalDir == direction.left)
                    dir = leftRay;
                else if (originalDir == direction.right)
                    dir = rightRay;
                else
                    dir = downRay;

                if (i > 0 && scriptList[i].ifType != ifType.None && scriptList[i].ifType != scriptList[i - 1].ifType)
                    satisfyCondition = true;

                //if 조건문에 해당하는 경우 검사
                if (scriptList[i].ifType == ifType.None)
                    ; // do nothing
                else if (i > 0 && scriptList[i].ifType == scriptList[i - 1].ifType && !satisfyCondition)
                    continue;
                else if (scriptList[i].ifType == ifType.ifArrow && !bounceScript.isArrowAlertOn)
                {
                    satisfyCondition = false;

                    scriptList[i].clonedBtn.GetComponent<Image>().material = UIGrayscaleMat;
                    for (int k = i + 1; k < scriptList.Count; k++)
                    {
                        if (scriptList[k].belongingIfBlock == scriptList[i].belongingIfBlock)
                            scriptList[k].clonedBtn.GetComponent<Image>().material = UIGrayscaleMat;
                    }

                    yield return new WaitForSeconds(moveInterval);
                    continue;
                }
                else if (scriptList[i].ifType == ifType.ifNotArrow && bounceScript.isArrowAlertOn)
                {
                    satisfyCondition = false;

                    scriptList[i].clonedBtn.GetComponent<Image>().material = UIGrayscaleMat;
                    for (int k = i + 1; k < scriptList.Count; k++)
                    {
                        if (scriptList[k].belongingIfBlock == scriptList[i].belongingIfBlock)
                            scriptList[k].clonedBtn.GetComponent<Image>().material = UIGrayscaleMat;
                    }

                    yield return new WaitForSeconds(moveInterval);
                    continue;
                }
                else if (scriptList[i].ifType == ifType.ifGasi && !bounceScript.isGasiAlertOn)
                {
                    satisfyCondition = false;

                    scriptList[i].clonedBtn.GetComponent<Image>().material = UIGrayscaleMat;
                    for (int k = i + 1; k < scriptList.Count; k++)
                    {
                        if (scriptList[k].belongingIfBlock == scriptList[i].belongingIfBlock)
                            scriptList[k].clonedBtn.GetComponent<Image>().material = UIGrayscaleMat;
                    }

                    yield return new WaitForSeconds(moveInterval);
                    continue;
                }
                else if (scriptList[i].ifType == ifType.ifNotGasi && bounceScript.isGasiAlertOn)
                {
                    satisfyCondition = false;

                    scriptList[i].clonedBtn.GetComponent<Image>().material = UIGrayscaleMat;
                    for (int k = i + 1; k < scriptList.Count; k++)
                    {
                        if (scriptList[k].belongingIfBlock == scriptList[i].belongingIfBlock)
                            scriptList[k].clonedBtn.GetComponent<Image>().material = UIGrayscaleMat;
                    }

                    yield return new WaitForSeconds(moveInterval);
                    continue;
                }

                HighlightButton(scriptList[i].clonedBtn);

                if (scriptList[i].direction == direction.up)
                {
                    if (Physics.Raycast(upRay.position, upRay.forward, out hit, rayLength) && hit.collider.tag != "LeftCar" && hit.collider.tag != "RightCar" && hit.collider.tag != "Bead" && hit.collider.tag != "Water" && hit.collider.tag != "Gasi" && hit.collider.tag != "ArrowGun")
                    {
                        bounceScript.UpCollideInput();
                    }
                    else
                    {
                        bounceScript.UpInput();
                    }
                }
                else if (scriptList[i].direction == direction.down)
                {
                    if (Physics.Raycast(downRay.position, downRay.forward, out hit, rayLength) && hit.collider.tag != "LeftCar" && hit.collider.tag != "RightCar" && hit.collider.tag != "Bead" && hit.collider.tag != "Water" && hit.collider.tag != "Gasi" && hit.collider.tag != "ArrowGun")
                    {
                        bounceScript.DownCollideInput();
                    }
                    else
                    {
                        bounceScript.DownInput();
                    }
                }
                else if (scriptList[i].direction == direction.left)
                {
                    if (Physics.Raycast(leftRay.position, leftRay.forward, out hit, rayLength) && hit.collider.tag != "LeftCar" && hit.collider.tag != "RightCar" && hit.collider.tag != "Bead" && hit.collider.tag != "Water" && hit.collider.tag != "Gasi" && hit.collider.tag != "ArrowGun")
                    {
                        bounceScript.LeftCollideInput();
                    }
                    else
                    {
                        bounceScript.LeftInput();
                    }
                }
                else if (scriptList[i].direction == direction.right)
                {
                    if (Physics.Raycast(rightRay.position, rightRay.forward, out hit, rayLength) && hit.collider.tag != "LeftCar" && hit.collider.tag != "RightCar" && hit.collider.tag != "Bead" && hit.collider.tag != "Water" && hit.collider.tag != "Gasi" && hit.collider.tag != "ArrowGun")
                    {
                        bounceScript.RightCollideInput();
                    }
                    else
                    {
                        bounceScript.RightInput();
                    }
                }
                else if (scriptList[i].direction == direction.stop)
                {
                    // do nothing
                }

                scriptList[i].SetDirection(originalDir);

                for (int j = 0; j < carControllerList.Count; j++)
                {
                    carControllerList[j].CarInput();
                }

                for (int j = 0; j < steppingControllerList.Count; j++)
                {
                    steppingControllerList[j].SteppingInput();
                }

                for (int j = 0; j < trainControllerList.Count; j++)
                {
                    trainControllerList[j].TrainInput();
                }

                for (int j = 0; j < arrowGunControllerList.Count; j++)
                {
                    arrowGunControllerList[j].ArrowInput();
                }

                for (int j = 0; j < gasiControllerList.Count; j++)
                {
                    gasiControllerList[j].GasiInput();
                }

                yield return new WaitForSeconds(moveInterval);
            }
        }

        Time.timeScale = 1f;

        ClearButton();
        scriptList.Clear();
        ifBtnList.Clear();
        curIfMode = ifType.None;
        isPlaying = false;
        ffBtn.interactable = false;
        undoBtn.interactable = true;
        repeatCount = 1;
        playBtn.sprite = RepeatSprite[0];
    }

    public void ClearClicked()
    {
        source.PlayOneShot(selectSound, audioVolume);
        if (isPlaying == false)
        {
            ClearButton();
            scriptList.Clear();
            ifBtnList.Clear();
            curIfMode = ifType.None;
        }
    }

    public void ReplayClicked()
    {
        //source.PlayOneShot(selectSound, audioVolume);
        Application.LoadLevel(Application.loadedLevel);
    }

    public void UndoClicked()
    {
        if (bounceScript.isOver)
            return;

        source.PlayOneShot(selectSound, audioVolume);

        foreach (var obj in movableObjs)
            obj.LoadState();

        //remove in History and get the history info
        var historyInfo = HistoryManager.Instance.RemoveLastHistory();

        // 기존에 입력되어있던거 지워주고
        ClearButton();
        scriptList.Clear();
        ifBtnList.Clear();
        curIfMode = ifType.None;

        //되돌린 스크립트 정보를 불러와 현재 입력상태로 전환
        if (historyInfo.scriptInfo != null)
            scriptList.AddRange(historyInfo.scriptInfo);

        this.repeatCount = historyInfo.repeatCount;
        playBtn.sprite = RepeatSprite[historyInfo.repeatCount == 1 ? 0 : historyInfo.repeatCount];

        GameObject ifBlock = null;
        int ifModeLength = 0;

        ifType prevIfMode = ifType.None;

        for (int i = 0; i < scriptList.Count; i++)
        {
            if (scriptList[i].ifType != ifType.None && scriptList[i].ifType != prevIfMode)
            {
                ifBlock = Instantiate(IfBtn, QueuePanel.transform) as GameObject;
                scriptList[i].belongingIfBlock = ifBlock;

                RectTransform ifBlcokRT = ifBlock.GetComponent<RectTransform>();
                ifBlcokRT.anchoredPosition = new Vector2(i * 208 - 522, 0);
                //ifBlcokRT.localScale = new Vector3(0.65f, 0.65f, 0.65f);

                Transform ifBtn = ifBlock.transform.Find("Button");
                if (ifBtn != null)
                {
                    ifBtn.GetComponent<Button>().enabled = false;

                    switch (scriptList[i].ifType)
                    {
                        case ifType.ifGasi:
                            ifBtn.GetComponent<Image>().sprite = ifBtn_gasi;
                            break;

                        case ifType.ifArrow:
                            ifBtn.GetComponent<Image>().sprite = ifBtn_arrow;
                            break;

                        case ifType.ifNotGasi:
                            ifBtn.GetComponent<Image>().sprite = ifBtn_notGasi;
                            break;

                        case ifType.ifNotArrow:
                            ifBtn.GetComponent<Image>().sprite = ifBtn_notArrow;
                            break;
                    }

                    ifBtn.GetComponent<ButtonHighlightController>().stopBlinkAnim();
                }
                ifModeLength++;
            }
            else if (scriptList[i].ifType != ifType.None)
            {
                if (ifBlock != null)
                {
                    ifBlock.GetComponent<Image>().sprite = IfBlockSprite[ifModeLength];
                    scriptList[i].belongingIfBlock = ifBlock;
                }
            }
            else
            {
                ifBlock = null;
                ifModeLength = 0;
            }

            GameObject btnIns;
            if (scriptList[i].direction == direction.up)
                btnIns = Instantiate(UpBtn, QueuePanel.transform) as GameObject;
            else if (scriptList[i].direction == direction.down)
                btnIns = Instantiate(DownBtn, QueuePanel.transform) as GameObject;
            else if (scriptList[i].direction == direction.left)
                btnIns = Instantiate(LeftBtn, QueuePanel.transform) as GameObject;
            else if (scriptList[i].direction == direction.right)
                btnIns = Instantiate(RightBtn, QueuePanel.transform) as GameObject;
            else// (scriptList[i].direction == direction.stop)
                btnIns = Instantiate(StopBtn, QueuePanel.transform) as GameObject;

            RectTransform rt = btnIns.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(i * 208 - 416, 0);
            if (scriptList[i].ifType != ifType.None)
                rt.sizeDelta *= 0.9f;

            // 생성된 버튼에 몇번 째 명령어인지 데이터 기록
            UserScriptInfo info = scriptList[i];
            scriptList[info.index].clonedBtn = btnIns;
            
            // 입력된 Move명령어에 대해 눌렀을 시 삭제되는 터치 이벤트를 붙인다
            btnIns.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (isPlaying)
                    return;

                // 현재 이프문 편집 중인데 이프문 바깥의 스크립트 삭제할 경우엔 편집중이던 이프 블록 펑~
                if (curIfMode != ifType.None && info.belongingIfBlock != ifBtnList[ifBtnList.Count - 1])
                {
                    int repeatAmount = scriptList.Count;
                    for (int k = repeatAmount - 1; k >= 0; k--)
                    {
                        if (scriptList[k].belongingIfBlock == ifBtnList[ifBtnList.Count - 1])
                        {
                            Destroy(scriptList[k].clonedBtn);
                            scriptList.Remove(scriptList[k]);
                        }
                    }

                    Destroy(ifBtnList[ifBtnList.Count - 1]);
                    ifBtnList.RemoveAt(ifBtnList.Count - 1);

                    curIfMode = ifType.None;
                }

                Destroy(info.clonedBtn);
                //info.clonedBtn = null;

                if (info.ifType != ifType.None)
                {
                    ifType tempIfType = info.ifType;
                    if (curIfMode == ifType.None &&
                        (   (info.index == 0 && scriptList.Count > 1 && scriptList[1].ifType != tempIfType) ||
                            (info.index == 0 && scriptList.Count == 1) ||
                            (info.index != 0 && scriptList[info.index - 1].ifType != tempIfType && info.index == scriptList.Count - 1) ||
                            (info.index != 0 && scriptList[info.index - 1].ifType != tempIfType && scriptList[info.index + 1].ifType != tempIfType) ))
                    {
                        ifBtnList.Remove(info.belongingIfBlock);
                        Destroy(info.belongingIfBlock);
                        info.belongingIfBlock = null;
                    }
                    else
                    {
                        AdjustIfBlockLength(info.belongingIfBlock.GetComponent<Image>(), true);
                    }
                }

                scriptList.RemoveAt(info.index);

                // 중간에 있는 명령어가 없어졌을 경우 뒤에있는 명령어들의 위치를 앞으로 당겨준다
                if (scriptList.Count != info.index)
                {
                    GameObject ifBlockTemp = null;

                    for (int j = info.index; j < scriptList.Count; j++)
                    {
                        RectTransform btnRT = scriptList[j].clonedBtn.GetComponent<RectTransform>();
                        btnRT.anchoredPosition = new Vector2(j * 208 - 416, 0);

                        scriptList[j].index = j;

                        if (scriptList[j].ifType != ifType.None && scriptList[j].belongingIfBlock != info.belongingIfBlock &&
                            scriptList[j].belongingIfBlock != ifBlockTemp)
                        {
                            ifBlockTemp = scriptList[j].belongingIfBlock;
                            RectTransform r = ifBlockTemp.GetComponent<RectTransform>();
                            r.anchoredPosition = new Vector2(r.anchoredPosition.x - 208, 0);
                        }
                    }
                }

                UserScriptInfo tempInfo = null;
                foreach (UserScriptInfo item in scriptList)
                {
                    if (tempInfo == null)
                        tempInfo = item;
                    else
                    {
                        if (item.ifType != ifType.None && tempInfo.ifType != ifType.None &&
                            item.ifType == tempInfo.ifType && item.belongingIfBlock != tempInfo.belongingIfBlock)
                        {
                            ifBtnList.Remove(item.belongingIfBlock);
                            Destroy(item.belongingIfBlock);
                            item.belongingIfBlock = tempInfo.belongingIfBlock;

                            AdjustIfBlockLength(tempInfo.belongingIfBlock.GetComponent<Image>(), false);
                        }

                        tempInfo = item;
                    }
                }
            });
            
            prevIfMode = scriptList[i].ifType;
        } // End of forloop
    }

    private void InitIfMode(ifType ifModeType)
    {
        source.PlayOneShot(selectSound2, audioVolume);

        if (!isPlaying && curIfMode == ifType.None && scriptList.Count < 5)
        {
            curIfMode = ifModeType;

            // 새로운 ifMode에 들어갈 때에만 ifblock을 그려준다
            // 05.02 기획 변경으로 이프문 종료 후 동일한 종류의 이프문 바로 시작해도 새로운 ifBlock으로 그려주려고 했으나 사이드이펙트가 커서 나중에 수정할 예정
            if (scriptList.Count == 0 || scriptList[scriptList.Count - 1].ifType != curIfMode)
            {
                var ifBlock = Instantiate(IfBtn, QueuePanel.transform) as GameObject;
                ifBtnList.Add(ifBlock);

                RectTransform rt = ifBlock.GetComponent<RectTransform>();
                rt.anchoredPosition = new Vector2((scriptList.Count) * 208 - 522, 0);
                rt.localScale = Vector3.one;

                // Set ifBtn image that is on the ifBlock
                ifBtn = ifBlock.transform.Find("Button");
                if (ifBtn != null)
                {
                    switch (curIfMode)
                    {
                        case ifType.ifGasi:
                            ifBtn.GetComponent<Image>().sprite = ifBtn_gasi;
                            break;

                        case ifType.ifArrow:
                            ifBtn.GetComponent<Image>().sprite = ifBtn_arrow;
                            break;

                        case ifType.ifNotGasi:
                            ifBtn.GetComponent<Image>().sprite = ifBtn_notGasi;
                            break;

                        case ifType.ifNotArrow:
                            ifBtn.GetComponent<Image>().sprite = ifBtn_notArrow;
                            break;
                    }

                    ifBtn.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        if (scriptList.Count == 0 || (scriptList[scriptList.Count - 1].ifType != curIfMode && curIfMode != ifType.None))
                        {
                            if (ifBtnList.Count != 0)
                            {
                                var latestIfBtn = ifBtnList[ifBtnList.Count - 1];
                                ifBtnList.RemoveAt(ifBtnList.Count - 1);
                                Destroy(latestIfBtn);
                            }
                        }
                        curIfMode = ifType.None;
                        buttonScript.stopBlinkAnim();
                        //이프문 종료 후 다시 해당 이프문 편집으로 진입하고 싶은 경우 때문에 밑줄 주석 처리했다가...
                        //05.02 그냥 이프문을 삭제하고 다시 만드는 걸로 UX를 변경함에 따라 주석 해제 하려다가.... 사이드이펙트가 커서 나중에하기로
                        //ifBtn.GetComponent<Button>().onClick.RemoveAllListeners();
                    });
                }// end of Set ifBtn image that is on the ifBlock
            }
            else // 새 블록이 아니라 기존거에 연결되는거일 때, 애니메이션을 다시 켜준다
            {
                if (buttonScript != null)
                {
                    buttonScript.blinkAnim();
                }
            }
            
            buttonScript = ifBtn.GetComponent<ButtonHighlightController>();
        }
    }

    void SetButtonEvent()
    {
        /* nextStageBtn */
        var nextStageBtn = GameObject.Find("ParentClearPanel").transform.Find("ClearPanel").transform.Find("NextButton");
        if (nextStageBtn != null)
        {
            nextStageBtn.GetComponent<Button>().onClick.AddListener(() =>
            {
                var curStageName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
                int curStageNum = curStageName[curStageName.Length - 1] - '0'; // (int)Char.GetNumericValue will works.

                if (curStageNum > 0 && curStageNum < DataManager.Instance.stagesPerChapter)
                {
                    StringBuilder sb = new StringBuilder(curStageName);
                    if (curStageNum == DataManager.Instance.stagesPerChapter - 1)
                    {
                        sb.Remove(sb.Length - 1, 1);
                        sb.Append(DataManager.Instance.stagesPerChapter.ToString());
                    }
                    else
                    {
                        sb[sb.Length - 1] = (char)(curStageNum + 1 + '0');
                    }
                    var nextStageName = sb.ToString();

                    UnityEngine.SceneManagement.SceneManager.LoadScene(nextStageName);
                }
                else if (curStageNum == DataManager.Instance.stagesPerChapter)
                {
                    // TODO : 챕터 클리어 시 이벤트 처리
                }
                else
                    Debug.Log("error");
            });
        }

        /* PlayBtn */
        var playBtn = GameObject.Find("QueuePanel").transform.Find("PlayBtn");
        if (playBtn != null)
        {
            playBtn.GetComponent<Button>().onClick.AddListener(() =>
            {
                Play();
            });
        }

        /* UpBtn */
        var upBtn = GameObject.Find("TabPanel").transform.Find("Tab1").transform.Find("UpBtn");
        if (upBtn != null)
        {
            upBtn.GetComponent<Button>().onClick.AddListener(() =>
            {
                UpClicked();
            });
        }

        /* DownBtn */
        var downBtn = GameObject.Find("TabPanel").transform.Find("Tab1").transform.Find("DownBtn");
        if (downBtn != null)
        {
            downBtn.GetComponent<Button>().onClick.AddListener(() =>
            {
                DownClicked();
            });
        }

        /* LeftBtn */
        var leftBtn = GameObject.Find("TabPanel").transform.Find("Tab1").transform.Find("LeftBtn");
        if (leftBtn != null)
        {
            leftBtn.GetComponent<Button>().onClick.AddListener(() =>
            {
                LeftClicked();
            });
        }

        /* RightBtn */
        var rightBtn = GameObject.Find("TabPanel").transform.Find("Tab1").transform.Find("RightBtn");
        if (rightBtn != null)
        {
            rightBtn.GetComponent<Button>().onClick.AddListener(() =>
            {
                RightClicked();
            });
        }

        /* StopBtn */
        var stopBtn = GameObject.Find("TabPanel").transform.Find("Tab1").transform.Find("StopBtn");
        if (stopBtn != null)
        {
            stopBtn.GetComponent<Button>().onClick.AddListener(() =>
            {
                StopClicked();
            });
        }

        ffBtn.onClick.AddListener(() =>
        {
            Time.timeScale = 1.6f;
        });
    }

}
