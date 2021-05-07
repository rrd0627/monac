using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadPrefab : MonoBehaviour
{
    void Start()
    {
        CharSelectPrefab.instance.gameObject.SetActive(true);
        StageSelectPrefab.instance.gameObject.SetActive(true);
    }
}
