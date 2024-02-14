using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class BasicEnemyAnimationController : MonoBehaviour
{
    public UnityEvent OnAttackComplete;
    public UnityEvent OnAttackHit;
    private Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void AttackComplete()
    {
        OnAttackComplete?.Invoke();
    }

    private void AttackHit()
    {
        OnAttackHit?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
