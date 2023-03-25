using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Bentendo.TTS
{
	public class KInput : MonoBehaviour
    {
        UnityAction<char> ProcessLetter;
        UnityAction Backspace;
        TouchScreenKeyboard m_Keyboard;
        int lastInputLength;

        public void SetAction(UnityAction<char> CharTyped, UnityAction backspace)
        {
            this.ProcessLetter = CharTyped;
            this.Backspace = backspace;
        }

        private void LateUpdate()
        {
            string s = "";
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
                s = m_Keyboard.text;
            }
            else
                s = Input.inputString;
            if (s.Length > lastInputLength)
                for (int i = lastInputLength; i < s.Length; i++)
                    ProcessLetter?.Invoke(s[i]);
            /*else if (s.Length < lastInputLength)
                for (int i = 0; i < lastInputLength - s.Length; i++)
                    Backspace?.Invoke();
            */
            lastInputLength = s.Length;
        }

        private void OnDisable()
        {
            if (m_Keyboard == null)
                return;
            if (m_Keyboard.active) m_Keyboard.active = false;
        }
    }
}
