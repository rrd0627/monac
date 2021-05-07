using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeScript : MonoBehaviour
{
    private Text TimeText;

    public float timer;
    private int min;
    // Start is called before the first frame update
    void Start()
    {
        TimeText = this.GetComponent<Text>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(GameManager.instance.IsGameStart)
        {
            GameManager.instance.TimeForText += 0.02f;
            timer += 0.02f;
        }
            
        if (timer >= 60)
        {
            timer = 0;
            min++;
        }
    }

    void OnGUI()
    {
        string timeStr;
        timeStr = min.ToString("00")+ ":" +  ((int)timer).ToString("00");
        timeStr = timeStr.Replace(".", ":");
        TimeText.text = timeStr;
    }
}
