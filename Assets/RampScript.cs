using UnityEngine;

public class RampScript : MonoBehaviour
{
    public ParticleSystem ParticleSystem;

    void Start()
    {
    }

   void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player"){
            Score();
        }
    }

    private void Score()
    {
        Camera.main.GetComponent<CameraController>().StartSlowMotion();
        ParticleSystem.Play();
    }
}
