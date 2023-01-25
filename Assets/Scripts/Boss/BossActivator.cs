using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossActivator : MonoBehaviour
{
    [SerializeField] private GameObject bossToActivate;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag(TagManager.PLAYER_TAG))
        {
            bossToActivate.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
