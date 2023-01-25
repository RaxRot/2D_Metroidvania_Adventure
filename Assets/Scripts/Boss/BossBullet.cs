using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    private Rigidbody2D _rb;
    
    [SerializeField] private float moveSpeed =7 ;

    [SerializeField] private int damageAmount = 1;
    [SerializeField] private GameObject impactFx ;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    
    private void Start()
    {
       SetValues();
    }
    
    private void SetValues()
    {
        Vector3 direction = transform.position -PlayerHealth.Instance.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x)*Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle,Vector3.forward);
        
        AudioManager.Instance.PlaySfxAdjusted(2);
    }

    private void Update()
    {
        _rb.velocity = -transform.right * moveSpeed;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag(TagManager.PLAYER_TAG))
        {
            PlayerHealth.Instance.DamagePlayer(damageAmount);
        }
        
        DestroyBullet();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag(TagManager.PLAYER_BULLET_TAG))
        {
            DestroyBullet();
        }
    }

    private void DestroyBullet()
    {
        Instantiate(impactFx, transform.position, Quaternion.identity);
        AudioManager.Instance.PlaySfxAdjusted(3);
        Destroy(gameObject);
    }
}
