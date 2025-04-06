using UnityEngine;

public class FrozenObjectsScript : MonoBehaviour
{
    public ParticleSystem ParticleSystem;
    private bool HitOnce;
    
    void Start()
    {
        GetComponent<Rigidbody>().Sleep();
    }

    void OnCollisionEnter(Collision collision)
    {
        if(HitOnce){
            HitOnce = true;
            ParticleSystem.Play();
        }
    }
}
