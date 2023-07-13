using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpaceUI : MonoBehaviour
{
    private Camera mainCamera;

    protected virtual void Awake()
    {
        mainCamera = Camera.main;
    }

    // 调整UI的朝向
    private void LateUpdate()
    {
        transform.forward = mainCamera.transform.forward;
    }
}
