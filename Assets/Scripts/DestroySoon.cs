using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySoon : MonoBehaviour
{
    private const float timer = 1f;
    private float timertemp;

    private void Start()
    {
        Destroy(this.gameObject, 1f);
        timertemp = 0;
    }
    
    public void DestroyObject()
    {
        StartCoroutine(DestroyObjectCor());
    }

    IEnumerator DestroyObjectCor()
    {
        while(true)
        {
            timertemp += 0.02f;
            if (timertemp > timer)
            {
                break;
            }
            yield return null;
        }        
        Destroy(this.gameObject);
    }
}
