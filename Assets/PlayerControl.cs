using System;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    Rigidbody Rigidbody;

    private float rotationSpeed = 100f;
    private float rotation = 0;
    private float xForce = 0;
    private float xForceRotation = 0;
    public GameObject Model;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Rigidbody = this.GetComponent<Rigidbody>();
        Rigidbody.Sleep();
    }

    // Update is called once per frames
    void Update()
    {
        RaycastHit raycastHit;
        Physics.Raycast(this.transform.position, Vector3.down, out raycastHit);
        var transformTarget = this.transform;
        transformTarget.up = raycastHit.normal;

        if(Input.GetKeyDown(KeyCode.Space)){
           Rigidbody.AddForce(new Vector3(0,0,1f), ForceMode.VelocityChange);
        }

        if(Input.GetKey(KeyCode.LeftArrow) && rotation > -90 && Rigidbody.linearVelocity.z > 0){
            rotation -= rotationSpeed * Time.deltaTime;
        }

        if(Input.GetKey(KeyCode.RightArrow) && rotation < 90 && Rigidbody.linearVelocity.z > 0){
            rotation += rotationSpeed * Time.deltaTime;
        }

        var absRotation = MathF.Abs(rotation);
        var zForce = 0.15f * (absRotation / 90) * Rigidbody.linearVelocity.z;
        xForceRotation = 0.5f * (rotation / 90) * Rigidbody.linearVelocity.magnitude;
        xForce = xForceRotation + (-Rigidbody.linearVelocity.x * (absRotation / 90));
        var force = new Vector3(xForce, 0, -zForce);
        Rigidbody.AddForce(force);

        Rigidbody.MoveRotation(Quaternion.Euler(0, rotation, 0));
        var targetModelRotation = Quaternion.Euler(transformTarget.rotation.eulerAngles.x, rotation, transformTarget.rotation.z);
        Model.transform.rotation = Quaternion.RotateTowards(Model.transform.rotation, targetModelRotation, 100f * Time.deltaTime);
    }
}
