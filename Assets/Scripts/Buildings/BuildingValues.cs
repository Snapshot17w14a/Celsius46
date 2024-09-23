using UnityEngine;

[CreateAssetMenu(fileName = "Building Values", menuName = "Building Values", order = 0)]
public class BuildingValues : ScriptableObject
{
    [Header("The amount of humans the building can upkeep")]
    public int popultionSupport;

    [Header("The amount of pollution the building produces each step of the simulation")]
    public float airPollution;
    public float soilPollution;
    public float waterPollution;

    [Header("Population values for spawning")]
    public int minPopulation;
    public int maxPopulation;

    [Header("UI display settings")]
    public string buildingName;
    public Sprite buildingIcon;
}
