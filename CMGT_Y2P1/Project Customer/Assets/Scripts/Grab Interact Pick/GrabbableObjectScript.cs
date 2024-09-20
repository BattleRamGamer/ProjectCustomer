using System;
using UnityEngine;

public class GrabbableObjectScript : MonoBehaviour
{
    public int objectID = -1;
    public bool hasPhysics = true;

    public AudioClip grabSFX = null;
    public AudioClip placeSFX = null;
    AudioSource audioPlayer;

    private GameObject PlacedOnPlacable = null;

    public GameObject placedOnPlacable
    {
        get
        {
            return PlacedOnPlacable;
        }
        set
        {
            if (value != null && value.TryGetComponent(out PlacerScript script))
            {
                PlaySound(placeSFX);
                if (script.placerID == objectID)
                {
                    isPlacedRight = true;
                }
            }
            else
            {
                PlaySound(grabSFX);
                //FadeManager.instance.TriggerFade();  // Call FadeManager to handle the fade effect
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

    void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
    }

    private void PlaySound(AudioClip sound)
    {
        if (audioPlayer != null && sound != null)
        {
            audioPlayer.PlayOneShot(sound);
        }
        else
        {
            Debug.LogWarning("Cannot play sound. Either audioPlayer or sound is missing.");
        }
    }
}
