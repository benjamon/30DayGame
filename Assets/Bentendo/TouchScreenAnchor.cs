using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bentendo
{
    public class TouchScreenAnchor : MonoBehaviour
    {
        public float ReferenceWidth;
        public float ReferenceHeight;
        public bool AnchorLeft;
        public bool AboveText;

        Camera cam;
        private void Awake()
        {
            cam = Camera.main;
        }

        private void Start()
        {
            transform.position = (Vector2)cam.ScreenToWorldPoint(new Vector2(((AnchorLeft) ? 0f : cam.pixelWidth), 0f));
            float idealWidth = cam.ScreenToWorldPoint(new Vector2((!AnchorLeft) ? 0f : cam.pixelWidth, 0f)).x - transform.position.x;
            transform.localScale = Vector3.one * (idealWidth / ReferenceWidth);
            float testScale = Random.Range(.2f, .5f);
            transform.position = (Vector2)cam.ScreenToWorldPoint(new Vector2(((AnchorLeft) ? 0f : cam.pixelWidth), testScale * cam.pixelHeight));
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position + (ReferenceWidth * Vector3.right + Vector3.up * ReferenceHeight) * .5f, new Vector3(ReferenceWidth, ReferenceHeight, 0f));
        }

#if !UNITY_EDITOR
        float lastHeight = 0f;
        void LateUpdate()
        {
            float height = GetKeyboardHeight(AboveText);
            if (height != lastHeight)
            {
                lastHeight = height;
                float scale = height / Screen.currentResolution.height;
                transform.position = (Vector2)cam.ScreenToWorldPoint(new Vector2(((AnchorLeft) ? 0f : cam.pixelWidth), scale * cam.pixelHeight));
            }
    }

        public static int GetKeyboardHeight(bool includeInput)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                using (var unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                {
                    var unityPlayer = unityClass.GetStatic<AndroidJavaObject>("currentActivity").Get<AndroidJavaObject>("mUnityPlayer");
                    var view = unityPlayer.Call<AndroidJavaObject>("getView");
                    var dialog = unityPlayer.Get<AndroidJavaObject>("mSoftInputDialog");

                    if (view == null || dialog == null)
                        return 0;

                    var decorHeight = 0;

                    if (includeInput)
                    {
                        var decorView = dialog.Call<AndroidJavaObject>("getWindow").Call<AndroidJavaObject>("getDecorView");

                        if (decorView != null)
                            decorHeight = decorView.Call<int>("getHeight");
                    }

                    using (var rect = new AndroidJavaObject("android.graphics.Rect"))
                    {
                        view.Call("getWindowVisibleDisplayFrame", rect);
                        return Display.main.systemHeight - rect.Call<int>("height") + decorHeight;
                    }
                }
            }
            else
            {
                var height = Mathf.RoundToInt(TouchScreenKeyboard.area.height);
                return height >= Display.main.systemHeight ? 0 : height;
            }
        }
#endif
    }
}