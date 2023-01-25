using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeTimer : MonoBehaviour
{
   [SerializeField] private float lifeTime;

   private void Start()
   {
      Destroy(gameObject,lifeTime);
   }

   private void OnBecameInvisible()
   {
      Destroy(gameObject);
   }
}
