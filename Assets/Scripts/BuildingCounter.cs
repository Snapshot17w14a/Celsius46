using TMPro;
using UnityEngine;

public class BuildingCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] buildingCounterText = new TextMeshProUGUI[9];
    [SerializeField] private int[] buildingCounter = new int[9];

    private static BuildingCounter instance;
    public static BuildingCounter Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<BuildingCounter>();
            }
            return instance;
        }
    }

    private void Start()
    {
        instance = this;
        foreach(var text in buildingCounterText)
        {
            text.text = "0";
        }
    }

    public void AddBuilding(int index)
    {
        buildingCounter[index]++;
        buildingCounterText[index].text = buildingCounter[index].ToString();
    }

    public void RemoveBuilding(int index)
    {
        buildingCounter[index]--;
        buildingCounterText[index].text = buildingCounter[index].ToString();
    }
}
