using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class KillZ : MonoBehaviour
{
    [SerializeField] private bool _drawDebug;
    private BoxCollider m_BoxCollider;
    // Start is called before the first frame update
    void Start()
    {
        m_BoxCollider = GetComponent<BoxCollider>();
        if(!m_BoxCollider)
        {
            Debug.Log("Warning! KillZ volume requires box collider component!");
            return;
        }
        if(!m_BoxCollider.isTrigger) m_BoxCollider.isTrigger = false;

    }



    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.GetComponent<CharacterController>())
        {
            GameManager.Instance.GameOver();
        }
        //otherwise destroy any game object that falls out of bounds
        Destroy(other.gameObject);

    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_drawDebug) DrawDebug();
    }

    private void DrawDebug()
    {
        Gizmos.color = Color.red;
        m_BoxCollider = GetComponent<BoxCollider>();
        Gizmos.DrawCube(transform.position, m_BoxCollider.size);
        Handles.color = Color.yellow;
        Handles.Label(transform.position, "Kill Z Volume");
    }
#endif
}
