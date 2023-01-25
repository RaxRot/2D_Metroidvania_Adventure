using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyShooter : MonoBehaviour
{
    [SerializeField] private GameObject bulletToShoot;
    [SerializeField] private Transform pointToShoot;

    [SerializeField] private float minTimeToShoot = 1f, maxTimeToShoot = 2f;
    private float _timeToShoot;

    [SerializeField] private bool horizontalShoot;
    private GameObject bullet;
    

    private void Start()
    {
        Shoot();
    }

    private void Shoot()
    {
        ShootVertical();
    }

    private void ShootVertical()
    {
        StartCoroutine(nameof(_ShootVerticalCo));
    }

    private IEnumerator _ShootVerticalCo()
    {
        _timeToShoot = Random.Range(minTimeToShoot, maxTimeToShoot);
        
        yield return new WaitForSeconds(_timeToShoot);
        
        if (horizontalShoot)
        {
            bullet = Instantiate(bulletToShoot, pointToShoot.position, bulletToShoot.transform.rotation);
            bullet.GetComponent<BulletController>().moveDirection =  new Vector2(transform.localScale.x,0f);;
            bullet.transform.localScale = transform.localScale;
        }
        else
        {
            Instantiate(bulletToShoot, pointToShoot.position, bulletToShoot.transform.rotation);
        }
        
        StartCoroutine(nameof(_ShootVerticalCo));
    }
}
