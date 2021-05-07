using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quit : MonoBehaviour
{
    public GameObject QuitPanel;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitPanel.SetActive(true);
        }
    }

    public void QuitApli()
    {
        Application.Quit();
    }
    public void QuitCancel()
    {
        QuitPanel.SetActive(false);
    }
}
