using System.Collections;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    private DialogueSystem dialogueSystem; // Reference to the dialogue system

    public float timer = 2f; // Duration to display the dialogue
    public string dialogue = "Dialogue"; // Dialogue text to display

    [SerializeField] private bool isRepetitive = false; // Is the dialogue repetitive?
    [SerializeField] private float repeatInterval = 30f; // Time after which dialogue reappears

    [SerializeField] private int interactionToEnable = -6; // Interaction that enables this trigger
    [SerializeField] private int interactionToDisable; // Interaction that disables repetition

    // Public getter for interactionToDisable
    public int GetInteractionToDisable()
    {
        return interactionToDisable;
    }

    private bool interactionCompleted = false; // Track whether the required interaction is done

    // Method to set up the dialogue system and text object
    public void SetUp(DialogueSystem _dialogueSystem)
    {
        dialogueSystem = _dialogueSystem;
    }

    // Method called when another collider enters the trigger collider attached to this object
    void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to the player
        if (other.gameObject.CompareTag("Player") && 
            (interactionToEnable < 0 || GameManager.GetMainManager().IsInteractedWith(interactionToEnable)))
        {
            // Handle the dialogue text and display duration
            dialogueSystem.HandleText(dialogue, timer);

            // If it's repetitive and the interaction is not completed, set it to repeat
            if (isRepetitive && !interactionCompleted)
            {
                StartCoroutine(RepeatDialogue());
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    // Coroutine to repeat the dialogue after a delay
    private IEnumerator RepeatDialogue()
    {
        yield return new WaitForSeconds(repeatInterval);

        if (!interactionCompleted)
        {
            dialogueSystem.HandleText(dialogue, timer);

            // Restart the coroutine if the interaction is still not done
            StartCoroutine(RepeatDialogue());
        }
    }

    // This method can be called when the specific interaction is completed
    public void CompleteInteraction()
    {
        interactionCompleted = true;
        Destroy(gameObject); // Optionally destroy the game object if the dialogue should stop
    }
}
