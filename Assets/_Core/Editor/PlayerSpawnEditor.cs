using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerSpawner))]
public class PlayerSpawnEditor : Editor
{
    //shows when component is selected
    private void OnSceneGUI()
    {
        PlayerSpawner playerSpawner = (PlayerSpawner)target;
        Handles.color = Color.red;
        Handles.DrawWireDisc(playerSpawner.transform.position, playerSpawner.transform.up, 1f);
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        PlayerSpawner playerSpawner = (PlayerSpawner)target;
        Handles.color = Color.red;
        Handles.DrawWireDisc(playerSpawner.transform.position, playerSpawner.transform.up, 1.5f);
        Handles.Label(playerSpawner.transform.position, "PlayerSpawn");
    }
}
