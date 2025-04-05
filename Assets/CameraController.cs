using System.Collections;
using UnityEngine;
using UnityEngine.Animations;

public class CameraController : MonoBehaviour
{
    private bool slowMotion;
    float timeScale = 1f;
    float timeToScaleDown = 0.09f;

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

    void Update()
    {

    }

    public void StartSlowMotion(){
        if(!slowMotion){
            StartCoroutine(SlowDownTime(true));
        }
    }

     private IEnumerator SlowDownTime(bool StartUpAgain)
    {
        slowMotion = true;
        timeScale = 1f;
        for (int i = 0; i < 10; i++)
        {
            timeScale -= timeToScaleDown;
            Time.timeScale = timeScale;
            Time.fixedDeltaTime = 0.02F * Time.timeScale;
            yield return new WaitForSeconds(0.02f);
        }

        yield return new WaitForSeconds(0.1f);

        if (StartUpAgain)
            StartCoroutine(SpeedUpTime());
    }

    private IEnumerator SpeedUpTime()
    {
        for (int i = 10; i > 0; i--)
        {
            timeScale += timeToScaleDown;
            Time.timeScale = timeScale;
            Time.fixedDeltaTime = 0.02F * Time.timeScale;
            yield return new WaitForSeconds(0.02f);
        }
        slowMotion = false;
    }
}
