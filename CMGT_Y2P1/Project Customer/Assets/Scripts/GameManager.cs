using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // This array keeps track of which object IDs are correctly linked
    private bool[] objectIDLinks = new bool[32];
    private bool[] interactedWithInteractables = new bool[32];

    public string[] correctlyPlacedObjectDialogues;


    public static GameManager GetMainManager()
    {
        return mainManager;
    }
    static GameManager mainManager = null;

    private void Awake()
    {
        if (mainManager == null)
        {
            DontDestroyOnLoad(gameObject);
            mainManager = this;

            // subscribe to events here
        }
        else
        {
            Debug.Log("Second game manager destroys itself");
            Destroy(gameObject);
        }
    }


    public bool CheckIDLink(int id)
    {
        return objectIDLinks[id];
    }
    public bool IsInteractedWith(int id)
    {
        return interactedWithInteractables[id];
    }

    // These are called by GrabbableObjectScript
    public void CorrectObjectIDLink(int id)
    {
        objectIDLinks[id] = true;
        Debug.Log("CorrectObjectIDLink");
        if (correctlyPlacedObjectDialogues.Length > id) DialogueSystem.GetMainDialogueSystem().HandleText(correctlyPlacedObjectDialogues[id], 5);

    }

    public void WrongObjectIDLink(int id)
    {
        objectIDLinks[id] = false;
        Debug.Log("WrongObjectIDLink");
    }

    public void InteractedWithInteractable(int id)
    {
        interactedWithInteractables[id] = true;
    }

}
