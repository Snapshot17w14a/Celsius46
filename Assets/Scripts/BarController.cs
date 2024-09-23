using UnityEngine;
using TMPro;

public class BarController : MonoBehaviour
{
    public enum PollutionType 
    { 
        Air      = 0,
        Soil     = 1,
        Water    = 2 
    }

    [SerializeField] private float[] pollutionEndValues = new float[3];
    [SerializeField] private float[] pollutionValues = new float[3];

    //[SerializeField] private FillBar[] pollutionBars = new FillBar[3];
    [SerializeField] private TextMeshProUGUI[] pollutionTexts = new TextMeshProUGUI[3];

    [SerializeField] private int minTemperature = 25;
    [SerializeField] private int maxTemperature = 125;
    [SerializeField] private TextMeshProUGUI cumulativeTemperature;

    public float PollutionValue
    {
        get
        {
            float max = 0f;
            foreach (float f in pollutionValues) max += f;
            return max;
        }
    }

    public float MaxPollution
    {
        get
        {
            float max = 0f;
            foreach (float f in pollutionEndValues) max += f;
            return max;
        }
    }

    public float pollutionProgress
    {
        get
        {
            return PollutionValue / MaxPollution;
        }
    }

    void Update()
    {
        //UpdateFillBars();
        UpdatePollutionTexts();
        UpdateCumulativeTemperature();
    }

    //private void UpdateFillBars()
    //{
    //    for (int i = 0; i < pollutionBars.Length; i++) pollutionBars[i].SetFill(pollutionValues[i] / pollutionEndValues[i]);
    //}

    private void UpdatePollutionTexts()
    {
        for (int i = 0; i < pollutionTexts.Length; i++) pollutionTexts[i].text = Mathf.Round((pollutionValues[i] / pollutionEndValues[i]) * 100).ToString() + "%";
    }

    private void UpdateCumulativeTemperature()
    {
        cumulativeTemperature.text = Mathf.RoundToInt(minTemperature + (maxTemperature - minTemperature) * (PollutionValue / MaxPollution)).ToString();
    }

    public void AddPollution(float amountToAdd, PollutionType pollutionType)
    {
        //pollutionBars[(int)pollutionType].SetFill((pollutionValues[(int)pollutionType] + amountToAdd) / pollutionEndValues[(int)pollutionType]);
        pollutionValues[(int)pollutionType] += amountToAdd;
    }
}
