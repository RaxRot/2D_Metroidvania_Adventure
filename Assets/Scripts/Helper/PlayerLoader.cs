using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoader : MonoBehaviour
{
    [SerializeField] private PlayerHealth thePlayer;
    [SerializeField] private Transform positionToSpawn;

    private void Awake()
    {
        if (PlayerHealth.Instance==null)
        {
            PlayerHealth newHealth = Instantiate(thePlayer, positionToSpawn.position, Quaternion.identity);
            
            PlayerHealth.Instance = newHealth;

            FindObjectOfType<PlayerAbilityTracker>().UnlockAllAbilities();
            
            DontDestroyOnLoad(newHealth.gameObject);
        }
    }
}
