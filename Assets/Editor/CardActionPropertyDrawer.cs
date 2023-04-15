using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Bentendo.TTS
{
	[CustomPropertyDrawer(typeof(CardAction))]
	public class CardActionPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            base.OnGUI(position, property, label);
            var actionId = property.FindPropertyRelative("actionId");
            var lineSize = EditorGUIUtility.singleLineHeight;
            EditorGUI.BeginProperty(position, label, property);
            var rect = new Rect(position.position + new Vector2(0f, lineSize), new Vector2(position.size.x, lineSize));
            EditorGUI.PropertyField(rect, actionId);
            NextLine(ref rect);
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("targetId"));
            NextLine(ref rect);
            var amt = property.FindPropertyRelative("amount");
            var card = property.FindPropertyRelative("cardAction");
            switch ((ActionType)actionId.intValue)
            {
                case ActionType.PLAY_CARD:
                    EditorGUI.PropertyField(rect, card);
                    NextLine(ref rect);
                    EditorGUI.PropertyField(rect, amt);
                    break;
                case ActionType.STRIKE:
                case ActionType.BLOCK:
                case ActionType.DRAW:
                case ActionType.HEAL:
                case ActionType.DISCARD:
                    EditorGUI.PropertyField(rect, amt);
                    break;
                case ActionType.APPLY_STATUS:
                    EditorGUI.PropertyField(rect, property.FindPropertyRelative("statusType"));
                    NextLine(ref rect);
                    EditorGUI.PropertyField(rect, amt);
                    break;
                case ActionType.MODIFY_STAT:
                    EditorGUI.PropertyField(rect, property.FindPropertyRelative("targetStat"));
                    NextLine(ref rect);
                    EditorGUI.PropertyField(rect, amt);
                    break;
            }
            EditorGUI.EndProperty();
        }

        Rect NextLine(ref Rect rect)
        {
            rect.position += Vector2.up * EditorGUIUtility.singleLineHeight;
            return rect;
        }


        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var actionId = property.FindPropertyRelative("actionId");
            var lineSize = EditorGUIUtility.singleLineHeight;
            float space = 6 + lineSize *3 ;
            switch ((ActionType)actionId.intValue)
            {
                case ActionType.PLAY_CARD:
                case ActionType.APPLY_STATUS:
                case ActionType.MODIFY_STAT:
                    return space + lineSize * 2;
                case ActionType.STRIKE:
                case ActionType.BLOCK:
                case ActionType.DRAW:
                case ActionType.HEAL:
                case ActionType.DISCARD:
                    return space + lineSize * 1;
            }
            return lineSize * 2f + space;
        }
    }
}
