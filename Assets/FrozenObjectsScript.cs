using UnityEngine;

public class FrozenObjectsScript : MonoBehaviour
{
    public ParticleSystem ParticleSystem;
    private bool HitOnce;

    public Rigidbody Rigidbody;
    
    void Start()
    {
        Rigidbody.Sleep();
    }

    void OnCollisionEnter(Collision collision)
    {
        if(!HitOnce){
            var randomExplosion = Random.Range(100f, 200f);
            Rigidbody.AddExplosionForce(randomExplosion, collision.transform.position, randomExplosion);
            HitOnce = true;
            ParticleSystem.Play();
        }
    }
}
