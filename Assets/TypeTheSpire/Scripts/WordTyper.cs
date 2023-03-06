using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WordTyper : MonoBehaviour
{
    UnityAction<char> ProcessLetter;
    TouchScreenKeyboard m_Keyboard;
    int lastInputLength;
    
    public void SetTarget(UnityAction<char> act)
    {
        ProcessLetter = act;
    }

    private void LateUpdate()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (m_Keyboard == null || !m_Keyboard.active || m_Keyboard.status != TouchScreenKeyboard.Status.Visible)
            {
                TouchScreenKeyboard.hideInput = true;
                m_Keyboard = TouchScreenKeyboard.Open("~", TouchScreenKeyboardType.NamePhonePad, false, false, true);
                lastInputLength = 1;
                TouchScreenKeyboard.Android.consumesOutsideTouches = false;
            }
            m_Keyboard.selection = new RangeInt(m_Keyboard.text.Length, 0);
            string s = m_Keyboard.text;
            if (s.Length > lastInputLength)
            {
                for (int i = lastInputLength; i < s.Length; i++)
                    ProcessLetter?.Invoke(s[i]);
            }
            lastInputLength = s.Length;
        }
    }
}