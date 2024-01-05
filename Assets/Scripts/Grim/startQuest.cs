using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class startQuest : MonoBehaviour
{

    public gameTimer timerScript;
    public PlayerManager playerManager;
    public UIManager UiManager;
    public Animator factoryOpen;
    
    public int timeInSeconds = 300;
    public int itemAmount = 10;

    private int initialScore;
    private bool firstInteraction = true;
    private bool questActive = false;
    private bool questArchieved = false;
    // Start is called before the first frame update
    void Start()
    {
        
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
                        StartQuest(timeInSeconds);
                    }
                    else
                    {
                        //reminder opdracht
                    }
                        
                }
            }
        }
    }

    void StartQuest(int seconds)
    {
        //optional: spawn 20 extra trash
        if(firstInteraction == true)
        {
            //veel tekst
        }
        else
        {
            //minder tekst
        }

        initialScore = playerManager.score;
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
        if(questArchieved == false)
        {
            DetectObject();
        }

        if(questActive == true)
        {
            UiManager.UpdateQuestProgress($"{playerManager.score - initialScore}/{itemAmount}");
            if (timerScript.timerComplete == true)
            {
                //quest failed
                Debug.Log("quest failed");
                //manneke zegt da ge altijd opnieuw moogt proberen, en je mag terugkomen wanneer er dankt klaar voor te zijn
                timerScript.timerComplete = false; //reset timerend not to introduce bugs or something
                StopQuest();
            }
            else if ((playerManager.score - initialScore) == itemAmount)
            {
                //quest complete
                Debug.Log("quest success");
                //display dat quest gelukt is
                //beetje meer conversatie en manneke zegt dat poort open gaat, en wenst good luck

                factoryOpen.SetTrigger("quest complete");
                StopQuest();
            }
        }
    }
}
