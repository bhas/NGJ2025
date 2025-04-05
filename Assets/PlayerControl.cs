using System;
using UnityEditor;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    Rigidbody Rigidbody;

    private float rotationSpeed = 0.1f;
    private float rotation = 0;
    public float xForce = 0;
    public float xForceRotation = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Rigidbody = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frames
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
           Rigidbody.AddForce(new Vector3(0,0,1f), ForceMode.VelocityChange);
        }

        if(Input.GetKey(KeyCode.LeftArrow) && rotation > -90){
            // Rigidbody.AddForce(new Vector3(-Force,0,0));
            rotation -= rotationSpeed;
        }

        if(Input.GetKey(KeyCode.RightArrow) && rotation < 90){
            // Rigidbody.AddForce(new Vector3(Force,0,0));
            rotation += rotationSpeed;
        }

        var absRotation = MathF.Abs(rotation);
        var zForce = 0.25f * (absRotation / 90) * Rigidbody.linearVelocity.z;
        xForceRotation = 0.5f * (rotation / 90) * Rigidbody.linearVelocity.magnitude;
        xForce = xForceRotation + (-Rigidbody.linearVelocity.x * (absRotation / 90));
        var force = new Vector3(xForce, 0, -zForce);
        Rigidbody.AddForce(force);

        Rigidbody.MoveRotation(Quaternion.Euler(0, rotation, 0));
    }
}
