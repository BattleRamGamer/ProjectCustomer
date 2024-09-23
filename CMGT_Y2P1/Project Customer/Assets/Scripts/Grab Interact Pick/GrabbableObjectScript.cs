using System;
using UnityEngine;

public class GrabbableObjectScript : MonoBehaviour
{
    // Change objectID from int to string
    public string objectID = ""; // String ID now
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
                // Use string comparison instead of int comparison
                if (script.placerID == objectID)
                {
                    isPlacedRight = true;
                }
            }
            else
            {
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
                if (value)
                {
                    GameManager.GetMainManager().CorrectObjectIDLink(objectID); // Pass string objectID
                }
                else
                {
                    GameManager.GetMainManager().WrongObjectIDLink(objectID); // Pass string objectID
                }
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
