using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestCity : MonoBehaviour
{

    public GameTimer timerScript;
    public UIManager UiManager;
    public PlayerManager playerManager;
    public StateManager statemanager;

    public CityMaker CityMaker;
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
        initialScore = PlayerManager.Score;
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
                    Transform parentObject = rayHit.transform.parent;
                    GameObject exclaimationpoint = parentObject.Find("exclaimationpoint").gameObject;
                    exclaimationpoint.SetActive(false);
                    if (questActive == false)
                    {
                        questCoroutine = StartCoroutine(StartQuest());
                    }
                    else if ((PlayerManager.Score - initialScore) < itemAmount)
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

    IEnumerator StartQuest()
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

        // UiManager.DisplayQuestInfo($"collect {itemAmount} boxs", "");


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
            playerManager.addScore(-(PlayerManager.Score - initialScore));

        }
        if (questArchieved == false)
        {
            DetectObject();
        }

        if (questActive == true)
        {
            UiManager.UpdateQuestProgress($"boxes collect:{PlayerManager.Score - initialScore}");

        }
    }


}
