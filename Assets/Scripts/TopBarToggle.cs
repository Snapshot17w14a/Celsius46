using UnityEngine;

public class TopBarToggle : MonoBehaviour
{
    [SerializeField] private GameObject topBar;

    private bool isTopBarActive = true;

    public void ToggleBar()
    {
        isTopBarActive = !isTopBarActive;
        topBar.SetActive(isTopBarActive);
    }
}
