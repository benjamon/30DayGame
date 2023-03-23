using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bentendo;

namespace Bentendo
{
	public class CamRigOne : MonoBehaviour
	{
        public Transform ShakeParent;
		Transform tCamera;
		Camera cam;

        public PhysUtils.bSpring ShiftSpring;
        public PhysUtils.bSpring ShakeSpring;

        public void Start()
        {
            cam = GetComponentInChildren<Camera>();
            tCamera = cam.transform;
        }

        Vector2 initialDisplacement = Vector2.right;
        float shakeAngle = 1f;

        public void Shove(Vector2 diff)
        {
            initialDisplacement = diff + (ShiftSpring.position * initialDisplacement);
            ShiftSpring.velocity = 0f;
            ShiftSpring.position = 1f;
        }

        public void Shake(float diff)
        {
            shakeAngle = ShakeSpring.position * shakeAngle + diff;
            ShakeSpring.position = 1f; 
        }

        public void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 pt = cam.ScreenToWorldPoint(Input.mousePosition);
                Shove(-((Vector2)transform.position - pt).normalized);
            }

            if (Input.GetMouseButtonDown(1))
            {
                Vector2 pt = cam.ScreenToWorldPoint(Input.mousePosition);
                Shake(-((Vector2)transform.position - pt).normalized.x * 12f);
            }

            ShiftSpring.Update(Time.deltaTime);
            ShakeParent.transform.localPosition = ShiftSpring.position * initialDisplacement;

            ShakeSpring.Update(Time.deltaTime);
            ShakeParent.transform.rotation = Quaternion.identity;
            ShakeParent.transform.Rotate(Vector3.forward, ShakeSpring.position * shakeAngle);
        }
    }
}
