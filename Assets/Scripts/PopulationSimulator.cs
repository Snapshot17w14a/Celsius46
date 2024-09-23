using System.Collections;
using UnityEngine;
using TMPro;

public class PopulationSimulator : MonoBehaviour
{
    [Header("Simulation idle time")]
    [Tooltip("The time in seconds the simulation will idle before the next step.")]
    [SerializeField] private float simulationIdleTime;

    [Header("Chance for growth")]
    [Tooltip("The chance for every 2 population to have a child and thus increase the population.")]
    [SerializeField] private float childChance;

    [Header("Building value")]
    [Tooltip("The percentage of the max population required to spawn a new building.")]
    [SerializeField] private float populationForNewBuilding;

    [Header("Action value")]
    [Tooltip("The amount of population required to gain an action point.")]
    [SerializeField] private int populationForActionPoint;
    [SerializeField] private int maxActionPoints;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI populationDisplay;
    [SerializeField] private TextMeshProUGUI actionDisplay;
    [SerializeField] private BarController barController;
    [SerializeField] private Material planetMaterial;

    private int population = 10;
    private int maxPopulation = 0;

    private int availableActionPoints = 0;

    public int AvailableActionPoints => availableActionPoints;
    public int GetPopulation => population;

    private static PopulationSimulator instance;
    public static PopulationSimulator Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PopulationSimulator>();
            }
            return instance;
        }
    }

    void Start()
    {
        PlanetPrefabSpawner.Instance.SpawnRandomPrefab();
        StartCoroutine(SimulationStep());
    }

    private IEnumerator SimulationStep()
    {
        while (true)
        {
            //Get all current buildings
            var buildings = FindObjectsByType<Building>(FindObjectsSortMode.None);
            var plants = FindObjectsByType<Plant>(FindObjectsSortMode.None);

            //Get the max possible population based on the buildings available
            maxPopulation = GetMaxPopulation(buildings);

            //Set the population based on the potential population growth
            population = Mathf.Clamp(population += GetPotentionPopulation(), 0, maxPopulation);

            //Check if the population is high enough to gain an action point
            CheckForActionThreshold();

            //Add the pollution produced by all buindings and update the planet blend
            AddPollution(buildings, plants);
            UpdatePlanetBlend();

            //Update the display texts
            UpdateDisplayText();
            

            //If population is high enough, spawn a new building
            if (NewBuildingRequired()) PlanetPrefabSpawner.Instance.SpawnRandomPrefab();

            //Idle the simulation for set seconds
            yield return new WaitForSeconds(simulationIdleTime);
        }
    }

    private int GetMaxPopulation(Building[] buildings)
    {
        int tempMaxPopulation = 0;
        foreach(var building in buildings)
        {
            tempMaxPopulation += building.GetUpkeepValue;
        }
        return tempMaxPopulation;
    }

    private int GetPotentionPopulation()
    {
        return Mathf.RoundToInt(population / 2 * (childChance / 100));
    }

    private void AddPollution(Building[] buildings, Plant[] plants)
    {
        for (int i = 0; i < 3; i++)
        {
            float pollutionToAdd = 0;
            foreach(var building in buildings)
            {
                pollutionToAdd += building.GetPollutionValues((BarController.PollutionType)i);
            }
            foreach(var plant in plants)
            {
                pollutionToAdd -= plant.GetPollutionValues((BarController.PollutionType)i);
            }
            barController.AddPollution(pollutionToAdd, (BarController.PollutionType)i);
        }
    }

    private void UpdateDisplayText()
    {
        actionDisplay.text = "Action Points: " + availableActionPoints;
    }

    private void CheckForActionThreshold()
    {
        availableActionPoints = Mathf.Clamp(availableActionPoints + (population / populationForActionPoint), 0, maxActionPoints);
    }

    private void UpdatePlanetBlend()
    {
        planetMaterial.SetFloat("_CurrentStage", barController.pollutionProgress);
        populationDisplay.text = "Population: " + population;
    }

    private bool NewBuildingRequired()
    {
       return population >= maxPopulation * (populationForNewBuilding / 100f);
    }

    public void SubtractMaxPopulation(int amount)
    {
        maxPopulation -= amount;
        UpdateDisplayText();
    }

    public void SubtractActionPoint(int amount)
    {
        availableActionPoints -= amount;
        UpdateDisplayText(); 
    }
}
