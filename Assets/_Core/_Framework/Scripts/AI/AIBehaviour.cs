using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AIBehaviour : MonoBehaviour
{
    [SerializeField]
    protected string BehaviourName;

    protected Animator _animator;
    public Animator Animator 
    {
        get { return _animator; }
        set { _animator = value; }
    }
    private void Awake()
    {
        RegisterBehaviour();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    private void RegisterBehaviour()
    {
        AIController controller = GetComponentInParent<AIController>();
        if (!controller)
        {
            Debug.Log("No AI Controller present on prefab. Cannot register behvaiour");
            return;
        }

        controller.RegisterBehaviour(BehaviourName, this);
    }
}
