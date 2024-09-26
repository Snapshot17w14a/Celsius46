using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] private BuildingValues buildingValues;

    public int GetUpkeepValue => buildingValues.popultionSupport;
    public (int, int) GetPopulationValues => (buildingValues.minPopulation, buildingValues.maxPopulation);
    public BuildingValues GetBuildingValues => buildingValues;

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

    private void Start()
    {
        BuildingCounter.Instance.AddBuilding(buildingValues.index);
    }

    private void OnDestroy()
    {
        BuildingCounter.Instance?.RemoveBuilding(buildingValues.index);
        PopulationSimulator.Instance?.SubtractMaxPopulation(buildingValues.popultionSupport);
        PopulationSimulator.Instance?.SubtractPopulation(buildingValues.toKillOnDestruction);
    }
}