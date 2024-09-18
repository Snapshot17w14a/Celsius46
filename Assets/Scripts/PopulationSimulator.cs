using System.Collections;
using TMPro;
using UnityEngine;

public class PopulationSimulator : MonoBehaviour
{
    [Header("Simulation idle")]
    [SerializeField] private float simulationIdleTime;

    [Header("Chance for growth")]
    [SerializeField] private float childChance;

    [SerializeField] private TextMeshProUGUI populationDisplay;
    [SerializeField] private BarController barController;

    private int population = 10;
    private int maxPopulation = 0;

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
        StartCoroutine(SimulationStep());
    }

    private IEnumerator SimulationStep()
    {
        while (true)
        {
            //Get all current buildings
            var buildings = FindObjectsByType<Building>(FindObjectsSortMode.None);

            //Get the max possible population based on the buildings available
            maxPopulation = GetMaxPopulation(buildings);

            //Set the population based on the potential population growth
            population = Mathf.Clamp(population += GetPotentionPopulation(), 0, maxPopulation);

            //Add the pollution produced by all buindings
            AddPollution(buildings);

            //Display the population
            populationDisplay.text = "Population: " + population;

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

    private void AddPollution(Building[] buildings)
    {
        for (int i = 0; i < 3; i++)
        {
            float pollutionToAdd = 0;
            foreach(var building in buildings)
            {
                pollutionToAdd += building.GetPollutionValues((BarController.PollutionType)i);
            }
            barController.AddPollution(pollutionToAdd, (BarController.PollutionType)i);
        }
    }

    public void SubtractMaxPopulation(int amount)
    {
        maxPopulation -= amount;
    }
}
