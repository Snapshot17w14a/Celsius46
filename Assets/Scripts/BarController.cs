using UnityEngine;
using TMPro;

public class BarController : MonoBehaviour
{
    public enum PollutionType { Air, Soil, Water }

    [SerializeField] private float[] pollutionEndValues = new float[3];
    [SerializeField] private float[] pollutionValues = new float[3];

    [SerializeField] private FillBar[] pollutionBars = new FillBar[3];

    [SerializeField] private int minTemperature = 25;
    [SerializeField] private int maxTemperature = 125;
    [SerializeField] private TextMeshProUGUI cumulativeTemperature;

    void Update()
    {
        UpdateFillBars();
        UpdateCumulativeTemperature();
    }

    private void UpdateFillBars()
    {
        for (int i = 0; i < pollutionBars.Length; i++) pollutionBars[i].SetFill(pollutionValues[i] / pollutionEndValues[i]);
    }

    private void UpdateCumulativeTemperature()
    {
        float cumulativePollution = 0f;
        foreach (FillBar bar in pollutionBars) cumulativePollution += bar.FillAmount;
        cumulativeTemperature.text = Mathf.RoundToInt(minTemperature + (maxTemperature - minTemperature) * (cumulativePollution / 3f)).ToString() + " C°";
    }

    public void AddPollution(float amountToAdd, PollutionType pollutionType)
    {
        pollutionBars[(int)pollutionType].SetFill((pollutionValues[(int)pollutionType] + amountToAdd) / pollutionEndValues[(int)pollutionType]);
    }
}
