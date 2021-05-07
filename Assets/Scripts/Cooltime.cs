using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cooltime : MonoBehaviour
{
    public bool IsObstacle;

    public Image thisImage;

    // Update is called once per frame
    void Update()
    {
        if(IsObstacle)
        {
            if (GameManager.instance.BarricateLeft == 0)
            {
                thisImage.fillAmount = GameManager.instance.ObstacleCooltime_Cur / GameManager.instance.ObstacleCooltime;                
            }                
            else
                thisImage.fillAmount = 1;
        }
        else
        {
            thisImage.fillAmount = GameManager.instance.BridgeCooltime_Cur / GameManager.instance.BridgeCooltime;
        }
    }
}
