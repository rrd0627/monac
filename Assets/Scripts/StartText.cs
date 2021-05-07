using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartEffect());
    }
    
    IEnumerator StartEffect()
    {
        while(true)
        { //커짐
            this.transform.localScale *= 1.1f;
            yield return null;
            if (this.transform.localScale.x >= 190)
                break;
        }
        while (true)
        { //천천히 커짐
            this.transform.localScale *= 1.01f;
            yield return null;
            if (this.transform.localScale.x >= 200)
                break;
        }
        while (true)
        { //천천히 커짐
            this.transform.localScale *= 0.99f;
            yield return null;
            if (this.transform.localScale.x <= 180)
                break;
        }
        while (true)
        {
            this.transform.localScale *= 0.8f;
            yield return null;
            if (this.transform.localScale.x <= 20)
                break;
        }
        Destroy(this.gameObject);
    }




}
