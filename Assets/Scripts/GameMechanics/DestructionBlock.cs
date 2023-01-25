using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class DestructionBlock : MonoBehaviour
{
    [SerializeField] private GameObject destructionFx;
    
    public void MakeDestruction()
    {
        Instantiate(destructionFx, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
