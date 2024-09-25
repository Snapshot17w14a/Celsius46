using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    private bool isPausedMenuVisible = false;
    private bool isPlayUIVisible = false;
    private bool isTutorialVisible = true;

    [SerializeField] private GameObject pausedMenu;
    [SerializeField] private GameObject playUI;
    [SerializeField] private GameObject tutorial;

    private Canvas playCanvas;

    private void Start()
    {
        playCanvas = playUI.GetComponent<Canvas>();
    }

    public void ShowPausedMenu()
    {
        isPausedMenuVisible = true;
        isPlayUIVisible = false;

        pausedMenu.SetActive(true);
        playCanvas.enabled = false;
    }

    public void ClosePauseMenu()
    {
        isPausedMenuVisible = false;
        isPlayUIVisible = true;

        pausedMenu.SetActive(false);
        playCanvas.enabled = true;
    }

    public void ShowPlayUI()
    {
        isPausedMenuVisible = false;
        isPlayUIVisible = true;

        pausedMenu.SetActive(false);
        playCanvas.enabled = true;
    }

    public void CloseTutorial()
    {
        isTutorialVisible = false;

        tutorial.SetActive(false);

        PopulationSimulator.Instance.StartSimulation();
    }
}
