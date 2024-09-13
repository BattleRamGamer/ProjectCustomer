using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // This array keeps track of which object IDs are correctly linked
    private bool[] objectIDLinks = new bool[32];

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
    

    // These are called by GrabbableObjectScript
    public void CorrectObjectIDLink(int id)
    {
        objectIDLinks[id] = true;
        Debug.Log("CorrectObjectIDLink");

    }

    public void WrongObjectIDLink(int id)
    {
        objectIDLinks[id] = false;
        Debug.Log("WrongObjectIDLink");
    }

}
