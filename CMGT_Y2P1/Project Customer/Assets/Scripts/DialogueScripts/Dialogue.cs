using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    private DialogueSystem dialogueSystem; // Reference to the dialogue system

    public float timer = 2f; // Duration to display the dialogue
    public string dialogue = "Dialogue"; // Dialogue text to display

    // Method to set up the dialogue system and text object
    public void SetUp(DialogueSystem _dialogueSystem)
    {
        dialogueSystem = _dialogueSystem;
    }

    // Method called when another collider enters the trigger collider attached to this object
    void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to the player
        if (other.gameObject.CompareTag("Player"))
        {
            // Handle the dialogue text and display duration
            dialogueSystem.HandleText(dialogue, timer);

            // Destroy this game object after displaying the dialogue
            Destroy(gameObject);
        }
    }
}

