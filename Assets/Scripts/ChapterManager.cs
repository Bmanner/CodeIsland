using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class ChapterManager : MonoBehaviour {
    [Serializable]
    public struct Chapter
    {
        public GameObject chapter;
        public Sprite unlockImage;
    }

    public AudioClip selectSound;

    public Chapter[] Chapters;

	// Use this for initialization
	void Awake () {
        
        int unlockedChapterCount = DataManager.Instance.GetClearedStageCount() / DataManager.Instance.stagesPerChapter;

        for (int i = 0; i <= unlockedChapterCount; i++)
        {
            int currentChapter = i;

            Chapters[i].chapter.GetComponent<Image>().sprite = Chapters[i].unlockImage;
            Chapters[i].chapter.GetComponent<Button>().onClick.AddListener(() =>
            {
                DataManager.Instance.currentChapter = (ChapterEnum)currentChapter;

                SceneManager.LoadScene("Stage_Select");
            });
        }
	}
	
}
