using UnityEngine;
using UnityEngine.UI; // For UI components like RawImage

public class InteractionScript : MonoBehaviour
{
    public KeyCode interactionKey;
    public float interactRange = 5f; // How far the player can interact from
    public RawImage interactionIcon; // Reference to the UI RawImage (e.g., PNG texture)

    private int holdLayerNr;
    private PickUpScript pickUpScript; // Cache PickUpScript
    private bool isHovering = false; // To track hover state

    void Start()
    {
        holdLayerNr = LayerMask.NameToLayer("holdLayer");
        pickUpScript = GetComponent<PickUpScript>(); // Cache this once
        if (interactionIcon != null)
        {
            interactionIcon.enabled = false; // Hide interaction icon initially
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(interactionKey))
        {
            Debug.Log("Pressed interaction key");
            TryInteract();
        }
        HandleHover(); // Check hover state every frame
    }

    void HandleHover()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * interactRange, Color.red);

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, interactRange, ~(1 << holdLayerNr)))
        {
            if (hit.transform.gameObject.tag == "canBeInteractedWith")
            {
                // Hover logic continues
                if (!isHovering) // Only activate if we weren't hovering before
                {
                    isHovering = true;
                    ShowInteractionIcon(hit);
                }
            }
            else
            {
                if (isHovering)
                {
                    isHovering = false;
                    HideInteractionIcon();
                }
            }
        }
        else
        {
            if (isHovering)
            {
                isHovering = false;
                HideInteractionIcon();
            }
        }
    }

    void ShowInteractionIcon(RaycastHit hit)
    {
        if (interactionIcon != null)
        {
            interactionIcon.enabled = true; // Show the interaction icon
            // Optionally, move the icon to follow the object or stay at a fixed position on the screen
        }
    }

    void HideInteractionIcon()
    {
        if (interactionIcon != null)
        {
            interactionIcon.enabled = false; // Hide the interaction icon
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
