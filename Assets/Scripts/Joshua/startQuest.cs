using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startQuest : MonoBehaviour
{

    public GameTimer timerScript;
    public UIManager UiManager;
    public PlayerManager playerManager;
    public StateManager statemanager;

    public CityMaker CityMaker;
    public int timeInSeconds = 3;
    public int itemAmount = 5;

    private int initialScore;
    private bool firstInteraction = true;
    private bool questActive = false;

    private bool questArchieved = false;
    private Coroutine questCoroutine; // Coroutine reference for interaction text
                                      // Start is called before the first frame update

    private Vector3 playerPosition;
    private Vector3 containerPosition;
    private List<Vector3> roadPositions;

    void Start()
    {
        roadPositions = CityMaker.RoadPositions;

        playerPosition = CityMaker.playerPrefab.transform.position;
        containerPosition = CityMaker.containerPrefab.transform.position; // Assuming 'container' is your container GameObject

    }

    void PlayIntro()
    {
        string[] intro = new string[]
            {
                "Hi, you are a trashman. This city is littered with trash, be careful there are dangerous street dogs.",
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

                    if (questActive == false)
                    {
                        hitObject.tag = "";
                        questCoroutine = StartCoroutine(StartQuest(timeInSeconds));
                        hitObject.tag = "NPC";
                    }
                    else
                    {
                        string[] reminder = new string[]
                        {
                            $"Hey, you haven't cleaned the city enough"
                        };
                        UiManager.TypeDialogue(reminder);
                    }
                    if ((PlayerManager.Score - initialScore) >= itemAmount)
                    {
                        statemanager.loadNextLevel();
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
                "Yo, I see you're on time.",
                "You have to collect as many discarded cardboard boxes as you think necessary.",
                "You can get as many boxes as you want but be aware of the street dogs, you can swing your bat to scare them.",
                "If you feel it's getting too dangerous, talk to me if you want to leave.",
                "Good luck!",
            };
            UiManager.TypeDialogue(longDialogue);
        }
        else
        {
            string[] shortDialogue = new string[]
            {
                "Hey, you haven't cleaned the city enough."
            };
            UiManager.TypeDialogue(shortDialogue);
        }

        // Wait until the dialogue is finished
        yield return new WaitUntil(() => UiManager.dialogueFinished);
        if (firstInteraction)
        {


            CityMaker.GenerateEnemies();
            CityMaker.GeneratePickups();
            CityMaker.InvokeRepeating("AddEnemy", 10.0f, 10.0f);
            initialScore = PlayerManager.Score;
        }
        questActive = true;
        firstInteraction = false;

        UiManager.DisplayQuestInfo($"collect {itemAmount} boxs", "");


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
        int health = playerManager.Health;
        if (health == 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        }
        if (questArchieved == false)
        {
            DetectObject();
        }

        if (questActive == true)
        {
            UiManager.UpdateQuestProgress($"boxes collect:{PlayerManager.Score - initialScore}");
            // if (timerScript.timerComplete == true)
            // {
            //     //quest failed
            //     string[] failDialogue = new string[]
            //     {
            //         "ahh, you ran out of time",
            //         "Don't worry tho. You can always come back when you think you're ready!"
            //     };
            //     UiManager.TypeDialogue(failDialogue);
            //     //manneke zegt da ge altijd opnieuw moogt proberen, en je mag terugkomen wanneer er dankt klaar voor te zijn
            //     timerScript.timerComplete = false; //reset timerend not to introduce bugs or something
            //     StopQuest();
            // }

            // {
            //     //quest complete
            //     string[] successDialogue = new string[]
            //     {
            //         "Great work!, you completed the assignent!",
            //         "The factory entrance is now open, so you can enter whenever you wish. Once you're inside, remember to be quick, because, you know, radiation and stuff...",
            //         "good luck, and thanks for helping"
            //     };
            //     UiManager.TypeDialogue(successDialogue);
            //     questArchieved = true;
            //     //display dat quest gelukt is
            //     //beetje meer conversatie en manneke zegt dat poort open gaat, en wenst good luck

            //     StopQuest();
            // }
        }
    }


}
