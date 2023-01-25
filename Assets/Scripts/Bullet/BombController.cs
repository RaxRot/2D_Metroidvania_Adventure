using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    [SerializeField] private float bombLifeTime = 3f;

    [SerializeField] private GameObject bombSimpleFx;
    [SerializeField] private GameObject bombImpactEnemyFx;

    [SerializeField] private LayerMask destructable;
    [SerializeField] private float destroyBombRadius = 0.5f;

    [SerializeField] private int damageAmount = 5;

    private void Start()
    {
        SetValues();
    }

    private void SetValues()
    {
        Instantiate(bombSimpleFx, transform.position, Quaternion.identity);
        BombLife();
    }
    
    private void BombLife()
    {
        StartCoroutine(nameof(_BombLifeCo));
    }

    private IEnumerator _BombLifeCo()
    {
        yield return new WaitForSeconds(bombLifeTime);
        Collider2D[] objectsToRemove = Physics2D.OverlapCircleAll(transform.position, destroyBombRadius, destructable);
            
        if (objectsToRemove.Length>0)
        {
            foreach (var destroBlocks in objectsToRemove)
            {
                destroBlocks.gameObject.GetComponent<DestructionBlock>().MakeDestruction();
            }
            
            MakeExplosion();
        }
        else
        {
            Instantiate(bombSimpleFx, transform.position, Quaternion.identity);
            AudioManager.Instance.PlaySfxAdjusted(4);
            Destroy(gameObject);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag(TagManager.ENEMY_TAG))
        {
            MakeExplosion();
            col.GetComponent<EnemyHealthController>().DamageEnemy(damageAmount);
        }
    }

    private void MakeExplosion()
    {
        Instantiate(bombImpactEnemyFx, transform.position, Quaternion.identity);
        AudioManager.Instance.PlaySfxAdjusted(4);
        Destroy(gameObject);
    }
}
