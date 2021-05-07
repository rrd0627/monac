using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockMotion : MonoBehaviour
{
    public GameObject LockUp;

    public SpriteRenderer[] sprites;


    private Color color;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 2f);

        color = sprites[0].color;
    }

    // Update is called once per frame
    void Update()
    {
        LockUp.transform.Translate(Vector3.up * Time.deltaTime);

        color.a -= Time.deltaTime*2;

        sprites[0].color = color;
        sprites[1].color = color;
    }
}
