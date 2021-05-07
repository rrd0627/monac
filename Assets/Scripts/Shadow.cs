using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        spriteRenderer.sprite = this.transform.parent.GetComponentInParent<SpriteRenderer>().sprite;
    }
}
