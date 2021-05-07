using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarricateHPbar : MonoBehaviour
{
    public GameObject target;
    
    Image HPimage;

    TreeAndBush Barricate;

    private void Start()
    {
        Barricate = target.GetComponent<TreeAndBush>();
        HPimage = this.GetComponent<Image>();
    }
    private void OnDisable()
    {
        if (!GameManager.instance.IsGameStart) return;
        HPimage.fillAmount = 0;
    }
    void Update()
    {
        HPimage.fillAmount = Barricate.HP / (float)500;
    }
}