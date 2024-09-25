using UnityEngine;

public class CanvasManager : MonoBehaviour
{
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
        pausedMenu.SetActive(true);
        playCanvas.enabled = false;
    }

    public void ClosePauseMenu()
    {
        pausedMenu.SetActive(false);
        playCanvas.enabled = true;
    }

    public void ShowPlayUI()
    {
        pausedMenu.SetActive(false);
        playCanvas.enabled = true;
    }

    public void CloseTutorial()
    {
        tutorial.SetActive(false);

        PopulationSimulator.Instance.StartSimulation();
    }
}
