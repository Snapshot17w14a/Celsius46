using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{

    [SerializeField]
    private Transform rayStartPoint;
    [SerializeField]
    private float maxDistance = Mathf.Infinity;
    [SerializeField]
    private LayerMask layerMask;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            RaycastHit hit;
            if (Physics.Raycast(rayStartPoint.position, rayStartPoint.forward, out hit, maxDistance, layerMask))
            {
                Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube), hit.point, Quaternion.identity);
                GameEvents.RaiseOnPlayerHitShot(hit);
            }
        }
    }
}
