using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthController : MonoBehaviour
{
    [SerializeField] private int totalHealth = 3;
    [SerializeField] private GameObject deadFX;

    public void DamageEnemy(int damageAmount)
    {
        totalHealth -= damageAmount;
        
        if (totalHealth<=0)
        {
            totalHealth = 0;
            
            Instantiate(deadFX, transform.position, Quaternion.identity);
            
            AudioManager.Instance.PlaySFX(4);
            
            Destroy(gameObject);
        }
    }
}
