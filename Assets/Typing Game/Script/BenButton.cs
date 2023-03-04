using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BenButton : MonoBehaviour
{
    public AudioSource source;
    public Image panel;
    RectTransform canvasRect;
    Camera cam;

    private void Awake()
    {
        cam = Camera.main;
        canvasRect = panel.canvas.GetComponent<RectTransform>();
        panel.raycastTarget = false;
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            foreach (var touch in Input.touches)
                ProcessTouch(touch);
        }
        if (Application.platform != RuntimePlatform.Android)
        {
            Debug.Log("mouse position inside?: " + GetScreenPointInside(Input.mousePosition));
            if (Input.GetMouseButtonDown(0))
            {
                if (GetScreenPointInside(Input.mousePosition))
                    Fire();
            }
        }
    }

    private void ProcessTouch(Touch touch)
    {
        if (touch.phase == TouchPhase.Began)
        {
            if (GetScreenPointInside(touch.position))
                Fire();
        }
    }

    private bool GetScreenPointInside(Vector2 point)
    {
        var pt = cam.ScreenToViewportPoint(point) * canvasRect.rect.size;
        if (pt.x > panel.rectTransform.anchoredPosition.x && 
            pt.x < panel.rectTransform.anchoredPosition.x + panel.rectTransform.rect.width && 
            pt.y > panel.rectTransform.anchoredPosition.y &&
            pt.y < panel.rectTransform.anchoredPosition.y + panel.rectTransform.rect.height)
        {
            Debug.Log("IS INSIDE");
            return true;
        }
        return false;
    }

    void Fire()
    {
        Debug.Log("FIRE!");
        source.Stop();
        source.Play();
    }
}
