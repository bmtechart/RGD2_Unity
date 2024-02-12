using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject PlayerCapsule;
    [SerializeField] private GameObject MainCamera;
    [SerializeField] private GameObject PlayerFollowCamera;
    // Start is called before the first frame update
    void Start()
    {
        PlayerCapsule = Instantiate(PlayerCapsule, transform);
        MainCamera = Instantiate(MainCamera, transform);
        PlayerFollowCamera = Instantiate(PlayerFollowCamera, transform);

        CinemachineVirtualCamera virtualFollowCamera = PlayerFollowCamera.GetComponent<CinemachineVirtualCamera>();
        virtualFollowCamera.Follow = PlayerCapsule.transform.Find("PlayerCameraRoot");
        virtualFollowCamera.LookAt = PlayerCapsule.transform.Find("Player");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnDrawGizmos()
    {
        Vector3 gizmoCenter = transform.position + new Vector3(0, 1, 0);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(gizmoCenter, new Vector3(1, 2, 1));

        Gizmos.DrawLine(gizmoCenter, (transform.forward * 1.0f) + gizmoCenter);
        Gizmos.DrawSphere((transform.forward * 1.0f) + gizmoCenter, 0.1f);
        Handles.Label(gizmoCenter, "Player Spawn");
    }
}
