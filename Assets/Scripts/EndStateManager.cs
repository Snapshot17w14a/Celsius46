using UnityEngine;

public class EndStateManager : MonoBehaviour
{
    [SerializeField] private BarController barController;
    [SerializeField] private YearConunter yearConunter;

    [SerializeField] private int endYear = 2050;

    private void Update()
    {
        if (barController.pollutionProgress >= 1)
        {
            SceneLoader.LoadScene("LoseScene");
        }

        if (PopulationSimulator.Instance.GetPopulation <= 0)
        {
            SceneLoader.LoadScene("LoseScene");
        }

        if (yearConunter.GetYear >= endYear)
        {
            SceneLoader.LoadScene("WinScene");
        }
    }
}
