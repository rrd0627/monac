using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharSelectPrefab : MonoBehaviour
{
    static public CharSelectPrefab instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        this.gameObject.SetActive(false);
    }
}
