using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;

[RequireComponent(typeof(BoxCollider))]
public class LevelEnd : MonoBehaviour
{
    public GameObject levelEndUI;

    private void Start()
    {
        if(levelEndUI) Instantiate(levelEndUI);
    }

    private void OnTriggerEnter(Collider other)
    {
        bool isPlayer = other.tag == "Player";
        if (!isPlayer) return;

        WidgetManager.Instance.OpenWidget("LevelEndScreen");

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        GameManager.Instance.LevelComplete();
    }
}
