using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
   private PlayerController _player;

   private Vector3 _tempCameraPosition;

   [SerializeField] private float yUpOffset = 5f;
   [SerializeField] private float minYCameraFollowPosition = -5f, maxYCameraFollowPosition = 3f;
   

   private void Start()
   {
      _player = FindObjectOfType<PlayerController>();
      AudioManager.Instance.PlayLevelMusic();
   }

   private void LateUpdate()
   {
      if (!_player)
      {
         _player = FindObjectOfType<PlayerController>();
      }

      if (_player)
      {
         FollowPlayer();
      }
   }

   private void FollowPlayer()
   {
      _tempCameraPosition = transform.position;
      
      _tempCameraPosition.x = _player.transform.position.x;
      _tempCameraPosition.y=Mathf.Clamp(_player.transform.position.y+yUpOffset,minYCameraFollowPosition,maxYCameraFollowPosition);
      
      transform.position = _tempCameraPosition;
   }
}
