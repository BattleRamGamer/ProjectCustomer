using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public string interactionDialogue = "";
    public float dialogueTimer = 2f;

    public GameObject interactionSpawnsPrefab = null;
    public Transform interactionSpawnPos = null;
    public int giveObjectID = -3;

    private bool isInteractedWith = false;
    
    public void Interact()
    {
        if (isInteractedWith) return;

        isInteractedWith = true;

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
