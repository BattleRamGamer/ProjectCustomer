using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class InteractionScript : MonoBehaviour
{
    public KeyCode interactionKey;
    public float interactRange = 5f; //how far the player can pickup the object from

    private int holdLayerNr;
    // canBeInteractedWith
    // Start is called before the first frame update
    void Start()
    {
        holdLayerNr = LayerMask.NameToLayer("holdLayer");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(interactionKey))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, interactRange, ~(1 << holdLayerNr)))
            {
                //make sure right tag is attached
                if (hit.transform.gameObject.tag == "canBeInteractedWith")
                {
                    //pass in placement target object into the PlaceObject function
                    hit.transform.gameObject.GetComponent<Interactable>().Interact();
                }
            }
        }
    }
}
