using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkText : MonoBehaviour
{
    Text _text;
    Color _color;
    int plusminus;
    // Start is called before the first frame update

    private void OnEnable()
    {
        plusminus = 1;
        _text = this.GetComponent<Text>();
        _color = _text.color;
        StartCoroutine(BlinkObject());
    }
    IEnumerator BlinkObject()
    {
        while (true)
        {
            _color.a += 0.02f * plusminus;

            if (_color.a < 0.1f)
            {
                plusminus = 1;
            }
            else if (_color.a > 0.9f)
            {
                plusminus = -1;
            }
            _text.color = _color;

            yield return null;
        }


    }

}
