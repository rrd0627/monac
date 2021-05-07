﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResoultion : MonoBehaviour
{    
    private void Awake()
    {
        Camera camera = GetComponent<Camera>();
        Rect rect = camera.rect;
        float scaleheight = ((float)Screen.width / Screen.height) / ((float)16 / 9);
        float scalewidth = 1f / scaleheight;

        if (scaleheight < 1)
        {
            //Screen.SetResolution(Screen.height * 16 / 9, Screen.height, false);
            rect.height = scaleheight;
            rect.y = (1f - scaleheight) / 2f;
        }
        else
        {
            //Screen.SetResolution(Screen.height * 16 / 9, Screen.height, false);
            rect.width = scalewidth;
            rect.x = (1f - scalewidth) / 2f;
        }
        camera.rect = rect;
    }
    private void OnPreCull() => GL.Clear(true, true, Color.black);
}
