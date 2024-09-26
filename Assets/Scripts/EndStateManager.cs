using UnityEngine;

public class EndStateManager : MonoBehaviour
{
    [SerializeField] private BarController barController;
    [SerializeField] private YearCounter yearCounter;
    [SerializeField] private GameEnd gameEnd;

    [SerializeField] private int endYear = 2050;

    private void Start()
    {
        Debug.Log(barController == null ? "BarController is null" : "BarController is assigned");
        Debug.Log(yearCounter == null ? "YearCounter is null" : "YearCounter is assigned");
    }

    private void Update()
    {
        if (barController.temperature >= 46)
        {
            gameEnd.EndGame();
            SceneLoader.LoadScene("HighPollution");
        }

        if (PopulationSimulator.Instance.GetPopulation <= 0)
        {
            gameEnd.EndGame();
            SceneLoader.LoadScene("NoPopulation");
        }

        if (yearCounter.GetYear >= endYear)
        {
            gameEnd.EndGame();
            SceneLoader.LoadScene("WinScene");
        }
    }
}
