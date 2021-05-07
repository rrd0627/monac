using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    public GameObject target;

    Image HPimage;

    Unit _unit;

    private void Start()
    {
        _unit = target.GetComponent<Unit>();
        HPimage = this.GetComponent<Image>();
    }
    private void OnDisable()
    {
        if (!GameManager.instance.IsGameStart) return;
        HPimage.fillAmount = 0;
        HPimage.color = Color.white;
    }
    void Update()
    {
        if(_unit.UnitLevel == 0)
        {
            HPimage.fillAmount = _unit.Amount / 500f;
            HPimage.color = Color.white;
        }
        else if(_unit.UnitLevel == 1)
        {
            HPimage.fillAmount = _unit.Amount / 2000f;
            HPimage.color = Color.yellow;
        }
        else if (_unit.UnitLevel == 2)
        {
            HPimage.fillAmount = _unit.Amount / 5000f;
            HPimage.color = Color.green;
        }
        else 
        {
            HPimage.fillAmount = _unit.Amount / 10000f;
            HPimage.color = Color.magenta;
        }
    }
}