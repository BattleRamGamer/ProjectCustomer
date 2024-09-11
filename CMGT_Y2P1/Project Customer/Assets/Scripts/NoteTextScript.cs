using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NoteTextScript : MonoBehaviour
{
    string text = "";
    bool isDone = false;
    void Start()
    {
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isDone = true;
        }
        if (isDone) return;
        string input = Input.inputString;
        //Debug.Log("input: "+input);
        text += input;
        //Debug.Log("text: " + text);
        GetComponent<TextMeshProUGUI>().text = text;
        if (input.Equals("")) return;
        //Debug.Log("didn't return");
    }
}
