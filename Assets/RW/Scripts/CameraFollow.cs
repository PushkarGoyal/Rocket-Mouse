using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public GameObject targetObject;  // Reference To Target Object
    private float distanceToTarget; // Distance To Maintain Bw Camera And Target Object


    void Start()
    {
        distanceToTarget = transform.position.x - targetObject.transform.position.x;  // Calculating Distance Bw Camera and Target object on Start

    }

    void Update()
    {
        float targetObjectX = targetObject.transform.position.x; // getting Target Object Updated Position
        Vector3 newCameraPosition = transform.position;
        newCameraPosition.x = targetObjectX + distanceToTarget; // calculating Camera Position 
        transform.position = newCameraPosition; // Setting Camera Position

    }
}
