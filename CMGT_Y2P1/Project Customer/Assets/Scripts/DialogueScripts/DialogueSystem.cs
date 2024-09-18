using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] private TMP_Text dialogueObject;

    public AudioClip dialogueSFX = null;
    AudioSource audioPlayer;

    public static DialogueSystem GetMainDialogueSystem()
    {
        return mainDialogueSystem;
    }
    static DialogueSystem mainDialogueSystem = null;

    private void Awake()
    {
        if (mainDialogueSystem == null)
        {
            DontDestroyOnLoad(gameObject);
            mainDialogueSystem = this;

            // subscribe to events here
        }
        else
        {
            Debug.Log("Second dialogue system destroys itself");
            Destroy(gameObject);
        }
    }


    private void Start()
    {
        // Iterate through each child transform of this GameObject that the DialogueSystem script is attached to
        foreach (Transform t in transform)
        {
            // Check if the child has a Dialogue component
            if (t.GetComponent<Dialogue>() != null)
            {
                // Set up the Dialogue component with a reference to this DialogueSystem and the dialogueObject
                t.GetComponent<Dialogue>().SetUp(this);
            }
        }
        audioPlayer = GetComponent<AudioSource>();
    }

    // Method to handle displaying the dialogue text
    public void HandleText(string textValue, float timer)
    {
        // Cancel any previously scheduled StopText invocation to avoid conflicts
        CancelInvoke(nameof(StopText));

        // Set the dialogueObject text to the new dialogue value
        dialogueObject.text = textValue;

        // Play dialogue sounds
        PlaySound(dialogueSFX);

        // Schedule the StopText method to be called after the specified timer duration
        Invoke(nameof(StopText), timer);
    }

    // Method to clear the dialogue text
    private void StopText()
    {
        // Clear the text from the dialogueObject
        dialogueObject.text = "";
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
            Debug.Log("DialogueSystem: Cannot play sound. audioPlayer = " + (audioPlayer != null) + ", sound = " + (sound != null));
        }
    }

}
