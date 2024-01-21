using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class startQuest : MonoBehaviour
{

    public GameTimer timerScript;
    public UIManager UiManager;

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
    List<Vector3> roadPositions = CityMaker.RoadPositions;

    void Start()
    {
        playerPosition = CityMaker.playerPrefab.transform.position;
        containerPosition = CityMaker.containerPrefab.transform.position; // Assuming 'container' is your container GameObject

        PlayIntro();
    }

    void PlayIntro()
    {
        string[] intro = new string[]
{
                "Hi,you are a trashman. This field is littered with trash",
                "It is your responsibility to collect this trash",
                "Now go, before I fire your ass"
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
                "Oh hi, i see you're collecting trash",
                "I could use your help. We had a bit of an oopsie in the factory and now it is littered with radioactive trash. Maybe you can throw them in your radiation-proof garbage truck with built-in homeless shelter�",
                "You have to be quick tho, as if you stay inside for too long, the radiation becomes lethal",
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
        yield return new WaitUntil(() => UIManager.dialogueFinished); //UiManager.dialogueFinished

        GenerateEnemies();
        GeneratePickups();

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
        if (questArchieved == false)
        {
            DetectObject();
        }

        if (questActive == true)
        {
            UiManager.UpdateQuestProgress($"{PlayerManager.Score - initialScore}/{itemAmount}");
            if (timerScript.timerComplete == true)
            {
                //quest failed
                string[] failDialogue = new string[]
                {
                    "ahh, you ran out of time",
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
                    "Great work!, you completed the assignent!",
                    "The factory entrance is now open, so you can enter whenever you wish. Once you're inside, remember to be quick, because, you know, radiation and stuff...",
                    "good luck, and thanks for helping"
                };
                UiManager.TypeDialogue(successDialogue);
                questArchieved = true;
                //display dat quest gelukt is
                //beetje meer conversatie en manneke zegt dat poort open gaat, en wenst good luck

                StopQuest();
            }
        }
    }

    void GenerateEnemies()
    {
        for (int i = 0; i < CityMaker.numberOfEnemies; i++)
        {
            if (roadPositions.Count > 0)
            {
                Vector3 enemyPosition;
                do
                {
                    enemyPosition = roadPositions[Random.Range(0, roadPositions.Count)];
                }
                while (Vector3.Distance(enemyPosition, playerPosition) < 10 && Vector3.Distance(enemyPosition, containerPosition) < 10);

                Instantiate(CityMaker.enemyPrefab, enemyPosition, Quaternion.identity);
            }
        }
    }

    void GeneratePickups()
    {

        for (int i = 0; i < CityMaker.numberOfPickups; i++)
        {
            if (roadPositions.Count > 0)
            {
                Vector3 pickupPosition = roadPositions[Random.Range(0, roadPositions.Count)];
                Quaternion pickupRotation;
                do
                {
                    float offsetX = Random.Range(-4f, 4f);
                    float offsetZ = Random.Range(-4f, 4f);
                    pickupPosition += new Vector3(offsetX, 0.0f, offsetZ);

                    float randomYRotation = Random.Range(0f, 360f);
                    pickupRotation = Quaternion.Euler(0, randomYRotation, 0);

                }
                while (Vector3.Distance(pickupPosition, playerPosition) < 30 && Vector3.Distance(pickupPosition, containerPosition) < 30);


                Instantiate(CityMaker.pickupPrefab, pickupPosition, pickupRotation);
            }
        }
    }
}
