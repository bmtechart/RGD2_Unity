using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

/// <summary>
/// This behaviour staggers an enemy when it is hit by a thrown object. 
/// </summary>
/// 
[RequireComponent(typeof(HealthBehaviour))]
public class EnemyStaggerBehaviour : MonoBehaviour
{
    private float agentSpeedCache;
    private bool isStaggered = false;

    [Header("Events")]
    public UnityEvent OnStaggerBegin;
    public UnityEvent OnStaggerEnd;

    [Tooltip("Speed at which an object colliding with the player must")]
    [SerializeField] private float collisionSpeedThreshold;

    [Header("Components")]
    [SerializeField] private Animator _animator;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private HealthBehaviour healthBehaviour;

    [Header("Audio")]
    [SerializeField] private AudioArrayScriptableObject _fallAudio;
    [SerializeField] private AudioArrayScriptableObject _hitAudio;

    private void Awake()
    {
        //get component references
        if(!_animator) _animator = GetComponent<Animator>();
        if(!agent) agent = GetComponent<NavMeshAgent>();
        if (!healthBehaviour) healthBehaviour = GetComponent<HealthBehaviour>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //we only care about collisions with throwable objects
        ThrowableObject throwableObject = collision.gameObject.GetComponent<ThrowableObject>();
        if (!throwableObject) return;
        if (!throwableObject.GetComponent<Rigidbody>()) return;

        //collision speed prevents this from triggering from thrown objects that are moving slowly in the scene
        float collisionSpeed = throwableObject.GetComponent<Rigidbody>().velocity.magnitude;
        if (collisionSpeed <= collisionSpeedThreshold) return;

        AudioSource.PlayClipAtPoint(_hitAudio.GetRandomClip(), collision.transform.position);
        healthBehaviour.Damage(0.5f);
    }

    public void TriggerStaggerAnimation()
    {
        if (!_animator) return;
        _animator.SetTrigger("Stagger");
        PauseMovement();
    }

    private void ResetStaggerAnimation()
    {
        if (!_animator) return;
        _animator.ResetTrigger("Stagger");
    }

    private void PauseMovement()
    {
        if (!agent) return;

        if(!isStaggered)
        {
            agentSpeedCache = agent.speed;
            agent.speed = 0.0f;
            isStaggered = true;
        }
    }

    private void UnpauseMovement()
    {
        if (!agent) return;
        agent.speed = agentSpeedCache;
    }

    private void StaggerEnd()
    {
        OnStaggerEnd?.Invoke();
        UnpauseMovement();
        isStaggered = false;
    }

    public void PlayDeathAnimation()
    {
        if (!_animator) return;
        agent.speed = 0.0f;
        agent.isStopped = true;
        _animator.SetTrigger("Die");
    }

    private void HitGround()
    {
        AudioSource.PlayClipAtPoint(_fallAudio.GetRandomClip(), transform.position);
    }
}
