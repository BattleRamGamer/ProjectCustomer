using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpScript : MonoBehaviour
{
    [Header("Keybinds")]
    public KeyCode GrabOrPlaceKey;
    public KeyCode PostItSummonKey;

    [Header("Configuration")]
    public GameObject player;
    public Transform holdPos;
    public GameObject postItPrefab;

    //if you copy from below this point, you are legally required to like the video
    public float pickUpRange = 5f; //how far the player can pickup the object from
    //private float rotationSensitivity = 1f; //how fast/slow the object is rotated in relation to mouse movement
    private GameObject heldObj; //object which we pick up
    private Rigidbody heldObjRb; //rigidbody of object we pick up
    //private bool canDrop = true; //this is needed so we don't throw/drop object when rotating the object
    private int LayerNumber; //layer index


    //Reference to script which includes mouse movement of player (looking around)
    //we want to disable the player looking around when rotating the object
    //example below 
    //MouseLookScript mouseLookScript;
    void Start()
    {
        LayerNumber = LayerMask.NameToLayer("holdLayer"); //if your holdLayer is named differently make sure to change this ""
        //mouseLookScript = player.GetComponent<MouseLookScript>();
    }
    void Update()
    {
        // Grab post-it note
        if (Input.GetKeyDown(PostItSummonKey) && heldObj == null)
        {
            PlayerMovement.GetPlayer().FreezeMovement(); // Freezing player movement
            PickUpObject(Instantiate(postItPrefab, new Vector3(0, 0, 0), GetComponent<Transform>().rotation));
        }
        
        // Place/grab object
        if (Input.GetKeyDown(GrabOrPlaceKey)) //change E to whichever key you want to press to pick up
        {
            ObjectInteraction();
        }
        if (heldObj != null) //if player is holding object
        {
            MoveObject(); //keep object position at holdPos

            //RotateObject();   ///////// do we keep object rotation?
        }
    }

    private void ObjectInteraction()
    {
        RaycastHit hit;
        if (heldObj == null) //if currently not holding anything
        {
            //perform raycast to check if player is looking at object within pickuprange
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickUpRange))
            {
                //make sure pickup tag is attached
                if (hit.transform.gameObject.tag == "canPickUp")
                {
                    //pass in object hit into the PickUpObject function
                    PickUpObject(hit.transform.gameObject);
                }
            }
        }
        else
        {
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickUpRange, ~(1 << LayerNumber)))
            {
                //make sure right tag is attached
                if (hit.transform.gameObject.tag == "canBePlacedOn")
                {
                    //pass in placement target object into the PlaceObject function
                    PlaceObject(hit.transform.gameObject);
                }
            }

        }
    }

    void PickUpObject(GameObject pickUpObj)
    {
        if (pickUpObj.GetComponent<Rigidbody>()) //make sure the object has a RigidBody
        {
            heldObj = pickUpObj; //assign heldObj to the object that was hit by the raycast (no longer == null)
            heldObjRb = pickUpObj.GetComponent<Rigidbody>(); //assign Rigidbody
            heldObjRb.isKinematic = true;
            heldObjRb.transform.parent = holdPos.transform; //parent object to holdposition
            heldObj.layer = LayerNumber; //change the object layer to the holdLayer
            //make sure object doesnt collide with player, it can cause weird bugs
            Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), true);

            //spaghettified check for swap or no swap
            //if (heldObj.GetComponent<GrabbableObjectScript>().placedOnPlacable.GetComponent<PlacerScript>())
            //{

            if (heldObj.Equals(heldObj.GetComponent<GrabbableObjectScript>().placedOnPlacable.GetComponent<PlacerScript>().heldObject))
            {
                //resetting placable value for keeping track of placed object
                heldObj.GetComponent<GrabbableObjectScript>().placedOnPlacable.GetComponent<PlacerScript>().heldObject = null;
            }
            //}
            //resetting grabbable parameter for keeping track of thing it's placed on
            heldObj.GetComponent<GrabbableObjectScript>().placedOnPlacable = null;
        }
    }

    void PlaceObject(GameObject placeOnObj)
    {
        GameObject placerIsHolding = placeOnObj.GetComponent<PlacerScript>().heldObject;

        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
        heldObj.layer = 0; //object assigned back to default layer
        if (heldObj.GetComponent<GrabbableObjectScript>().hasPhysics)
        {
            heldObjRb.isKinematic = false;
        }
        else
        {   // Post-it note
            heldObj.transform.rotation = placeOnObj.transform.rotation;
            heldObj.transform.Rotate(90, 0, 0);
        }
        heldObj.transform.parent = null; //unparent object
        heldObj.transform.position = placeOnObj.transform.position; //placing obj in the right place

        //linking object with thing it's placed on and vice versa
        placeOnObj.GetComponent<PlacerScript>().heldObject = heldObj;
        heldObj.GetComponent<GrabbableObjectScript>().placedOnPlacable = placeOnObj;

        if (placerIsHolding != null)
        {
            PickUpObject(placerIsHolding); //swapping object
        }
        else
        {
            heldObj = null; //undefine game object
        }
    }

    void MoveObject()
    {
        //keep object position the same as the holdPosition position
        heldObj.transform.position = holdPos.transform.position;
    }

    /*void RotateObject()
    {
        if (Input.GetKey(KeyCode.R))//hold R key to rotate, change this to whatever key you want
        {
            canDrop = false; //make sure throwing can't occur during rotating

            //disable player being able to look around
            //mouseLookScript.verticalSensitivity = 0f;
            //mouseLookScript.lateralSensitivity = 0f;

            float XaxisRotation = Input.GetAxis("Mouse X") * rotationSensitivity;
            float YaxisRotation = Input.GetAxis("Mouse Y") * rotationSensitivity;
            //rotate the object depending on mouse X-Y Axis
            heldObj.transform.Rotate(Vector3.down, XaxisRotation);
            heldObj.transform.Rotate(Vector3.right, YaxisRotation);
        }
        else
        {
            //re-enable player being able to look around
            //mouseLookScript.verticalSensitivity = originalvalue;
            //mouseLookScript.lateralSensitivity = originalvalue;
            canDrop = true;
        }
    }*/


}
