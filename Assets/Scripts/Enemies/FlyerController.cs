using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyerController : MonoBehaviour
{
    [SerializeField] private float rangeToStartChase=10;
    private bool _isChasing;
    [SerializeField] private float moveSpeed=5;
    [SerializeField] private float turnSpeed=5;

    private Transform _player;

    [SerializeField] private Animator anim;

    private void Start()
    {
        _player = GameObject.FindWithTag(TagManager.PLAYER_TAG).transform;
        
        if (!_player)
        {
            print("Not find");
        }
        else
        {
            print("find");
        }
    }

    private void Update()
    {
        if (!_isChasing)
        {
            if (_player)
            {
                if (Vector3.Distance(transform.position,_player.position)<=rangeToStartChase)
                {
                    _isChasing = true;
                    anim.SetBool(TagManager.ENEMY_CHASING_ANIM_PARAMETR,_isChasing);
                }
            }
            else
            {
               _player = GameObject.FindWithTag(TagManager.PLAYER_TAG).transform;
                print("find player time to check distanse");
            }
        }
        else
        {
           Chasing();
        }
    }

    private void Chasing()
    {
        RotateToPlayer();
        
        MoveToPlayer();
    }

    private void RotateToPlayer()
    {
        Vector3 direction = transform.position - _player.position;
        float angle = Mathf.Atan2(direction.y, direction.x)*Mathf.Rad2Deg;
        Quaternion targetRot=Quaternion.AngleAxis(angle,Vector3.forward);
                
        transform.rotation=Quaternion.Slerp(transform.rotation,targetRot,turnSpeed*Time.deltaTime);
    }

    private void MoveToPlayer()
    {
        transform.position =
        Vector3.MoveTowards(transform.position, _player.position, moveSpeed * Time.deltaTime);

        //transform.position += -transform.right * moveSpeed * Time.deltaTime;
        //2 types of chasing
    }
}
