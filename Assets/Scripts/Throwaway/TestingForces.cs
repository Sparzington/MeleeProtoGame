using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingForces : MonoBehaviour
{
    private Rigidbody _rigidBody;
    public float MovementAccel = 5;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            _rigidBody.AddForce(transform.forward * MovementAccel, ForceMode.Force);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            _rigidBody.AddForce(-transform.forward * MovementAccel, ForceMode.Force);
        }
    }
}
