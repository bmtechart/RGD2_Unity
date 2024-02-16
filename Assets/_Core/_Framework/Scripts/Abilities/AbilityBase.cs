using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityBase : MonoBehaviour
{
    [SerializeField] protected LayerMask targetLayers;

    public abstract void TriggerAbility();
}
