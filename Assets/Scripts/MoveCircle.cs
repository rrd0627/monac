using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCircle : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(0, 0, 10 * Time.fixedDeltaTime);
    }
}
