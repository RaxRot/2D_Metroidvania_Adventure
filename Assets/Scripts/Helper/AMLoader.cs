using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMLoader : MonoBehaviour
{
    [SerializeField] private AudioManager theAm;

    private void Awake()
    {
        if (AudioManager.Instance==null)
        {
          AudioManager newAm = Instantiate(theAm);
          AudioManager.Instance = newAm;
          DontDestroyOnLoad(newAm.gameObject);
        }
    }
}
