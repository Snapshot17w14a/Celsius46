using System.Collections;
using UnityEngine;

public class YearCounter : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI yearText;
    [SerializeField] private float seconsPerYear = 1f;

    private int year = 2024;

    public int GetYear => year;

    // Start is called before the first frame update
    void Start()
    {
        yearText.text = "Year - " + year.ToString() + " A.D.";
        StartCoroutine(YearCounterCoroutine());
    }

    private IEnumerator YearCounterCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(seconsPerYear);
            year++;
            yearText.text = "Year - " + year.ToString() + " A.D.";
        }
    }
}
