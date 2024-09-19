using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public int interactableID = -4; // To help GameManager keep track of what's interacted with

    [Header("Requirements")]
    // (see inspector) When interacted with, checks GameManager if these IDs are correct. Leave empty if not needed 
    public int[] requiredIDLinks;
    public int[] requiredInteractions;
    public int requiredHeldObjectID = -9;
    public bool destroyHeldObj;

    [Header("Dialogue")]
    public float dialogueTimer = 2f;
    public string interactionDialogue = "";
    public string[] missingObjectDialogues;
    public string[] missingInteractionDialogues;
    public string missingHeldObjDialogue;

    [Header("Object Spawning")]
    public GameObject interactionSpawnsPrefab = null;
    public Transform interactionSpawnPos = null;
    public int giveObjectID = -3;

    public AudioClip interactionSFX = null;
    AudioSource audioPlayer;

    private bool isInteractedWith = false;

    void Start()
    {
        audioPlayer = GetComponent<AudioSource>();

    }

    public void Interact(int heldObjID, GameObject heldObj)
    {
        if (!RequirementsAreMet(heldObjID, heldObj)) return;

        DoInteraction();

    }

    private bool RequirementsAreMet(int heldObjID, GameObject heldObj)
    {
        // Has this already been interacted with?
        if (isInteractedWith) return false;

        // Has the player interacted with everything yet?
        if (!InteractionRequirementIsMet()) return false;

        // Are all objects on the right place?
        if (!IDLinkRequirementIsMet()) return false;



        // Checking requirements for held obj 
        if (requiredHeldObjectID >= 0)
        {
            if (requiredHeldObjectID != heldObjID)
            {
                DialogueSystem.GetMainDialogueSystem().HandleText(missingHeldObjDialogue, dialogueTimer);
                return false;
            }
            if (destroyHeldObj) Destroy(heldObj);
        }



        return true;
    }

    private bool IDLinkRequirementIsMet()
    {
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
                    return false;
                }
            }
        }

        return true;
    }

    private bool InteractionRequirementIsMet()
    {

        if (requiredInteractions.Length > 0) // Checks for any requirements
        {
            for (int i = 0; i < requiredInteractions.Length; i++)
            {
                if (!GameManager.GetMainManager().IsInteractedWith(requiredInteractions[i])) // Checks specific requirements
                {
                    if (missingInteractionDialogues.Length > i) // Checks if there's dialogue
                    {
                        DialogueSystem.GetMainDialogueSystem().HandleText(missingInteractionDialogues[i], dialogueTimer);
                    }
                    return false;
                }
            }
        }


        return true;
    }


    private void DoInteraction()
    {
        PlaySound();

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

    private void PlaySound()
    {
        if (audioPlayer != null && interactionSFX != null)
        {
            //Debug.Log("Playing sound");
            audioPlayer.PlayOneShot(interactionSFX);
        }
        else
        {
            Debug.Log("Interactable: Cannot play sound. audioPlayer = " + (audioPlayer != null) + ", interactionSFX = " + (interactionSFX != null));
        }
    }
}
