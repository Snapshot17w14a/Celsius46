using UnityEngine;

public class GameEnd : MonoBehaviour
{
    [SerializeField] private GameObject BuildingsParent;
    [SerializeField] private GameObject NatureParent;

    public void EndGame()
    {
        for(int i = 0; i < BuildingsParent.transform.childCount; i++)
        {
            DestroyImmediate(BuildingsParent.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < NatureParent.transform.childCount; i++)
        {
            DestroyImmediate(NatureParent.transform.GetChild(i).gameObject);
        }
        DestroyImmediate(BuildingsParent);
        DestroyImmediate(NatureParent);
        PopulationSimulator.Instance.StopSimulation();
    }
}
