using UnityEngine;

public class SnowmanScript : MonoBehaviour
{
    private Rigidbody[] Rigidbodies;
    private BoxCollider BoxCollider;

    

    void Start()
    {
        BoxCollider = GetComponent<BoxCollider>();
        Rigidbodies = GetComponentsInChildren<Rigidbody>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q)){
            Explode();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Explode();
    }

    private void Explode()
    {
        Camera.main.GetComponent<CameraController>().StartSlowMotion();

        BoxCollider.enabled = false;
        foreach(var body in Rigidbodies){
            body.isKinematic = false;
            var randomExplosion = Random.Range(200f, 500f);
            var randomVector = new Vector3(Random.Range(0.9f,0.9f),Random.Range(0.9f,0.9f),Random.Range(0.9f,0.9f));
            body.AddExplosionForce(200f, this.transform.position + randomVector, randomExplosion);
        }
    }
}
