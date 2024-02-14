using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AIBehaviour : MonoBehaviour
{
    public string Name;

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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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

        controller.RegisterBehaviour(Name, this);
    }
}
