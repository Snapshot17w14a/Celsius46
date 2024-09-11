using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    //public delegate void OnEnemyDies(GameObject enemyObject);
    public delegate void OnPlayerHitShot(RaycastHit hit);

    //public static event OnEnemyDies onEnemyDies;
    public static event OnPlayerHitShot onPlayerHitShot;

    /*public static void RaiseOnEnemyDies(GameObject enemyObject)
    {
        if (onEnemyDies != null)
        {
            onEnemyDies(enemyObject);
        }
    }*/

    public static void RaiseOnPlayerHitShot(RaycastHit hit)
    {
        if (onPlayerHitShot != null)
        {
            onPlayerHitShot(hit);
        }
    }
}
