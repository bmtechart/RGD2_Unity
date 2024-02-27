using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityBase : MonoBehaviour
{
    public delegate void AbilityComplete();
    public virtual event AbilityComplete abilityComplete;

    protected virtual void OnAbilityComplete()
    {
        abilityComplete?.Invoke();
    }

    [SerializeField] protected LayerMask targetLayers;

    public abstract void TriggerAbility();
}
