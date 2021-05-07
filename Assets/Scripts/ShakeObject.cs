using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeObject : MonoBehaviour
{
    int plusminus;

    float dist;

    // Start is called before the first frame update
    private void OnEnable()
    {
        dist = 0;
        plusminus = 1;
        StartCoroutine(ShakeObjectCor());
    }
    IEnumerator ShakeObjectCor()
    {
        while (true)
        {
            transform.Translate(plusminus * Vector3.up * Time.fixedDeltaTime);
            dist += Time.fixedDeltaTime;

            if (dist > 0.25f)
            {
                plusminus *= -1;
                dist = 0;
            }
            yield return null;
        }
    }


}
