using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class BasicEnemyAnimationController : MonoBehaviour
{
    public UnityEvent OnAttackComplete;
    public UnityEvent OnAttackHit;
    private Animator _animator;

    [SerializeField] private AudioArrayScriptableObject _whooshArray;

    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void AttackComplete()
    {
        OnAttackComplete?.Invoke();
    }

    private void AttackHit()
    {
        OnAttackHit?.Invoke();
    }

    private void PlayWhoosh()
    {
        AudioSource.PlayClipAtPoint(_whooshArray.GetRandomClip(), transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void StaggerEnd()
    {
        if (!_animator) return;
        _animator.ResetTrigger("Stagger");

        //unpause behaviour tree
        //enable nav mesh agent
    }
}
