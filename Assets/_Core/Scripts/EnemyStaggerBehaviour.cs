using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This behaviour staggers an enemy when it is hit by a thrown object. 
/// </summary>
public class EnemyStaggerBehaviour : MonoBehaviour
{
    public UnityEvent OnStaggerBegin;
    public UnityEvent OnStaggerEnd;

    [SerializeField] private Animator _animator;
    private void OnCollisionEnter(Collision collision)
    {
        ThrowableObject throwableObject = collision.gameObject.GetComponent<ThrowableObject>();
        if (!throwableObject) return;
        
        //damageable

        if (!_animator) return;

        _animator.SetTrigger("Stagger");
        _animator.SetLayerWeight(1, 0.0f);
        //stop nav agent movement
        //pause behaviour tree
    }
}
