using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    [SerializeField] private int damageAmount = 1;

    [SerializeField] private bool shouldDestroyOnDamage;
    [SerializeField] private bool isEnemyBullet;
    [SerializeField] private bool deadZone;

    [SerializeField] private GameObject enemyDestroyFX;

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag(TagManager.PLAYER_TAG))
        {
            DealDamage();
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag(TagManager.PLAYER_TAG))
        {
            DealDamage();
        }
    }

    private void DealDamage()
    {
        PlayerHealth.Instance.DamagePlayer(damageAmount);

        if (deadZone)
        {
            
        }
        if (shouldDestroyOnDamage)
        {
            Instantiate(enemyDestroyFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        if (isEnemyBullet)
        {
            Destroy(gameObject);
        }
    }
}
