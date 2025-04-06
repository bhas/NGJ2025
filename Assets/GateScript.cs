using UnityEngine;

public class GateScript : MonoBehaviour
{
    public ParticleSystem ParticleSystem;

    public UIController uiController;

    void Start()
    {
        uiController = GameObject.FindGameObjectWithTag("UI")?.GetComponent<UIController>();   
    }

   void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player"){
            Score();
        }
    }

    private void Score()
    {
        Camera.main.GetComponent<CameraController>().StartSlowMotion(0.05f);
        uiController.GateHit();
        
        ParticleSystem.Play();
    }
}
