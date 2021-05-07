using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectBarBlink : MonoBehaviour
{
    Color _color;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().color = new Color(1, 1, 1, 0);
        _color = new Color(1, 1, 1, 0);
    }

    private void OnEnable()
    {
        GetComponent<Image>().color = new Color(1, 1, 1, 0);
        _color = new Color(1, 1, 1, 0);
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void Blink()
    {
        StopAllCoroutines();
        GetComponent<Image>().color = new Color(1, 1, 1, 0);

        if(this.gameObject.activeSelf)
            StartCoroutine(BlinkCor());
    }
    IEnumerator BlinkCor()
    {
        while(true)
        {
            _color.a += 0.1f;
            GetComponent<Image>().color = _color;
            if (_color.a >= 0.8f)
                break;
            yield return null;
        }
        while (true)
        {
            _color.a -= 0.1f;
            GetComponent<Image>().color = _color;
            if (_color.a <= 0f)
                break;
            yield return null;            
        }
    }

}
