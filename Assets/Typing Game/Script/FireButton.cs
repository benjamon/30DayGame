using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireButton : MonoBehaviour
{
    public TypingGame game;
    Collider2D _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (Input.touchCount > 0)
            foreach (var touch in Input.touches)
                ProcessTouch(touch);
    }

    private void ProcessTouch(Touch touch)
    {
        if (touch.phase != TouchPhase.Began)
            return;
        if (_collider.OverlapPoint(Camera.main.ScreenToWorldPoint(touch.position)))
            game.Fire();
    }
}
