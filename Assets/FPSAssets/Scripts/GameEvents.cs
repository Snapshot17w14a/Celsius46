using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    //comments are example

    //public delegate void OnEnemyDies(GameObject enemyObject);
    public delegate void OnPlayerHitShot(RaycastHit hit);
    public delegate void OnPlayerShoot();
    public delegate void OnPlayerPickUpItem(PickUpItem item);
    public delegate void OnPlayerDies();

    //public static event OnEnemyDies onEnemyDies;
    public static event OnPlayerHitShot onPlayerHitShot;
    public static event OnPlayerShoot onPlayerShoot;
    public static event OnPlayerPickUpItem onPlayerPickUpItem;
    public static event OnPlayerDies onPlayerDies;

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

    public static void RaiseOnPlayerShoot()
    {
        if (onPlayerShoot != null)
        {
            onPlayerShoot();
        }
    }

    public static void RaiseOnPlayerPickUpItem(PickUpItem item)
    {
        if (onPlayerPickUpItem != null)
        {
            onPlayerPickUpItem(item);
        }
    }

    public static void RaiseOnPlayerDies()
    {
        if (onPlayerDies != null)
        {
            onPlayerDies();
        }
    }
}
