using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] private BuildingValues buildingValues;
    //[SerializeField] private GameObject infoCard;

    public int GetUpkeepValue => buildingValues.popultionSupport;
    public (int, int) GetPopulationValues => (buildingValues.minPopulation, buildingValues.maxPopulation);
    public float GetPollutionValues(BarController.PollutionType pollutionType)
    {
        return (int)pollutionType switch
        {
            0 => buildingValues.airPollution,
            1 => buildingValues.soilPollution,
            2 => buildingValues.waterPollution,
            _ => 0
        };
    }

    private void OnDestroy()
    {
        if(PopulationSimulator.Instance != null)
        {
            PopulationSimulator.Instance.SubtractMaxPopulation(buildingValues.popultionSupport);
        }
    }

    //private void OnMouseEnter()
    //{
    //    infoCard.SetActive(true);
    //}

    //private void OnMouseExit()
    //{
    //    infoCard.SetActive(false);
    //}
}
