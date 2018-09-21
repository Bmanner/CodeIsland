using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class StageManager : MonoBehaviour {
    public int StarPosYFromStageBtn = 0; // 별 위치가 스테이지 버튼 아래쪽에 위치하도록 해주는 값. (기기별 동일한 값인지 확인 필요)

    public Sprite[] UnlockImage;
    public Sprite StarImage;
    public Sprite BlackStarImage;

    public Sprite Chapter1BG;
    public Sprite Chapter2BG;
    public Sprite Chapter3BG;
    public Sprite Chapter4BG;


    public GameObject Star;

    private readonly int _numberOfStar = 3; //스코어는 별 3개까지 있음


    // Use this for initialization
    void Awake ()
    {
        var curChapter = DataManager.Instance.currentChapter; Debug.Log("current chapter is " + curChapter.ToString());
        int stagesPerChapter = DataManager.Instance.stagesPerChapter;
        var curChapterUnlockedStageCount = ( DataManager.Instance.GetClearedStageCount() - ((int)curChapter * stagesPerChapter) );
        if (curChapterUnlockedStageCount > DataManager.Instance.stagesPerChapter)
            curChapterUnlockedStageCount = DataManager.Instance.stagesPerChapter;

        Transform[] stages = transform.GetComponentsInChildren<Transform>();

        int score;
        // GetComponentsInChildren에 자기 자신이 포함되므로 int i = 0 이 아닌 1부터 시작한다 (자신 제외)
        for(int i = 1; i < stages.Length; i++)
        {
            var stage = stages[i];

            score = DataManager.Instance.GetStageScore(curChapter.ToString() + stage.name);

            if (i == curChapterUnlockedStageCount + 1)
                score = 0;

            if ( score >= 0)
            {
                var starPrefab = (GameObject)Instantiate(Star, stage, false);

                var stars = new Transform[starPrefab.transform.childCount];
                for (int k = 0; k < _numberOfStar; k++)
                {
                    stars[k] = starPrefab.transform.Find("Star" + (k + 1));
                    if(k < score)
                        stars[k].GetComponent<Image>().sprite = StarImage;
                    else
                        stars[k].GetComponent<Image>().sprite = BlackStarImage;
                }

                stage.GetComponent<Image>().sprite = UnlockImage[i-1];

                stage.GetComponent<Button>().onClick.AddListener(() =>
                {
                    SceneManager.LoadScene(curChapter.ToString() + stage.name);
                });
            }
            else
            {
                //stage.GetComponent<Image>().sprite = LockImage;
            }

        }
        /*
        stars = new Transform[transform.childCount];

        var score = DataManager.GetInstance().GetStageScore("Stage_1");

        for(int i = 0; i < score; i++)
        {
            stars[i] = transform.FindChild("Star" + (i + 1));
            stars[i].GetComponent<Image>().sprite = Star;
        }*/


        // 현재 챕터에 맞게 BG이미지를 바꿔준다
        var BG = GameObject.Find("0_BG");
        if (BG != null)
        {
            var BGImage = BG.GetComponent<Image>();
            if (BGImage != null)
            {
                switch (curChapter)
                {
                    case ChapterEnum.Chapter1:
                        BGImage.sprite = Chapter1BG; break;

                    case ChapterEnum.Chapter2:
                        BGImage.sprite = Chapter2BG; break;

                    case ChapterEnum.Chapter3:
                        BGImage.sprite = Chapter3BG; break;

                    case ChapterEnum.Chapter4:
                        BGImage.sprite = Chapter4BG; break;
                }
            }
        }

	}
    

}
