using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchScreenAnchor : MonoBehaviour
{
    public bool AnchorLeft;
    public bool AboveText;

    Camera cam;
    private void Awake()
    {
        cam = Camera.main;
    }
    void LateUpdate()
    {
        float height = GetKeyboardHeight(AboveText);
        float scaled = height / Screen.currentResolution.height;
        transform.position = (Vector2)cam.ScreenToWorldPoint(new Vector2(((AnchorLeft) ? 0f : cam.pixelWidth), scaled * cam.pixelHeight));
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
}
