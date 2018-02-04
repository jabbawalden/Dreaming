using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAMFollow : MonoBehaviour {

    public Transform target;
    //how quickly our cam follows the palyer
    public float smoothTime = 0.2f;
    public float maxSpeed = 1;
    public Vector3 offset;
    Vector3 velocity = Vector3.zero;


    private void FixedUpdate()
    {
        
        var smoothDelta = smoothTime * Time.deltaTime;
        Vector3 desiredPosition = target.position + offset;
        //Vector3 smoothedPosition = Vector3.SmoothDamp (transform.position, desiredPosition, ref velocity , smoothDelta, maxDelta);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothDelta);
        transform.position = smoothedPosition; 
    }

}
