using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    [SerializeField] private PlantValues plantValues;

    public int GetActionCost => plantValues.actionCost;

    public float GetPollutionValues(BarController.PollutionType pollutionType)
    {
        return (int)pollutionType switch
        {
            0 => plantValues.airPollution,
            1 => plantValues.soilPollution,
            2 => plantValues.waterPollution,
            _ => 0
        };
    }
}
