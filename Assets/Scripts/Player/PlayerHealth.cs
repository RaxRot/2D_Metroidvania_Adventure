using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance;
    
    [SerializeField] private int maxHealth = 10;
     private int _currentHealth;

     public int CurrentHealth
     {
         get => _currentHealth;
         set => _currentHealth = value;
     }

     [SerializeField] private float invincibilityLength;
    private float _invincibilityCounter;

    [SerializeField] private float flashLength;
    private float _flashCounter;

    [SerializeField] private SpriteRenderer[] playerSprites;

    [SerializeField] private GameObject playerDeadFx;

    private void Awake()
    {
        if (Instance==null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
       SetValues();
    }

    private void SetValues()
    {
        _currentHealth = maxHealth;
        UIManager.Instance.UpdatePlayerHealth(_currentHealth,maxHealth);
    }

    private void Update()
    {
        if (_invincibilityCounter>0)
        {
            _invincibilityCounter -= Time.deltaTime;
            _flashCounter -= Time.deltaTime;
            if (_flashCounter<=0)
            {
                foreach (var sprites in playerSprites)
                {
                    sprites.enabled = !sprites.enabled;
                }

                _flashCounter = flashLength;
            }

            if (_invincibilityCounter<=0)
            {
                foreach (var sprites in playerSprites)
                {
                    sprites.enabled = true;
                }
                
                _flashCounter = 0;
            }
        }
    }

    public void DamagePlayer(int damageAmount)
    {
        if (_invincibilityCounter<=0)
        {
            _currentHealth -= damageAmount;
        
            if (_currentHealth<=0)
            {
                _currentHealth = 0;
                //GM
                AudioManager.Instance.PlaySFX(8);
                Instantiate(playerDeadFx, transform.position, Quaternion.identity);
               RespawnManager.Instance.RespawnPlayer();
            }
            else
            {
                _invincibilityCounter = invincibilityLength;
                
                AudioManager.Instance.PlaySFX(11);
            }
        
            UIManager.Instance.UpdatePlayerHealth(_currentHealth,maxHealth);
        }
    }

    public void FillHealth()
    {
        _currentHealth = maxHealth;
        UIManager.Instance.UpdatePlayerHealth(_currentHealth,maxHealth);
    }

    public void HealPlayer(int healAmount)
    {
        _currentHealth += healAmount;

        if (_currentHealth>maxHealth)
        {
            _currentHealth = maxHealth;
        }
        
        UIManager.Instance.UpdatePlayerHealth(_currentHealth,maxHealth);
    }

    public bool IsMaxHealth()
    {
        return _currentHealth == maxHealth;
    }
}
