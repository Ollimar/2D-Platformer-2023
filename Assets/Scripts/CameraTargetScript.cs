using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTargetScript : MonoBehaviour
{

    public Transform target;

    public float posY;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        transform.position = new Vector3(target.position.x, posY, transform.position.z);
    }
}
