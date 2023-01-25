using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AbilityUnlock : MonoBehaviour
{
   [SerializeField] private bool unlockDoubleJump, unlockDah, unlockBecomeBall, unlockDropBomb,unlockCanShoot;
   [SerializeField] private GameObject abilityPickupFX;

   [SerializeField] private string unlockMessage;
   [SerializeField] private TMP_Text unlockText;
   
   private void OnTriggerEnter2D(Collider2D col)
   {
      if (col.CompareTag(TagManager.PLAYER_TAG))
      {
         PlayerAbilityTracker playerAbilityTracker = col.GetComponentInParent<PlayerAbilityTracker>();
         
         if (unlockDoubleJump)
         {
            playerAbilityTracker.canDoubleJump = true;
         }
         if (unlockDah)
         {
            playerAbilityTracker.canDash = true;
         }
         if (unlockBecomeBall)
         {
            playerAbilityTracker.canBecomeBall = true;
         }
         if (unlockDropBomb)
         {
            playerAbilityTracker.canDropBomb = true;
         }

         if (unlockCanShoot)
         {
            playerAbilityTracker.canShoot = true;
         }
         
         Instantiate(abilityPickupFX, transform.position, Quaternion.identity);
         
         AudioManager.Instance.PlaySFX(5);
         
         ShowTextMessage();
         
         Destroy(gameObject);
      }
   }

   private void ShowTextMessage()
   {
      unlockText.transform.parent.SetParent(null);
      unlockText.transform.parent.position = transform.position;
      unlockText.text = unlockMessage;
      unlockText.gameObject.SetActive(true);
      Destroy(unlockText.transform.parent.gameObject,3f);
   }
}
