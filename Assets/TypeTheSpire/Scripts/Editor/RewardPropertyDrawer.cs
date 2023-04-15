using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Bentendo.TTS
{
	[CustomPropertyDrawer(typeof(Reward))]
	public class RewardPropertyDrawer : PropertyDrawer
	{
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            base.OnGUI(position, property, label);
            var rewardType = property.FindPropertyRelative("rewardType");
            EditorGUI.BeginProperty(position, label, property);
            var rect = new Rect(position.position + new Vector2(0f, EditorGUIUtility.singleLineHeight), new Vector2(position.size.x, EditorGUIUtility.singleLineHeight));
            EditorGUI.PropertyField(rect, rewardType);
            var amt = property.FindPropertyRelative("amount");
            rect.position += Vector2.up * EditorGUIUtility.singleLineHeight;
            switch ((RewardType)rewardType.intValue)
            {
                case RewardType.ModifyStat:
                    var targ = property.FindPropertyRelative("targetStat");
                    EditorGUI.PropertyField(rect, targ);
                    rect.position += Vector2.up * EditorGUIUtility.singleLineHeight;
                    EditorGUI.PropertyField(rect, amt);
                    break;
                case RewardType.Card:
                    var card = property.FindPropertyRelative("cardReward");
                    EditorGUI.PropertyField(rect, card);
                    break;
                case RewardType.HP:
                    EditorGUI.PropertyField(rect, amt);
                    break;
            }
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var rewardType = property.FindPropertyRelative("rewardType");
            switch ((RewardType)rewardType.intValue)
            {
                default:
                case RewardType.ModifyStat:
                    return EditorGUIUtility.singleLineHeight * 4 + 5;
                case RewardType.Card:
                    return EditorGUIUtility.singleLineHeight * 3 + 5;
                case RewardType.HP:
                    return EditorGUIUtility.singleLineHeight * 3 + 5;
            }
        }
    }
}
