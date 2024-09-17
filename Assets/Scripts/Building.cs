using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] private BuildingValues buildingValues;

    public int GetUpkeepValue => buildingValues.popultionSupport;
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
}
