using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float y;
    private float z;
    public float FollowSpeed = 2f;
    public Transform target;

    // Update is called once per frame
    void Start(){
        z = transform.position.z;
    }

    void FixedUpdate()
    {
        Vector3 newPos = new Vector3(target.position.x, target.position.y, z);
        transform.position = Vector3.Slerp(transform.position,newPos,FollowSpeed*Time.deltaTime);
    }
}
