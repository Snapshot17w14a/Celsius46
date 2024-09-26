using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ModeSwitch : MonoBehaviour
{
    [SerializeField] public bool currentMode = false;
    [SerializeField] Sprite destroyIcon;
    [SerializeField] Sprite buildIcon;
    [SerializeField] Image buttonImage;
    [SerializeField] Mode modes;

    public enum Mode
    {
        build = 0,
        destroy = 1,
    }

    public Mode GetterCurrentMode => modes;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            SwitchMode();
        }
    }

    public void SwitchMode()
    {
        currentMode = !currentMode;
        modes = currentMode ? Mode.build : Mode.destroy;

        buttonImage.sprite = currentMode ? buildIcon : destroyIcon;
    }
}
