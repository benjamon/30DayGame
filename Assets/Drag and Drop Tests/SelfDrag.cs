using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDrag : MonoBehaviour
{
    static Camera Cam
    {
        get
        {
            if (!_camera) _camera = Camera.main;
            return _camera;
        }
    }
    static Camera _camera;
    bool isDrag = false;
    Vector2 offset;

    private void OnMouseDown()
    {
        isDrag = true;
        offset = (Vector2)transform.position-(Vector2)Cam.ScreenToWorldPoint(Input.mousePosition);
        Debug.Log("grabbed");
    }

    private void OnMouseUp()
    {
        isDrag = false;
        Debug.Log("released");
    }

    private void LateUpdate()
    {
        if (isDrag)
            transform.position = (Vector2)Cam.ScreenToWorldPoint(Input.mousePosition) + offset;
    }
}
