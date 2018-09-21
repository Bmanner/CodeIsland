using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class HistoryLineController : MonoBehaviour
{

    public GameObject UpBtn;
    public GameObject DownBtn;
    public GameObject LeftBtn;
    public GameObject RightBtn;
    public GameObject StopBtn;
    public GameObject IfBtn;

    public Sprite[] RepeatSprites;

    public Sprite[] IfBlockSprite;
    public Sprite ifBtn_arrow;
    public Sprite ifBtn_notArrow;
    public Sprite ifBtn_gasi;
    public Sprite ifBtn_notGasi;

    Text lineNumber;
    Image repeatImgComp;

    bool hasListener = false;

    // Use this for initialization
    void Awake()
    {
        lineNumber = transform.Find("LineNumber/Text").GetComponent<Text>();
        repeatImgComp = transform.Find("Repeat").GetComponent<Image>();
    }

    void Start()
    {
        transform.localScale = Vector2.one;
    }

    public void Initialize(List<UserScriptInfo> scriptList, int repeatCount, int lineNumber, ButtonEvent buttonEvent)
    {
        GameObject btnIns;
        GameObject ifBlock = null;
        int ifModeLength = 0;

        Transform scriptBox = transform.Find("ScriptBox");

        Transform[] scripts = scriptBox.GetComponentsInChildren<Transform>();
        for (int i = 1; i < scripts.Length; i++) //자기자신 제외하고 [1]부터 시작
            Destroy(scripts[i].gameObject);

        ifType prevIfMode = ifType.None;

        this.lineNumber.text = lineNumber.ToString();

        for (int i = 0; i < scriptList.Count; i++)
        {
            if (scriptList[i].ifType != ifType.None && scriptList[i].ifType != prevIfMode)
            {
                ifBlock = Instantiate(IfBtn, scriptBox) as GameObject;

                RectTransform ifBlcokRT = ifBlock.GetComponent<RectTransform>();
                ifBlcokRT.anchoredPosition = new Vector2(i * 135 - 336f, 0);
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
                }
                ifModeLength++;
            }
            else if (scriptList[i].ifType != ifType.None)
            {
                if (ifBlock != null)
                    ifBlock.GetComponent<Image>().sprite = IfBlockSprite[ifModeLength];
            }
            else
            {
                ifBlock = null;
                ifModeLength = 0;
            }

            if (scriptList[i].direction == direction.up)
                btnIns = Instantiate(UpBtn, scriptBox) as GameObject;
            else if (scriptList[i].direction == direction.down)
                btnIns = Instantiate(DownBtn, scriptBox) as GameObject;
            else if (scriptList[i].direction == direction.left)
                btnIns = Instantiate(LeftBtn, scriptBox) as GameObject;
            else if (scriptList[i].direction == direction.right)
                btnIns = Instantiate(RightBtn, scriptBox) as GameObject;
            else// (scriptList[i].direction == direction.stop)
                btnIns = Instantiate(StopBtn, scriptBox) as GameObject;

            RectTransform rt = btnIns.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(i * 135 - 270f, 0);
            rt.localScale = new Vector3(0.65f, 0.65f, 0.65f);

            prevIfMode = scriptList[i].ifType;
        } // End of forloop

        repeatImgComp.sprite = RepeatSprites[repeatCount - 1];
    }

}
