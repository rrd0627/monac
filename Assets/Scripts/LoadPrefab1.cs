using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadPrefab1 : MonoBehaviour
{
    void Start()
    {
        CharSelectPrefab.instance.gameObject.SetActive(false);
        StageSelectPrefab.instance.gameObject.SetActive(true);
    }

}
