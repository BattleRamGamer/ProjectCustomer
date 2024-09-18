using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableObjectScript : MonoBehaviour
{
    //public static event Action OnCorrectIDLink;
    //public static event Action OnWrongIDLink;
    
    public int objectID = -1;
    public bool hasPhysics = true;

    public AudioClip grabSFX = null;
    public AudioClip placeSFX = null;
    AudioSource audioPlayer;

    void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
    }

    private GameObject PlacedOnPlacable = null;
    public GameObject placedOnPlacable
    {
        get
        {
            return PlacedOnPlacable;
        }
        set
        {
            // Object is given new location
            if (value != null && value.TryGetComponent(out PlacerScript script))
            {
                // Object is placed on placable
                PlaySound(placeSFX);
                if (script.placerID == objectID)
                {
                    // Object is placed on placable with matching ID
                    isPlacedRight = true;
                }
            }
            else
            {
                // Object is grabbed
                PlaySound(grabSFX);
                isPlacedRight = false;
            }
            PlacedOnPlacable = value;
        }
    }

    bool IsPlacedRight = false;
    bool isPlacedRight
    {
        get
        {
            return IsPlacedRight;
        }
        set
        {
            if (value != IsPlacedRight)
            {
                if (value) GameManager.GetMainManager().CorrectObjectIDLink(objectID);
                else GameManager.GetMainManager().WrongObjectIDLink(objectID);
            }
            IsPlacedRight = value;
        }
    }

    private void PlaySound(AudioClip sound)
    {
        if (audioPlayer != null && sound != null)
        {
            //Debug.Log("Playing sound");
            audioPlayer.PlayOneShot(sound);
        }
        else
        {
            Debug.Log("GrabbableObjectScript: Cannot play sound. audioPlayer = " + (audioPlayer != null) + ", sound = " + (sound != null));
        }
    }
}
