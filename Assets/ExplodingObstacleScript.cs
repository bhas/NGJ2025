using UnityEngine;

public class SnowmanScript : MonoBehaviour
{
    private Rigidbody[] Rigidbodies;
    private BoxCollider BoxCollider;

    public ParticleSystem ParticleSystem;

    public UIController uiController;

    void Start()
    {
        BoxCollider = GetComponent<BoxCollider>();
        Rigidbodies = GetComponentsInChildren<Rigidbody>();
        uiController = GameObject.FindGameObjectWithTag("UI")?.GetComponent<UIController>();   
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q)){
            Explode(this.transform.position);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Explode(other.transform.position);
    }

    private void Explode(Vector3 hitPoint)
    {
        Camera.main.GetComponent<CameraController>().StartSlowMotion();
        uiController.SnowmanHit();
        
        ParticleSystem.Play();

        BoxCollider.enabled = false;
        foreach(var body in Rigidbodies){
            body.isKinematic = false;
            var randomExplosion = Random.Range(300f, 1000f);
            body.AddExplosionForce(randomExplosion, hitPoint, randomExplosion);
        }
    }
}
