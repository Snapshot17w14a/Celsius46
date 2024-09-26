using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private Material planetMaterial;
    [SerializeField] private CanvasManager canvasManager;
    [SerializeField] private YearCounter yearCounter;
    [SerializeField] private GameObject[] tutorialsWindows;

    private int currentTutorialIndex = 0;

    private void Start()
    {
        planetMaterial.SetFloat("_CurrentStage", 0.0f);
    }

    public void IncrementTutorialWindowIndex()
    {
        tutorialsWindows[currentTutorialIndex].SetActive(false);
        tutorialsWindows[++currentTutorialIndex].SetActive(true);
    }

    public void DecrementTutorialWindowIndex()
    {
        tutorialsWindows[currentTutorialIndex].SetActive(false);
        tutorialsWindows[--currentTutorialIndex].SetActive(true);
    }

    public void CloseTutorial()
    {
        //PlayerPrefs.SetInt("TutorialCompleted", 1);
        gameObject.SetActive(false);
        canvasManager.CloseTutorial();
        yearCounter.StartCounter();
    }
}