using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public int interactableID = -4; // To help GameManager keep track of what's interacted with

    // (see inspector) When interacted with, checks GameManager if these IDs are correct. Leave empty if not needed 
    public int[] requiredIDLinks;
    public int requiredHeldObjectID = -9;
    public bool destroyHeldObj;

    [Header("Dialogue")]
    public float dialogueTimer = 2f;
    public string interactionDialogue = "";
    public string[] missingObjectDialogues;
    public string missingHeldObjDialogue;

    [Header("Object Spawning")]
    public GameObject interactionSpawnsPrefab = null;
    public Transform interactionSpawnPos = null;
    public int giveObjectID = -3;

    private bool isInteractedWith = false;
    
    public void Interact(int heldObjID, GameObject heldObj)
    {
        if (isInteractedWith) return;

        // Checking requirements for correctly placed objects
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

        // Checking requirements for held obj 
        if (requiredHeldObjectID >= 0)
        {
            if (requiredHeldObjectID != heldObjID)
            {
                DialogueSystem.GetMainDialogueSystem().HandleText(missingHeldObjDialogue, dialogueTimer);
                return;
            }
            if (destroyHeldObj) Destroy(heldObj);
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
