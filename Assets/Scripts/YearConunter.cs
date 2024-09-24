using System.Collections;
using UnityEngine;

public class YearConunter : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI yearText;
    [SerializeField] private float seconsPerYear = 1f;

    private int year = 2024;

    public int GetYear => year;

    // Start is called before the first frame update
    void Start()
    {
        yearText.text = "year - " + year.ToString();
        StartCoroutine(YearCounter());
    }

    private IEnumerator YearCounter()
    {
        while (true)
        {
            yield return new WaitForSeconds(seconsPerYear);
            year++;
            yearText.text = "year - " + year.ToString();
        }
    }
}
