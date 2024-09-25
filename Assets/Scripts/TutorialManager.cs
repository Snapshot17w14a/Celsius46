using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private CanvasManager canvasManager;
    [SerializeField] private GameObject[] tutorialsWindows;

    private int currentTutorialIndex = 0;

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
    }
}