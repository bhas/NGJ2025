using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraController : MonoBehaviour
{
    private bool slowMotion;
    float timeScale = 1f;
    float timeToScaleDown = 0.09f;

    Transform Target;
    public float Distance;
    public float Height;
    public VolumeProfile volume;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Target = player.transform;
    }

    void Update()
    {
        var targetPos = new Vector3(Target.position.x, Target.position.y + Height, Target.position.z - Distance);
        gameObject.transform.LookAt(Target);
        gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, targetPos, 2.5f * Time.deltaTime);

        if(Input.GetKeyDown(KeyCode.K)){
            LevelWon();
        }
    }

    public void LevelWon(){
        Height = 1;
        Distance = 1;
        StartCoroutine(SlowDownTime(false, 1f));
    }

    public void StartSlowMotion(){
        if(!slowMotion){
            StartCoroutine(SlowDownTime(true));
        }
    }

    private IEnumerator SlowDownTime(bool StartUpAgain, float waitBeforeSlow = 0.0f)
    {
        yield return new WaitForSeconds(waitBeforeSlow);

        slowMotion = true;
        timeScale = 1f;
        for (int i = 0; i < 10; i++)
        {
            timeScale -= timeToScaleDown;
            Time.timeScale = timeScale;
            Time.fixedDeltaTime = 0.02F * Time.timeScale;
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(0.2f);

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
