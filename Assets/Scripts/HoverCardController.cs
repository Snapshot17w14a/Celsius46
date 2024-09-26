using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HoverCardController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI buildingName;
    [SerializeField] TextMeshProUGUI airPollutonValue;
    [SerializeField] TextMeshProUGUI soilPollutionValue;
    [SerializeField] TextMeshProUGUI waterPollutionValue;

    [SerializeField] private Image buildingIcon;

    [SerializeField] private GameObject hoverCard;

    void Update()
    {
        hoverCard.transform.position = Input.mousePosition;
        CheckForBuildingOverlap();
    }

    private void CheckForBuildingOverlap()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 40f, LayerMask.GetMask("Building")))
        {
            hoverCard.SetActive(true);
            Building building = hit.collider.GetComponent<Building>();
            UpdateValues(building.GetBuildingValues);
        }
        else hoverCard.SetActive(false);
    }

    private void UpdateValues(BuildingValues buildingValues)
    {
        buildingName.text = buildingValues.buildingName;
        buildingIcon.sprite = buildingValues.buildingIcon;
        airPollutonValue.text = buildingValues.airPollution.ToString();
        soilPollutionValue.text = buildingValues.soilPollution.ToString();
        waterPollutionValue.text = buildingValues.waterPollution.ToString();
    }
}
