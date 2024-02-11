using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : Singleton<CameraManager>
{
    public List<CinemachineVirtualCamera> cameras;

    public CinemachineVirtualCamera ActiveCamera;

    public bool IsActiveCamera(CinemachineVirtualCamera camera)
    {
        if (camera == ActiveCamera) return true;
        return false;
    }

    /// <summary>
    /// Appends the input camera to the list of available cameras. 
    /// </summary>
    /// <param name="camera"></param>
    public void RegisterCamera(CinemachineVirtualCamera camera)
    {
        if (cameras.Contains(camera)) return;
        cameras.Add(camera);
    }

    public void UnregisterCamera(CinemachineVirtualCamera camera)
    {
        if(cameras.Contains(camera)) cameras.Remove(camera);
    }

    /// <summary>
    /// Sets the input camera to be the active camera. 
    /// </summary>
    /// <param name="camera"></param>
    public void SwitchCamera(CinemachineVirtualCamera camera)
    {
        camera.Priority = 10;
        ActiveCamera = camera;

        foreach (CinemachineVirtualCamera cam in cameras)
        {
            if(cam != ActiveCamera) cam.Priority = 0;
        }
    }
}
