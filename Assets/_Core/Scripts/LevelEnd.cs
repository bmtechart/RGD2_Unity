using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;

[RequireComponent(typeof(BoxCollider))]
public class LevelEnd : MonoBehaviour
{
    public GameObject levelEndUI;
    private void OnTriggerEnter(Collider other)
    {
        bool isPlayer = other.tag == "Player";
        if (!isPlayer) return;
        //if(levelEndUI) UIManager.Instance.AddWidgetToViewport(levelEndUI);
        Cursor.visible = true;
        
        GameManager.Instance.LevelEnd();
    }
}
