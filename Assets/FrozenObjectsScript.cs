using UnityEngine;

public class FrozenObjectsScript : MonoBehaviour
{
    void Start()
    {
        GetComponent<Rigidbody>().Sleep();
    }
}
