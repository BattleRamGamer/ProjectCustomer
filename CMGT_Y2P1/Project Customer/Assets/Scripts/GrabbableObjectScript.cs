using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableObjectScript : MonoBehaviour
{
    //public static event Action OnCorrectIDLink;
    //public static event Action OnWrongIDLink;
    
    public int objectID = -1;
    public bool hasPhysics = true;

    private GameObject PlacedOnPlacable = null;
    public GameObject placedOnPlacable
    {
        get
        {
            return PlacedOnPlacable;
        }
        set
        {
            if (value != null && value.TryGetComponent(out PlacerScript script))
            {
                if (script.placerID == objectID)
                {
                    isPlacedRight = true;
                }
            }
            else
            {
                isPlacedRight = false;
            }
            PlacedOnPlacable = value;
        }
    }

    bool IsPlacedRight = false;
    bool isPlacedRight
    {
        get
        {
            return IsPlacedRight;
        }
        set
        {
            if (value != IsPlacedRight)
            {
                if (value) GameManager.GetMainManager().CorrectObjectIDLink(objectID);
                else GameManager.GetMainManager().WrongObjectIDLink(objectID);
            }
            IsPlacedRight = value;
        }
    }
}
