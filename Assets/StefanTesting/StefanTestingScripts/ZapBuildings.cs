using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZapBuildings : MonoBehaviour
{
    [SerializeField]
    private LayerMask layerMask;
    Ray ray;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                Debug.Log(hit.transform.name);
                Debug.Log("hit");
                //hit.collider.gameObject.SetActive(false);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(ray);
    }
}
