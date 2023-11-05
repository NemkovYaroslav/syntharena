using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWidgetLookAtCamera : MonoBehaviour
{
    private Camera _camera;
    private bool _isCameraNotNull;

    private void Start()
    {
        _camera = Camera.main;
        _isCameraNotNull = _camera != null;
    }

    void LateUpdate()
    {
        if (_isCameraNotNull)
        {
            transform.LookAt(transform.position + _camera.transform.forward);
        }
    }
}