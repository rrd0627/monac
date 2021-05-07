using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightPrefab : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke("Vanish", 0.5f);
    }
    private void Vanish()
    {
        this.gameObject.SetActive(false);
    }
}
