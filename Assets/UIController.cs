using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    private int gateCount = 0;
    private int snowMenCount = 0;
    private DateTime? startTime = null;
    private DateTime? endTime = null;

    public TextMeshProUGUI Time;
    public TextMeshProUGUI Snowmen;
    public TextMeshProUGUI Gate;

public GameObject WinScreen;
public GameObject LoseScreen;

    // Update is called once per frame
    void Update()
    {
        if(startTime.HasValue)
        {
            var passedTime = DateTime.Now-startTime.Value;
            Time.text = $"Time: {passedTime.Minutes:D2}:{passedTime.Seconds:D2}";
        }else if(startTime.HasValue && endTime.HasValue)
        {   
            var passedTime = endTime.Value-startTime.Value;
            Time.text = $"Time: {passedTime.Minutes:D2}:{passedTime.Seconds:D2}";
        }else{
            Time.text = "Time: 00:00";
        }
        
        Snowmen.text = snowMenCount.ToString();
        Gate.text = gateCount.ToString();
    }

    public void SnowmanHit(){
        snowMenCount++;
    }

    public void Start()
    {
        if(!startTime.HasValue){
            startTime = DateTime.Now;
        }
    }

    public void GateHit()
    {
        gateCount++;
    }

    public void Died()
    {
        if(!endTime.HasValue){
            endTime = DateTime.Now;
        }
        LoseScreen.SetActive(true);
    }

    public void LevelWon()
    {
        if(!endTime.HasValue){
            endTime = DateTime.Now;
        }
        WinScreen.SetActive(true);
    }

    public void Restart(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
