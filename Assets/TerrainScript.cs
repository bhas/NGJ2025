using UnityEngine;

public class TerrainScript : MonoBehaviour
{
    public UIController uiController;

    void Start()
    {
        uiController = GameObject.FindGameObjectWithTag("UI")?.GetComponent<UIController>();   
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player"){
            Camera.main.GetComponent<CameraController>().LevelWon();
            uiController.Died();
        }
        Destroy(collision.gameObject);
    }
}
