using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeSwitch : MonoBehaviour
{
    [SerializeField] public int currentMode = 0;
    [SerializeField] GameObject destroyIcon;
    [SerializeField] GameObject buildIcon;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchMode();
        }
    }
    public void SwitchMode()
    {
        if (currentMode == 0)
        {
            buildIcon.SetActive(true);
            destroyIcon.SetActive(false);
            Debug.Log("Switched to build!");
            currentMode = 1;
        }
        else if (currentMode == 1)
        {
            buildIcon.SetActive(false);
            destroyIcon.SetActive(true);
            Debug.Log("Switched to destroy!");
            currentMode = 0;
        }
    }
}
