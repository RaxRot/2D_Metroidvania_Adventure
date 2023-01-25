using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityTracker : MonoBehaviour
{
    public bool canDoubleJump, canDash, canBecomeBall, canDropBomb, canShoot;

    public void UnlockAllAbilities()
    {
        canDoubleJump = canDash = canBecomeBall = canDropBomb = canShoot = true;
    }
}
