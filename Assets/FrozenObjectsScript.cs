using UnityEngine;

public class FrozenObjectsScript : MonoBehaviour
{
    public ParticleSystem ParticleSystem;
    
    void Start()
    {
        GetComponent<Rigidbody>().Sleep();
    }

    void OnCollisionEnter(Collision collision)
    {
        ParticleSystem.Play();
    }
}
