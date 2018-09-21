using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class PanelController : MonoBehaviour {

    static readonly int UISLIDINGDEG = 120;

    public delegate void ClearDelegate();
    ClearDelegate clearCallback;

    public Sprite[] tabPanelSprite;
    public Sprite tab_on;
    public Sprite tab_off;

    ButtonEvent buttonEvent;

    Transform TabPanel;
    Transform Tab1;
    Transform Tab2;
    Transform Tab3;
    Transform TabBtn1;
    Transform TabBtn2;
    Transform TabBtn3;

    Transform ClearPanel;
    Transform FailPanel;
    Transform PausePanel;
    Transform BlackCurtain;
    Transform cube1;
    Transform cube2;
    Transform cube3;
    Transform star1;
    Transform star2;
    Transform star3;

    Transform QueuePanel;
    Transform CategoryPanel;
    Transform HistoryPanel;
    Transform CMinBtn;
    Transform CMaxBtn;
    Transform HMinBtn;
    Transform HMaxBtn;

    bool isPausePanel = true;  // PausePanel이 켜질 수 있는 상태 : true

    Text timeText;

    void Awake()
    {
        Application.targetFrameRate = 30;
    }

    void Start ()
    {
        buttonEvent = GameObject.Find("Canvas/ButtonEvent").GetComponent<ButtonEvent>();

        QueuePanel = GameObject.Find("QueuePanel").transform;
        CategoryPanel = GameObject.Find("CategoryPanel").transform;
        HistoryPanel = GameObject.Find("HistoryPanel").transform;

        TabPanel = GameObject.Find("TabPanel").transform;
        Tab1 = GameObject.Find("TabPanel").transform.Find("Tab1");
        Tab2 = GameObject.Find("TabPanel").transform.Find("Tab2");
        Tab3 = GameObject.Find("TabPanel").transform.Find("Tab3");
        TabBtn1 = CategoryPanel.Find("TabBtn1");
        TabBtn2 = CategoryPanel.Find("TabBtn2");
        TabBtn3 = CategoryPanel.Find("TabBtn3");

        ClearPanel = GameObject.Find("ParentClearPanel").transform.Find("ClearPanel");
        FailPanel = GameObject.Find("ParentFailPanel").transform.Find("FailPanel");
        PausePanel = GameObject.Find("ParentPausePanel").transform.Find("PausePanel");
        BlackCurtain = GameObject.Find("ParentBlackCurtain").transform.Find("BlackCurtain");

        cube1 = GameObject.Find("ParentCube1").transform.Find("Cube1");
        cube2 = GameObject.Find("ParentCube2").transform.Find("Cube2");
        cube3 = GameObject.Find("ParentCube3").transform.Find("Cube3");

        CMinBtn = QueuePanel.Find("MinBtn");
        CMaxBtn = QueuePanel.Find("MaxBtn");
        HMinBtn = HistoryPanel.Find("MinBtn");
        HMaxBtn = HistoryPanel.Find("MaxBtn");

        SetButtonEvent();
        
        timeText = GameObject.Find("Canvas/TopPanel/Time/Text").GetComponent<Text>();
    }

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePausePanel();
        }

        if(Input.GetKeyDown(KeyCode.F1))
        {
            /*
            isPausePanel = false;

            ClearPanel.gameObject.SetActive(true);
            BlackCurtain.gameObject.SetActive(true);

            /* UI가 stage scene별로 있으므로 10스테이지의 다시하기 버튼만 UI에서 제거해줌
            // 10번째 스테이지인 경우 다음 스테이지 버튼을 꺼준다
            var curStageName = SceneManager.GetActiveScene().name;
            if(curStageName[curStageName.Length - 1] == '0')
            {
                var nextBtn = ClearPanel.FindChild("NextButton");
                if (nextBtn != null)
                    nextBtn.GetComponent<UnityEngine.UI.Button>().interactable = false;
            }
            
            GameObject.Find("ParentStar1").transform.FindChild("Star1").gameObject.SetActive(true);
            GameObject.Find("ParentStar2").transform.FindChild("Star2").gameObject.SetActive(true);
            GameObject.Find("ParentStar3").transform.FindChild("Star3").gameObject.SetActive(true);

            DataManager.Instance.AddDataStageClear(SceneManager.GetActiveScene().name, 2);
            */

            OnClearPanel(3, 0f, 0);
        }
    }
    
    void SetButtonEvent ()
    {
        /* TabBtn1 */
        if (TabBtn1 != null)
        {
            TabBtn1.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (Tab1.gameObject.activeSelf == false)
                {
                    TabBtn2.GetComponent<Image>().sprite = tab_off;
                    TabBtn3.GetComponent<Image>().sprite = tab_off;
                    Tab2.gameObject.SetActive(false);
                    Tab3.gameObject.SetActive(false);

                    TabPanel.GetComponent<Image>().sprite = tabPanelSprite[0];
                    TabBtn1.GetComponent<Image>().sprite = tab_on;
                    Tab1.gameObject.SetActive(true);
                }
            });
        }

        /* TabBtn2 */
        if (TabBtn2 != null)
        {
            TabBtn2.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (Tab2.gameObject.activeSelf == false)
                {
                    TabBtn1.GetComponent<Image>().sprite = tab_off;
                    TabBtn3.GetComponent<Image>().sprite = tab_off;
                    Tab1.gameObject.SetActive(false);
                    Tab3.gameObject.SetActive(false);

                    TabPanel.GetComponent<Image>().sprite = tabPanelSprite[1];
                    TabBtn2.GetComponent<Image>().sprite = tab_on;
                    Tab2.gameObject.SetActive(true);
                }
            });
        }

        /* TabBtn3 */
        if (TabBtn3 != null)
        {
            TabBtn3.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (Tab3.gameObject.activeSelf == false)
                {
                    TabBtn1.GetComponent<Image>().sprite = tab_off;
                    TabBtn2.GetComponent<Image>().sprite = tab_off;
                    Tab1.gameObject.SetActive(false);
                    Tab2.gameObject.SetActive(false);

                    TabPanel.GetComponent<Image>().sprite = tabPanelSprite[2];
                    TabBtn3.GetComponent<Image>().sprite = tab_on;
                    Tab3.gameObject.SetActive(true);
                }
            });
        }

        if (CMinBtn != null)
        {
            CMinBtn.GetComponent<Button>().onClick.AddListener(() =>
            {
                CMinBtn.gameObject.SetActive(false);
                CMaxBtn.gameObject.SetActive(true);
                //CategoryPanel.gameObject.SetActive(false);

                //IEnumerator coroutine = PanelMoveDown();
                StartCoroutine("CPanelMoveDown");


            });
        }

        if (CMaxBtn!= null)
        {
            CMaxBtn.GetComponent<Button>().onClick.AddListener(() =>
            {
                CMinBtn.gameObject.SetActive(true);
                CMaxBtn.gameObject.SetActive(false);

                StartCoroutine("CPanelMoveUp");
            });
        }

        if (HMinBtn != null)
        {
            HMinBtn.GetComponent<Button>().onClick.AddListener(() =>
            {
                HMinBtn.gameObject.SetActive(false);
                HMaxBtn.gameObject.SetActive(true);
                
                StartCoroutine("HPanelClose");
            });
        }

        if (HMaxBtn != null)
        {
            HMaxBtn.GetComponent<Button>().onClick.AddListener(() =>
            {
                HMinBtn.gameObject.SetActive(true);
                HMaxBtn.gameObject.SetActive(false);

                StartCoroutine("HPanelOpen");
            });
        }
    }

    IEnumerator CPanelMoveUp()
    {
        RectTransform CPPos = CategoryPanel.GetComponent<RectTransform>();
        RectTransform QPPos = QueuePanel.GetComponent<RectTransform>();

        int maxY = (int)(290 / Mathf.Sin(UISLIDINGDEG * Mathf.Deg2Rad));

        int deg = 0;
        int startY = (int)CPPos.anchoredPosition.y;
        int endY = startY + maxY;
        int y = 0;

        while (deg <= UISLIDINGDEG)
        {
            y = (int)Mathf.Lerp(startY, endY, Mathf.Sin(deg * Mathf.Deg2Rad));

            CPPos.anchoredPosition = new Vector2(CPPos.anchoredPosition.x, y);
            QPPos.anchoredPosition = new Vector2(QPPos.anchoredPosition.x, y + 349);

            deg += 8;

            yield return null;
        }
    }

    IEnumerator CPanelMoveDown()
    {
        RectTransform CPPos = CategoryPanel.GetComponent<RectTransform>();
        RectTransform QPPos = QueuePanel.GetComponent<RectTransform>();

        int maxY = (int)(290 / Mathf.Sin(UISLIDINGDEG * Mathf.Deg2Rad));

        int deg = 0;
        int startY = (int)CPPos.anchoredPosition.y;
        int endY = startY - maxY; 
        int y = 0;

        while (deg <= UISLIDINGDEG)
        {
            y = (int)Mathf.Lerp(startY, endY, Mathf.Sin(deg * Mathf.Deg2Rad));

            CPPos.anchoredPosition= new Vector2(CPPos.anchoredPosition.x, y);
            QPPos.anchoredPosition = new Vector2(QPPos.anchoredPosition.x, y + 349);

            deg += 8;

            yield return null;
        }
    }

    IEnumerator HPanelOpen()
    {
        RectTransform panelPos = HistoryPanel.GetComponent<RectTransform>();

        int maxX = (int)(980 / Mathf.Sin(UISLIDINGDEG * Mathf.Deg2Rad));

        int deg = 0;
        int startX = (int)panelPos.anchoredPosition.x;
        int endX = startX + maxX;
        int x = 0;

        while (deg <= UISLIDINGDEG)
        {
            x = (int)Mathf.Lerp(startX, endX, Mathf.Sin(deg * Mathf.Deg2Rad));

            panelPos.anchoredPosition = new Vector2(x, panelPos.anchoredPosition.y);

            deg += 8;

            yield return null;
        }
    }

    IEnumerator HPanelClose()
    {
        RectTransform panelPos = HistoryPanel.GetComponent<RectTransform>();

        int maxX = (int)(980 / Mathf.Sin(UISLIDINGDEG * Mathf.Deg2Rad));

        int deg = 0;
        int startX = (int)panelPos.anchoredPosition.x;
        int endX = startX - maxX;
        int x = 0;

        while (deg <= UISLIDINGDEG)
        {
            x = (int)Mathf.Lerp(startX, endX, Mathf.Sin(deg * Mathf.Deg2Rad));

            panelPos.anchoredPosition = new Vector2(x, panelPos.anchoredPosition.y);

            deg += 8;//(int) (120 * 3 * Time.deltaTime);

            yield return null;
        }
        
        panelPos.anchoredPosition = new Vector2(-1200, panelPos.anchoredPosition.y);
    }
    /*
    IEnumerator HistoryButtonHide()
    {
        RectTransform panelPos = HistoryPanel.GetComponent<RectTransform>();

        int deg = 0;
        int startX = (int)panelPos.anchoredPosition.x;
        int endX = startX - maxX; 
        int x = 0;

        while (deg <= UISLIDINGDEG)
        {
            

            panelPos.anchoredPosition = new Vector2(x, panelPos.anchoredPosition.y);

            deg += 8;

            yield return null;
        }
    }

    IEnumerator HistoryButtonShow()
    {

    }*/

    public void OnClearPanel(int count, float clearTime, int numOfScript)
    {
        buttonEvent.StopAllCoroutines();

        if(clearCallback != null)
            clearCallback();

        isPausePanel = false;

        ClearPanel.gameObject.SetActive(true);
        BlackCurtain.gameObject.SetActive(true);

        /* UI가 stage scene별로 있으므로 10스테이지의 다시하기 버튼만 UI에서 제거해줌
        // 10번째 스테이지인 경우 다음 스테이지 버튼을 꺼준다
        var curStageName = SceneManager.GetActiveScene().name;
        if(curStageName[curStageName.Length - 1] == '0')
        {
            var nextBtn = ClearPanel.FindChild("NextButton");
            if (nextBtn != null)
                nextBtn.GetComponent<UnityEngine.UI.Button>().interactable = false;
        }
        */

        TimeFormatHelper TFH = new TimeFormatHelper();

        Text timeText = ClearPanel.Find("Time/Text").GetComponent<Text>();
        timeText.text = TFH.GetTimeFormat(clearTime);

        Text numOfScrptText = ClearPanel.Find("ScriptCount/Text").GetComponent<Text>();
        var SB = new StringBuilder(numOfScript.ToString());
        SB.Append("개"); // Localization

        numOfScrptText.text = SB.ToString();

        star1 = ClearPanel.Find("ParentStar1").Find("Star1");
        star2 = ClearPanel.Find("ParentStar2").Find("Star2");
        star3 = ClearPanel.Find("ParentStar3").Find("Star3");

        if (count == 1)
        {
            star1.gameObject.SetActive(true);
        }

        if (count == 2)
        {
            star1.gameObject.SetActive(true);
            star2.gameObject.SetActive(true);
        }

        if (count == 3)
        {
            star1.gameObject.SetActive(true);
            star2.gameObject.SetActive(true);
            star3.gameObject.SetActive(true);
        }

        DataManager.Instance.AddDataStageClear(SceneManager.GetActiveScene().name, count);


        /* 스테이지 클리어 업적 달성 */
        
        if (Social.localUser.authenticated) // 로그인이 되어 있는지 확인
        {
            if (SceneManager.GetActiveScene().name == "Chapter1Stage_8")
            {
                // unlock achievement (achievement_1)
                Social.ReportProgress("CgkI0Njd1fQSEAIQBg", 100.0f, (bool success) => {
                    // handle success or failure
                });
            }
            else if (SceneManager.GetActiveScene().name == "Chapter2Stage_8")
            {
                // unlock achievement (achievement_2)
                Social.ReportProgress("CgkI0Njd1fQSEAIQBw", 100.0f, (bool success) => {
                    // handle success or failure
                });
            }
            else if (SceneManager.GetActiveScene().name == "Chapter3Stage_8")
            {
                // unlock achievement (achievement_3)
                Social.ReportProgress("CgkI0Njd1fQSEAIQCA", 100.0f, (bool success) => {
                    // handle success or failure
                });
            }
            else if (SceneManager.GetActiveScene().name == "Chapter4Stage_8")
            {
                // unlock achievement (achievement_4)
                Social.ReportProgress("CgkI0Njd1fQSEAIQCQ", 100.0f, (bool success) => {
                    // handle success or failure
                });
            }
        }
        
    }
    

    public void OnFailPanel(float clearTime, int numOfScript)
    {
        isPausePanel = false;

        TimeFormatHelper TFH = new TimeFormatHelper();

        Text timeText = FailPanel.Find("Time/Text").GetComponent<Text>();
        timeText.text = TFH.GetTimeFormat(clearTime);

        Text numOfScrptText = FailPanel.Find("ScriptCount/Text").GetComponent<Text>();
        var SB = new StringBuilder(numOfScript.ToString());
        SB.Append("개"); // Localization

        numOfScrptText.text = SB.ToString();

        FailPanel.gameObject.SetActive(true);
        BlackCurtain.gameObject.SetActive(true);
    }

    public void TogglePausePanel()
    {
        if (isPausePanel == true)
        {
            if (PausePanel.gameObject.activeSelf == true)
            {
                Time.timeScale = 1;

                PausePanel.gameObject.SetActive(false);
                BlackCurtain.gameObject.SetActive(false);
            }
            else
            {
                Time.timeScale = 0;

                PausePanel.gameObject.SetActive(true);
                BlackCurtain.gameObject.SetActive(true);
            }
        }
    }

    public void CubeController(int count)
    {

        if(count == 0)
        {
            cube1.gameObject.SetActive(false);
            cube2.gameObject.SetActive(false);
            cube3.gameObject.SetActive(false);
        }
        else if(count == 1)
        {
            cube1.gameObject.SetActive(true);
            cube2.gameObject.SetActive(false);
            cube3.gameObject.SetActive(false);
        }
        else if (count == 2)
        {
            cube1.gameObject.SetActive(true);
            cube2.gameObject.SetActive(true);
            cube3.gameObject.SetActive(false);
        }
        else if (count == 3)
        {
            cube1.gameObject.SetActive(true);
            cube2.gameObject.SetActive(true);
            cube3.gameObject.SetActive(true);
        }
    }

    public void UpdateElapseTime(float time)
    {
        if (timeText != null)
        {
            TimeFormatHelper TFH = new TimeFormatHelper();
            timeText.text = TFH.GetTimeFormat(time);
        }
    }

    public void SetClearCallback(ClearDelegate callback)
    {
        clearCallback = callback;
    }
}
