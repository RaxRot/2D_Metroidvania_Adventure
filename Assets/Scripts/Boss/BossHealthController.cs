using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthController : MonoBehaviour
{
    public static BossHealthController Instance;

    [SerializeField] private Slider bossHealthSlider;
    [SerializeField] private int currentHealth = 50;

    public int CurrentHealth
    {
        get => currentHealth;
        set => currentHealth = value;
    }

    [SerializeField] private BossBatle theBoss;

    private void Awake()
    {
        if (Instance==null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        bossHealthSlider.maxValue = currentHealth;
        bossHealthSlider.value = currentHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth<=0)
        {
            currentHealth = 0;
            
            theBoss.EndBattle();
            
            AudioManager.Instance.PlaySFX(0);
        }
        else
        {
            AudioManager.Instance.PlaySFX(1);
        }

        bossHealthSlider.value = currentHealth;
    }
}
