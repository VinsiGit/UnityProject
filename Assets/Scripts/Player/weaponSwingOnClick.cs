using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponSwingOnClick : MonoBehaviour
{
    private Animator anim;
    public pauseManager pauseMan; // Reference to the PauseManager script or any other script controlling pause state
    public AudioSource src;
    public AudioClip whoosh;

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        // Check if the game is not paused before responding to mouse clicks
        if (pauseMan != null && !pauseMan.paused && Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("click");
            src.PlayOneShot(whoosh);
        }
    }
}
