using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedTile : MonoBehaviour
{
    private SpriteRenderer sprite;
    private Color _color;
    private int flag;
    // Start is called before the first frame update
    void Start()
    {
        flag = 1;
        sprite = GetComponent<SpriteRenderer>();
        _color = sprite.color;
        Destroy(this.gameObject,1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        _color.a -= flag * 0.01f;
        sprite.color = _color;
        if (_color.a <= 0 || _color.a >= 0.5f)
            flag *= -1;
    }
}
