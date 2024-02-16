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

    [SerializeField] private AudioArrayScriptableObject _punchClipArray;

    [SerializeField] private GameObject _leftHand;
    [SerializeField] private GameObject _rightHand;
    /// <summary>
    /// Gets all colliders in a radius around the attack and damages their parent game objects if they are damageable
    /// </summary>
    public override void TriggerAbility()
    {
        //trigger animator
        CheckHit();
    }

    public void CheckHit()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, AttackRadius, targetLayers);
        foreach (Collider collider in colliders)
        {
            if (!collider.enabled) continue;
            IDamageable damageable = collider.gameObject.GetComponent<IDamageable>();
            damageable?.Damage(DamageAmount);


            
            if (damageable != null) 
            {
                Debug.Log("Hit Player!");
                AudioSource.PlayClipAtPoint(_punchClipArray.GetRandomClip(), transform.position);
            };
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, AttackRadius);
    }
}
