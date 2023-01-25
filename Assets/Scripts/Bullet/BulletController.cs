using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Rigidbody2D _rb;

    [SerializeField] private float bulletSpeed = 10f;

    [HideInInspector]public Vector2 moveDirection;

    [SerializeField] private GameObject shootImpactFx;

    [SerializeField] private int damageAmount = 1;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
       MoveBullet();
    }

    private void MoveBullet()
    {
        _rb.velocity = moveDirection * bulletSpeed;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag(TagManager.ENEMY_TAG))
        {
            col.GetComponent<EnemyHealthController>().DamageEnemy(damageAmount);
        }

        if (col.CompareTag(TagManager.BOSS_TAG))
        {
            BossHealthController.Instance.TakeDamage(damageAmount);
        }
        
        Instantiate(shootImpactFx, transform.position, Quaternion.identity);
        AudioManager.Instance.PlaySfxAdjusted(3);
        Destroy(gameObject);
    }
}
