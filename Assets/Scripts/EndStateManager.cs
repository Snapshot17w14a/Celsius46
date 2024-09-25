using UnityEngine;

public class EndStateManager : MonoBehaviour
{
    [SerializeField] private BarController barController;
    [SerializeField] private YearCounter yearCounter;

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
            SceneLoader.LoadScene("LoseScene");
        }

        if (PopulationSimulator.Instance.GetPopulation <= 0)
        {
            SceneLoader.LoadScene("LoseScene");
        }

        if (yearCounter.GetYear >= endYear)
        {
            SceneLoader.LoadScene("WinScene");
        }
    }
}
