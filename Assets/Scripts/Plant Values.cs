using UnityEngine;

[CreateAssetMenu(fileName = "Plant Values", menuName = "Plant Values", order = 1)]
public class PlantValues : ScriptableObject
{
    [Header("Plant values")]
    [Tooltip("The amount of action points it costs to place the plant.")]
    public int actionCost;

    [Header("Pollution values")]
    [Tooltip("The amount of pollution a plant produces.")]
    public float airPollution;
    public float soilPollution;
    public float waterPollution;
}
