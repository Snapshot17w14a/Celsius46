using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnd : MonoBehaviour
{
    [SerializeField] private GameObject BuildingsParent;
    [SerializeField] private GameObject NatureParent;

    public void EndGame()
    {
        for(int i = 0; i < BuildingsParent.transform.childCount; i++)
        {
            Destroy(BuildingsParent.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < NatureParent.transform.childCount; i++)
        {
            Destroy(NatureParent.transform.GetChild(i).gameObject);
        }
    }
}
