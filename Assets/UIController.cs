using System;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    private int snowMenCount = 0;
    private DateTime? startTime = null;

    public TextMeshProUGUI Time;
    public TextMeshProUGUI Snowmen;


    // Update is called once per frame
    void Update()
    {
        if(startTime.HasValue)
        {
            var passedTime = DateTime.Now-startTime.Value;
            Time.text = $"Time: {passedTime.Minutes:D2}:{passedTime.Seconds:D2}";
        }else{
            Time.text = "Time: 00:00";
        }
        
        Snowmen.text = snowMenCount.ToString();
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
}
