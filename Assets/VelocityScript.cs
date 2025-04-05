using TMPro;
using UnityEngine;

public class VelocityScript : MonoBehaviour
{
    public Rigidbody Rigidbody;
    public TextMeshProUGUI TextMeshPro;
    public PlayerControl playerControl;

    // Update is called once per frame
    void Update()
    {
        TextMeshPro.text = $"Velocity: {Rigidbody.linearVelocity.ToString()} \nxForceRotation: {playerControl.xForceRotation}\nxForce: {playerControl.xForce}\n";
    }
}
