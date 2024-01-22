using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class startQuest : MonoBehaviour
{

    public GameTimer timerScript;
    public UIManager UiManager;
    public Animator factoryOpen;
    public AudioSource gate_src;
    public AudioClip gate_open;
    
    public int timeInSeconds = 300;
    public int itemAmount = 10;

    private int initialScore;
    private bool firstInteraction = true;
    private bool questActive = false;
    private bool questArchieved = false;
    private bool start = true;
    private Coroutine questCoroutine; // Coroutine reference for interaction text
    // Start is called before the first frame update
    void Start()
    {
    }

    void PlayIntro()
    {
        string[] intro = new string[]
{
                "Hi, welcome to your first day on the job as a trashman. As you can see, this field is littered with trash.",
                "It is your responsibility to collect this trash, and throw it into the truck.",
                "Now go, before I fire your ass!"
};
        UiManager.TypeDialogue(intro);
    }

    void DetectObject()
    {
        // Detect object with raycast
        float rayDistance = 2.5f;

        // ray from view
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit rayHit;

        // Check hit
        if (Physics.Raycast(ray, out rayHit, rayDistance))
        {
            // Get the object that was hit.
            GameObject hitObject = rayHit.collider.gameObject;
            if (hitObject.tag == "NPC")
            {
                if (Input.GetKey(KeyCode.E))
                {
                    Transform parentObject = rayHit.transform.parent;
                    GameObject exclaimationpoint = parentObject.Find("exclaimationpoint").gameObject;
                    exclaimationpoint.SetActive(false);
                    if (questActive == false) 
                    {
                        questCoroutine = StartCoroutine(StartQuest(timeInSeconds));
                    }
                    else
                    {
                        string[] reminder = new string[]
                        {
                            "Bring me 10 trashbags before the time runs out"
                        };
                        UiManager.TypeDialogue(reminder);
                    }
                        
                }
            }
        }
    }

    IEnumerator StartQuest(int seconds)
    {
        //optional: spawn 20 extra trash
        if (firstInteraction == true)
        {
            string[] longDialogue = new string[]
            {
                "Oh hi, i see you're collecting trash...",
                "I could use your help. We had a bit of an oopsie in the factory and now it is littered with radioactive trash. Maybe you can throw them in your radiation-proof garbage truck™",
                "You have to be quick tho, as if you stay inside for too long, the radiation becomes lethal.",
                $"So to prove you are quick enough, you get {timeInSeconds} seconds to deliver {itemAmount} trash bags to the dumpster next to me.",
                "good luck!"
            };
            UiManager.TypeDialogue(longDialogue);
        }
        else
        {
            string[] shortDialogue = new string[]
            {
                "Ah, you're back. You think you can do it this time?",
                $"Remember, {itemAmount} trash bags in {timeInSeconds} seconds. Good luck!"
            };
            UiManager.TypeDialogue(shortDialogue);
        }

        // Wait until the dialogue is finished
        yield return new WaitUntil(() => UiManager.dialogueFinished == true);

        initialScore = PlayerManager.Score;
        questActive = true;
        firstInteraction = false;

        UiManager.DisplayQuestInfo($"collect {itemAmount} trash bags", $"0/{itemAmount}");

        timerScript.StartTimer(seconds, "time left");
    }



    void StopQuest()
    {
        timerScript.StopTimer();
        UiManager.HideQuestInfo();
        questActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (start == true)
        {
            PlayIntro();
            start = false;
        }

        if(questArchieved == false)
        {
            DetectObject();
        }

        if(questActive == true)
        {
            UiManager.UpdateQuestProgress($"{PlayerManager.Score - initialScore}/{itemAmount}");
            if (timerScript.timerComplete == true)
            {
                //quest failed
                string[] failDialogue = new string[]
                {
                    "ahh, you ran out of time...",
                    "Don't worry tho. You can always come back when you think you're ready!"
                };
                UiManager.TypeDialogue(failDialogue);
                //manneke zegt da ge altijd opnieuw moogt proberen, en je mag terugkomen wanneer er dankt klaar voor te zijn
                timerScript.timerComplete = false; //reset timerend not to introduce bugs or something
                StopQuest();
            }
            else if ((PlayerManager.Score - initialScore) == itemAmount)
            {
                //quest complete
                string[] successDialogue = new string[]
                {
                    "Great work!, you completed the assignment!",
                    "The factory entrance is now open, so you can enter whenever you wish. Once you're inside, remember to be quick, because, you know, radiation and stuff...",
                    "The radioactive trash is also quite spread out, so you'll have to really search for them!",
                    "And if you can't find your truck, just follow the emergency exit signs. They lead to the garage where i'll park your truck.",
                    "good luck, and thanks again for the help!"
                };
                UiManager.TypeDialogue(successDialogue);
                questArchieved = true;
                //display dat quest gelukt is
                //beetje meer conversatie en manneke zegt dat poort open gaat, en wenst good luck

                factoryOpen.SetTrigger("quest complete");
                gate_src.PlayOneShot(gate_open);
                StopQuest();
            }
        }
    }
}
