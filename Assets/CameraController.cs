using UnityEngine;
using UnityEngine.Animations;

public class CameraController : MonoBehaviour
{
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player != null){
            ConstraintSource source = new ConstraintSource();
            source.sourceTransform = player.transform;
            source.weight = 1;
            GetComponent<LookAtConstraint>().AddSource(source);
            GetComponent<PositionConstraint>().AddSource(source);
        }
    }
}
