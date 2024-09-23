using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public string interactableID = ""; // String identifier for tracking

    [Header("Requirements")]
    public string[] requiredIDLinks; // String array for required object links
    public string[] requiredInteractions; // String array for required interactions
    public string requiredHeldObjectID = ""; // String for required held object
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
    public string giveObjectID = ""; // String for object ID
    public float spawnTime = 2;

    public AudioClip interactionSFX = null;
    AudioSource audioPlayer;

    public enum fadeToBlackState { NoFade, FadeInOut, FadeIn }
    public fadeToBlackState fadeToBlack;

    private bool isInteractedWith = false;

    void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
    }

    public void Interact(string heldObjID, GameObject heldObj)
    {
        if (!RequirementsAreMet(heldObjID, heldObj)) return;
        DoInteraction();
    }

    private bool RequirementsAreMet(string heldObjID, GameObject heldObj)
    {
        if (isInteractedWith) return false;
        if (!InteractionRequirementIsMet()) return false;
        if (!IDLinkRequirementIsMet()) return false;

        // Checking held object requirements
        if (!string.IsNullOrEmpty(requiredHeldObjectID))
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
        if (requiredIDLinks.Length > 0)
        {
            for (int i = 0; i < requiredIDLinks.Length; i++)
            {
                if (!GameManager.GetMainManager().CheckIDLink(requiredIDLinks[i])) // Using string check instead of int
                {
                    if (missingObjectDialogues.Length > i)
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
        if (requiredInteractions.Length > 0)
        {
            for (int i = 0; i < requiredInteractions.Length; i++)
            {
                if (!GameManager.GetMainManager().IsInteractedWith(requiredInteractions[i])) // Using string check
                {
                    if (missingInteractionDialogues.Length > i)
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

        switch (fadeToBlack)
        {
            case fadeToBlackState.FadeInOut:
                FadeManager.instance.TriggerFadeOutIn();
                break;
            case fadeToBlackState.FadeIn:
                FadeManager.instance.TriggerFadeOut();
                break;
        }

        isInteractedWith = true;
        if (!string.IsNullOrEmpty(interactableID))
        {
            GameManager.GetMainManager().InteractedWithInteractable(interactableID); // Track by string
        }

        if (!string.IsNullOrEmpty(interactionDialogue))
        {
            DialogueSystem.GetMainDialogueSystem().HandleText(interactionDialogue, dialogueTimer);
        }

        Invoke(nameof(SpawnPrefab), spawnTime);
    }

    void SpawnPrefab()
    {
        if (interactionSpawnsPrefab != null)
        {
            GameObject spawnedObj = Instantiate(interactionSpawnsPrefab, interactionSpawnPos.position, interactionSpawnPos.rotation);
            if (!string.IsNullOrEmpty(giveObjectID) && spawnedObj.GetComponent<GrabbableObjectScript>())
            {
                spawnedObj.GetComponent<GrabbableObjectScript>().objectID = giveObjectID;
            }
        }
    }

    private void PlaySound()
    {
        if (audioPlayer != null && interactionSFX != null)
        {
            audioPlayer.PlayOneShot(interactionSFX);
        }
        else
        {
            Debug.Log("Interactable: Cannot play sound. audioPlayer = " + (audioPlayer != null) + ", interactionSFX = " + (interactionSFX != null));
        }
    }
}
