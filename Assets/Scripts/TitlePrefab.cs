﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitlePrefab : MonoBehaviour
{
    static public TitlePrefab instance;

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
    }
}
