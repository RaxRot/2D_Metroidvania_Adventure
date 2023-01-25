using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabPatroller : MonoBehaviour
{
    private Rigidbody2D _rb;
    [SerializeField] private Transform[] patrolPoints;
    private int _currentPatrolPoint;
    [SerializeField] private float distanceToSwitchPatrol =0.5f;
    [SerializeField] private Transform enemyPatrolPointsParent;
    

    [SerializeField] private float moveSpeed =4f, waitAtPoints=1.5f;
    private float _waitCounter;

    [SerializeField] private float jumpForce=8f;
    [SerializeField] private Animator anim;
    
    [SerializeField]private float yDistanceToJump=0.5f;
    [SerializeField] private float minYVelocityToJump = 0.1f;

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
        foreach (var point in patrolPoints)
        {
            point.SetParent(enemyPatrolPointsParent);
        }
    }

    private void Update()
    {
        Movement();
    }

    private void Movement()
    {
        if (Vector3.Distance(transform.position,patrolPoints[_currentPatrolPoint].position)<distanceToSwitchPatrol)
        {
            _waitCounter = waitAtPoints;
            _currentPatrolPoint++;
            if (_currentPatrolPoint>=patrolPoints.Length)
            {
                _currentPatrolPoint = 0;
            }
        }
        else
        {
            _waitCounter -= Time.deltaTime;
        }

        if (_waitCounter<=0)
        {
            if (transform.position.x<patrolPoints[_currentPatrolPoint].position.x)
            {
                _rb.velocity = new Vector2(moveSpeed, _rb.velocity.y);
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            else
            {
                _rb.velocity = new Vector2(-moveSpeed, _rb.velocity.y);
                transform.localScale = Vector3.one;
            }

            if (transform.position.y<patrolPoints[_currentPatrolPoint].position.y-yDistanceToJump && _rb.velocity.y<minYVelocityToJump)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
            }
        }
        
        anim.SetFloat(TagManager.ENEMY_MOVE_SPEED_ANIM_PARAMETR,Math.Abs(_rb.velocity.x));
    }
}
