using UnityEngine;

public class EndStateManager : MonoBehaviour
{
    [SerializeField] private BarController barController;
    [SerializeField] private YearCounter yearCounter;

    [SerializeField] private int endYear = 2050;

    private void Update()
    {
        if (barController.temperature >= 46)
        {
            SceneLoader.LoadScene("HighPollution");
        }

        if (PopulationSimulator.Instance.GetPopulation <= 0)
        {
            SceneLoader.LoadScene("NoPopulation");
        }

        if (yearCounter.GetYear >= endYear)
        {
            SceneLoader.LoadScene("WinScene");
        }
    }
}
