using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnManager : MonoBehaviour
{
   public static RespawnManager Instance;

   private Vector3 _respawnPosition;
   [SerializeField] private float waitToRespawn=2f;

   private GameObject _thePlayer;

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
      _thePlayer = PlayerHealth.Instance.gameObject;
      _respawnPosition = _thePlayer.transform.position;
   }

   public void RespawnPlayer()
   {
      StartCoroutine(nameof(_RespawnPlayerCo));
   }

   private IEnumerator _RespawnPlayerCo()
   {
      _thePlayer.SetActive(false); 
      
      UIManager.Instance.StartFadeToBlack();
      yield return new WaitForSeconds(waitToRespawn);

      SceneManager.LoadScene(SceneManager.GetActiveScene().name);

      _thePlayer.transform.position = _respawnPosition;
      _thePlayer.SetActive(true);
      PlayerHealth.Instance.FillHealth();
      UIManager.Instance.StartFadeFromBlack();
   }

   public void SetSpawnPoint(Vector3 newPosition)
   {
      _respawnPosition = newPosition;
   }
}
