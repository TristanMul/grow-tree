using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragControll : MonoBehaviour, IDragHandler
{
    //public Swing swing;
    [Header("The rigidbody of pole base")]
    public Rigidbody rb;
    [Header("Finger drag distance -> pole movement")]
    public float multiplier;
    [Header("Speed the pole moves to reach target position")]
    public float speed;

    [Header("Swing angle limit (negative value)")]
    public float minAngle;
    [Header("Swing angle limit (positive value)")]
    public float maxAngle;
    [Header("Speed limmit for pole movement")]
    public float maxSpeed;

    float distMult;
    bool lockControl;


    //Angle calculation variables
    float targetAngle;
    float tempAngle;
    [HideInInspector]
    public float angle;

    //On drag is movement of finger/pointer on screen
    public void OnDrag(PointerEventData eventData)
    {
        if (!lockControl)
        {
            targetAngle += eventData.delta.x * multiplier;
        }
    }

    // Update is called once per frame
    void FixedUpdate() 
    {
        //Get the current angle
        tempAngle = rb.gameObject.transform.localEulerAngles.y;
        //The angle we got is a value from 0 to 360, we want a value of -180 to 180
        if (tempAngle < 180)
        {
            //The first 180 degrees are correct
            angle = tempAngle;
        }
        else
        {
            //Above 180 degrees we substract 360 to get the correct angle
            angle = (tempAngle - 360);
        }

        //Limit the max angles of the pole
        if (targetAngle > maxAngle)
        {
            targetAngle = maxAngle;
        }
        if (targetAngle < minAngle)
        {
            targetAngle = minAngle;
        }
        
        //Rotate the pole towards the target angle 
        if (angle < targetAngle)
        {
            distMult = Mathf.Abs(targetAngle - angle);
            if (distMult > maxSpeed)
            {
                distMult = maxSpeed;
            }
            rb.angularVelocity = Vector3.up * speed * distMult;
        }
        if (angle > targetAngle)
        {
            distMult = Mathf.Abs(targetAngle - angle);
            if (distMult > maxSpeed)
            {
                distMult = maxSpeed;
            }
            rb.angularVelocity = -Vector3.up * speed * distMult;
        }
    }

    public void LockControls()
    {
        lockControl = true;
        targetAngle = 0;
        StartCoroutine("SetTarget0");
    }

    IEnumerator SetTarget0()
    {
        for (int i = 0; i < 10; i++)
        {
            targetAngle = 0;
            yield return null;
        }
    }
}
