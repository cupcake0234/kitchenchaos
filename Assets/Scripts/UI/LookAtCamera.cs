using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Mode
{
    LookAt,
    LookAtInverted,
    CameraForward,
    CameraForwardInverted
}

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] private Mode mode;
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        switch (mode)
        {
            case Mode.LookAt:
                transform.LookAt(mainCamera.transform);
                break;
            case Mode.LookAtInverted:
                Vector3 dirFromCamera = transform.position - mainCamera.transform.position;
                transform.LookAt(transform.position + dirFromCamera);
                break;
            case Mode.CameraForward:
                transform.forward = mainCamera.transform.forward;
                break;
            case Mode.CameraForwardInverted:
                transform.forward = -mainCamera.transform.forward;
                break;
            default:
                break;
        }
    }
}
