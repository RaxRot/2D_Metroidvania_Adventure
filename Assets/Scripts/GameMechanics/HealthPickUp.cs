using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
    private Animator _anim;
    
    [SerializeField] private int healAmount = 1;
    
    [SerializeField] private GameObject healPickUpFx;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag(TagManager.PLAYER_TAG))
        {
            AudioManager.Instance.PlaySFX(5);
            
            if (PlayerHealth.Instance.IsMaxHealth())
            {
                _anim.SetTrigger(TagManager.PICK_UP_MAX_HEALTH_TRIGGER);
            }
            else
            {
                PlayerHealth.Instance.HealPlayer(healAmount);
                Instantiate(healPickUpFx, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }
}
