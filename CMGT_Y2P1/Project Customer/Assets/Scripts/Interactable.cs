using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public int interactableID = -4;
    // (see inspector) When interacted with, checks GameManager if these IDs are correct. Leave empty if not needed 
    public int[] requiredIDLinks;

    [Header("Dialogue")]
    public float dialogueTimer = 2f;
    public string interactionDialogue = "";
    public string[] missingObjectDialogues;

    [Header("Object Spawning")]
    public GameObject interactionSpawnsPrefab = null;
    public Transform interactionSpawnPos = null;
    public int giveObjectID = -3;

    private bool isInteractedWith = false;
    
    public void Interact()
    {
        if (isInteractedWith) return;

        if (requiredIDLinks.Length > 0) // Checks for any requirements
        {
            for (int i = 0; i < requiredIDLinks.Length; i++)
            {
                if (!GameManager.GetMainManager().CheckIDLink(requiredIDLinks[i])) // Checks specific requirements
                {
                    if (missingObjectDialogues.Length > i) // Checks if there's dialogue
                    {
                        DialogueSystem.GetMainDialogueSystem().HandleText(missingObjectDialogues[i], dialogueTimer);
                    }
                    return;
                }
            }
        }

        // Keeping track of interaction
        isInteractedWith = true;
        if (interactableID >= 0)
        {
            GameManager.GetMainManager().InteractedWithInteractable(interactableID);
        }

        // If you have something to say, speak
        if (interactionDialogue != "")
        {
            DialogueSystem.GetMainDialogueSystem().HandleText(interactionDialogue, dialogueTimer);
        }

        // If you have something to present, show
        if (interactionSpawnsPrefab != null)
        {
            GameObject spawnedObj = Instantiate(interactionSpawnsPrefab, interactionSpawnPos.position, interactionSpawnPos.rotation);
            if (giveObjectID >= 0 && spawnedObj.GetComponent<GrabbableObjectScript>())
            {
                spawnedObj.GetComponent<GrabbableObjectScript>().objectID = giveObjectID;
            }
        }


    }
}
