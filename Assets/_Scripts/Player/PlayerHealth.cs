﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour 
{
    public SceneFadeInOut sceneFadeInOut;
    public LastPlayerSighting lastPlayerSighting;

    //public gameObject gameController;
    //public HashIDs hash; 

    public float health = 100f;
    public float resetAfterDeathTime = 5f;
    public AudioClip deathClip;

    private Animator anim;
    private PlayerMovement playerMovement;

    //private HashIDs hash;
    //private ScreenFadeInOut sceneFadeInOut;
    //private LastPlayerSighting lastPlayerSighting;

    private float timer;
    private bool playerDead;

    private AudioSource audioSource;

    public Text healthText;
    public RectTransform healthImageSize;
    
    void Awake ()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();

        //hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashIDs>();
        //sceneFadeInOut = GameObject.FindGameObjectWithTag(Tags.fader).GetComponent<ScreenFadeInOut>();
        //lastPlayerSighting = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashIDs>();
        
        //// alternatlly grabe gameController once
        //hash = gameController.GetComponent<HashIDs>();
        //lastPlayerSighting = gameController.GetComponent<LastPlayerSighting>();

        audioSource = GetComponent<AudioSource>();
        UpdateHealthDisplay();
    }

    void Update ()
    {
        if (0 >= health)
        {
            if (!playerDead)
            {
                playerDying();
                health = 0;
                UpdateHealthDisplay();
            }
            else
            {
                PlayerDead();
                LevelReset();
            }
        }
    }

    void playerDying ()
    {
        playerDead = true;
        anim.SetBool(HashIDs.deadBool, true);
        AudioSource.PlayClipAtPoint(deathClip, transform.position);
    }

    void PlayerDead ()
    {
        if (HashIDs.dyingState == anim.GetCurrentAnimatorStateInfo(0).fullPathHash)
        {
            anim.SetBool(HashIDs.deadBool, false);
        }

        anim.SetFloat(HashIDs.speedFloat, 0f);
        playerMovement.enabled = false;
        LastPlayerSighting.position = LastPlayerSighting.resetPosition;
        audioSource.Stop();
    }

    void LevelReset ()
    {
        timer += Time.deltaTime;

        if (timer >= resetAfterDeathTime)
        {
            sceneFadeInOut.EndScene();
        }
    }

    public void TakeDamage (float amount)
    {
        health -= amount;
        UpdateHealthDisplay();
        
    }

    public void UpdateHealthDisplay ()
    {
        healthText.text = Mathf.RoundToInt(health) + System.String.Empty;
        healthImageSize.sizeDelta = new Vector2(health, 10f);
        
    }
}