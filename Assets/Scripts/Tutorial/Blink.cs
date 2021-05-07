using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour
{
    SpriteRenderer _sprite;
    Color _color;
    int plusminus;
    // Start is called before the first frame update

    private void OnEnable()
    {
        plusminus = 1;
        _sprite = this.GetComponent<SpriteRenderer>();
        _color = _sprite.color;
        StartCoroutine(BlinkObject());
    }
    IEnumerator BlinkObject()
    {
        while(true)
        {
            _color.a += 0.05f * plusminus;

            if(_color.a<0.1f)
            {
                plusminus= 1;
            }
            else if(_color.a > 0.9f)
            {
                plusminus = -1;
            }
            _sprite.color = _color;

            yield return null;
        }


    }

}
