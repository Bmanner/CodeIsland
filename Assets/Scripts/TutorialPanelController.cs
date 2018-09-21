using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;
using System.Collections;

public class TutorialPanelController : MonoBehaviour {

    [Serializable]
    public struct TutorialList
    {
        public Sprite Dialogue;
        public GameObject Button;
        public GameObject HandIcon;
    }

    //public Sprite[] DialogueList;
    public TutorialList[] tutorialList;
    public bool ShowUIGuide = false;
    public bool ShowOnClear = false;

    public AudioClip ButtonSelectSound;
    public float AudioVolume;

    AudioSource audioSrc;
    Transform uiGuide;
    Transform dialogue;
    Transform blackCurtain;
    Image dialogueImage;

    int index;

    void Start () {

        if(tutorialList.Length <= 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            index = 0;

            uiGuide = transform.Find("UIGuide");
            dialogue = transform.Find("Dialogue");
            blackCurtain = transform.Find("BlackCurtain");

            audioSrc = GetComponent<AudioSource>();

            dialogueImage = dialogue.GetComponent<Image>();
            dialogueImage.sprite = tutorialList[index].Dialogue;

            if (!ShowOnClear)
            {
                blackCurtain.gameObject.SetActive(true);
                dialogue.gameObject.SetActive(true);
                index++;
            }
            else
            {
                var PanelController = GameObject.Find("Canvas/PanelController").GetComponent<PanelController>();

                PanelController.SetClearCallback(callback: () =>
                {
                    blackCurtain.gameObject.SetActive(true);
                    dialogue.gameObject.SetActive(true);
                    index++;
                });
            }

            uiGuide.GetComponent<Button>().onClick.AddListener(call: () =>
            {
                audioSrc.PlayOneShot(ButtonSelectSound, AudioVolume);
                uiGuide.gameObject.SetActive(false);
                gameObject.SetActive(false);
            });
            dialogue.GetComponent<Button>().onClick.AddListener(call: () =>
            {
                if (tutorialList[index - 1].Button != null)
                    return; 

                if (index < tutorialList.Length && tutorialList[index].Button != null)
                {
                    var test = Instantiate(tutorialList[index].Button, gameObject.transform.GetChild(0), true);
                    test.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        HLButtonCB(test);
                    });
                }

                audioSrc.PlayOneShot(ButtonSelectSound, AudioVolume);
                if (index < tutorialList.Length)
                {
                    dialogueImage.sprite = tutorialList[index].Dialogue;
                    index++;
                }
                else
                {
                    dialogue.gameObject.SetActive(false);
                    blackCurtain.gameObject.SetActive(false);

                    if (ShowUIGuide)
                        //uiGuide.gameObject.SetActive(true);
                        StartCoroutine(ShowUIGuideWhenDoneMoving());
                    else
                        gameObject.SetActive(false);
                }

                if (index - 1 < tutorialList.Length && tutorialList[index - 1].HandIcon != null)
                    tutorialList[index - 1].HandIcon.SetActive(true);

                if (index - 2 > -1 && tutorialList[index - 2].HandIcon != null)
                    tutorialList[index - 2].HandIcon.SetActive(false);
            });
        }


        /*
        if (DialogueList.Length <= 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            index = 0;
            
            uiGuide = transform.Find("UIGuide");
            dialogue = transform.Find("Dialogue");
            blackCurtain = transform.Find("BlackCurtain");

            audioSrc = GetComponent<AudioSource>();

            dialogueImage = dialogue.GetComponent<Image>();
            dialogueImage.sprite = DialogueList[index];

            if (!ShowOnClear)
            {
                blackCurtain.gameObject.SetActive(true);
                dialogue.gameObject.SetActive(true);
                index++;
            }
            else
            {
                var PanelController = GameObject.Find("Canvas/PanelController").GetComponent<PanelController>();

                PanelController.SetClearCallback(callback: () =>
                {
                    blackCurtain.gameObject.SetActive(true);
                    dialogue.gameObject.SetActive(true);
                    index++;
                });
            }

            uiGuide.GetComponent<Button>().onClick.AddListener(call: () =>
            {
                audioSrc.PlayOneShot(ButtonSelectSound, AudioVolume);
                uiGuide.gameObject.SetActive(false);
                gameObject.SetActive(false);
            });
            dialogue.GetComponent<Button>().onClick.AddListener(call: () =>
            {
                audioSrc.PlayOneShot(ButtonSelectSound, AudioVolume);
                if (index < DialogueList.Length)
                {
                    dialogueImage.sprite = DialogueList[index];
                    index++;
                }
                else
                {
                    dialogue.gameObject.SetActive(false);
                    blackCurtain.gameObject.SetActive(false);

                    if (ShowUIGuide)
                        uiGuide.gameObject.SetActive(true);
                    else
                        gameObject.SetActive(false);
                }
            });
        }*/
    }    

    void HLButtonCB(GameObject buttonSelf)
    {
        if (index < tutorialList.Length && tutorialList[index].Button != null)
        {
            var test = Instantiate(tutorialList[index].Button, gameObject.transform.GetChild(0), true);
            test.GetComponent<Button>().onClick.AddListener(call: () =>
            {
                HLButtonCB(test);
            });

            dialogueImage.sprite = tutorialList[index].Dialogue;

            tutorialList[index - 1].Button.GetComponent<Button>().onClick.Invoke();
            Destroy(buttonSelf);
            index++;
        }
        else
        {
            if (index < tutorialList.Length)
            {
                dialogueImage.sprite = tutorialList[index].Dialogue;
            }
            else
            {
                dialogue.gameObject.SetActive(false);
                blackCurtain.gameObject.SetActive(false);

                if (ShowUIGuide)
                    //uiGuide.gameObject.SetActive(true);
                    StartCoroutine(ShowUIGuideWhenDoneMoving());
                else
                    gameObject.SetActive(false);
            }
            tutorialList[index-1].Button.GetComponent<Button>().onClick.Invoke();
            Destroy(buttonSelf);
            index++;
        }

        if (index - 1 < tutorialList.Length && tutorialList[index - 1].HandIcon != null)
            tutorialList[index-1].HandIcon.SetActive(true);

        if (index - 2 > -1 && tutorialList[index - 2].HandIcon != null)
            tutorialList[index - 2].HandIcon.SetActive(false);
    }

    IEnumerator ShowUIGuideWhenDoneMoving()
    {
        ButtonEvent BE = GameObject.Find("Canvas/ButtonEvent").GetComponent<ButtonEvent>();

        yield return new WaitForSeconds(0.2f);

        while(BE.isPlaying)
        {
            yield return null;
        }

        uiGuide.gameObject.SetActive(true);
    }

    Vector2 GetWorldPosNonRelToPivot(Transform tf)
    {
        var posWithoutPivot = new Vector3(tf.position.x, tf.position.y);

        var pivot = tf.GetComponent<RectTransform>().pivot;

        //if (pivot.x == 0)

        return posWithoutPivot;
    }

}
