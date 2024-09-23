using System.Collections;
using UnityEngine;

public class InteractionScript : MonoBehaviour
{
    public KeyCode interactionKey;
    public float interactRange = 5f; // How far the player can interact from

    private int holdLayerNr;
    private PickUpScript pickUpScript; // Cache PickUpScript

    void Start()
    {
        holdLayerNr = LayerMask.NameToLayer("holdLayer");
        pickUpScript = GetComponent<PickUpScript>(); // Cache this once
    }

    void Update()
    {
        if (Input.GetKeyDown(interactionKey))
        {
            Debug.Log("Pressed interaction key");
            TryInteract();
        }
    }

    void TryInteract()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, interactRange, ~(1 << holdLayerNr)))
        {
            Debug.Log("Raycast hit something");

            TryInteract(hit); // Separate method for tag and interaction check
        }
        else
        {
            Debug.Log("Raycast didn't hit anything");
        }
    }

    void TryInteract(RaycastHit hit)
    {
        if (hit.transform.gameObject.tag == "canBeInteractedWith")
        {
            Debug.Log("Hit object has the right tag");
            InteractWithObject(hit);
        }
        else
        {
            Debug.Log("Hit object does not have the right tag");
        }
    }

    void InteractWithObject(RaycastHit hit)
    {
        string heldObjID = pickUpScript != null ? pickUpScript.GetHeldObjectID() : "";
        GameObject heldObj = pickUpScript != null ? pickUpScript.GetHeldObj() : null;

        Interactable interactable = hit.transform.gameObject.GetComponentInParent<Interactable>();
        if (interactable != null)
        {
            interactable.Interact(heldObjID, heldObj);
            Debug.Log($"Interacted with object: {hit.transform.gameObject.name} at position {hit.point}");
        }
        else
        {
            Debug.LogWarning("Hit object lacks Interactable component");
        }
    }
}
