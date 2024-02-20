using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class PunchBehaviour : AbilityBase
{

    public float AttackRadius = 0.2f;
    public float DamageAmount = .2f;

    [SerializeField]
    private Animator _animator;

    [Header("Audio")]
    [SerializeField] private AudioArrayScriptableObject _punchClipArray;
    [SerializeField] private AudioArrayScriptableObject _whooshClipArray;

    //not yet implemented, but the overlap sphere will be drawn around
    //different hands depending on the animation playing
    [Tooltip("Gameobjects on the animated component for the left and right hand.")]
    [SerializeField] private GameObject _leftHand;

    [Tooltip("Gameobjects on the animated component for the left and right hand.")]
    [SerializeField] private GameObject _rightHand;
    /// <summary>
    /// Gets all colliders in a radius around the attack and damages their parent game objects if they are damageable
    /// </summary>
    public override void TriggerAbility()
    {

        if (_animator) _animator.SetTrigger("Attack");
 
    }

    public void CheckHit()
    {
        Collider[] colliders = Physics.OverlapSphere(_rightHand.transform.position, AttackRadius, targetLayers);
        foreach (Collider collider in colliders)
        {
            if (!collider.enabled) continue;
            IDamageable damageable = collider.gameObject.GetComponent<IDamageable>();
            damageable?.Damage(DamageAmount);

            if (damageable != null) 
            {
                AudioSource.PlayClipAtPoint(_punchClipArray.GetRandomClip(), _rightHand.transform.position);
            };
        }
    }

    private void AttackHit()
    {
        CheckHit();
    }

    private void AttackComplete()
    {
        if (_animator)
        {
            _animator.ResetTrigger("Attack");
            OnAbilityComplete();
        }
    }

    private void PlayWhoosh()
    {
        AudioSource.PlayClipAtPoint(_whooshClipArray.GetRandomClip(), _rightHand.transform.position);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_rightHand.transform.position, AttackRadius);
    }
}
