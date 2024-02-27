using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackBehaviour : AbilityBase
{
    [SerializeField] GameObject projectile;
    [SerializeField] Animator _animator;

    [SerializeField] Transform projectileSpawnTransform;
    [SerializeField] float projectileForce;
    public Transform Target;

    private void Start()
    {
        if (!_animator) _animator = GetComponent<Animator>();        
    }

    

    private void AttackTrigger()
    {
        if (!Target) return;

        GameObject newProjectile = Instantiate(projectile);
        newProjectile.transform.position = projectileSpawnTransform.position;
        newProjectile.transform.rotation = projectileSpawnTransform.rotation;

        newProjectile.GetComponent<EnemyProjectile>().Throw(Target.position, projectileForce);

    }

    public override void TriggerAbility()
    {
        if (!_animator) return;
        _animator.SetTrigger("Attack");
    }

    public void AttackComplete()
    {
        if (!_animator) return;
        _animator.ResetTrigger("Attack");
    }
}
