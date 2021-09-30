using UnityEngine;
using UnityEditor;

namespace Blartenix.EditorScripting
{
    [CustomPropertyDrawer(typeof(Sprite)), CanEditMultipleObjects]
    internal class SpritePropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent labelN)
        {
            if (property.objectReferenceValue != null)
            {
                return _texSize;
            }
            else
            {
                return base.GetPropertyHeight(property, labelN);
            }
        }

        private const float _texSize = 100;



        public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, prop);

            if (prop.objectReferenceValue != null)
            {
                //position.width = EditorGUIUtility.labelWidth;
                EditorGUI.PrefixLabel(position, new GUIContent(prop.displayName));

                //position.x += position.width;
                //position.width = _texSize;
                //position.height = _texSize;

                Rect pos = new Rect(EditorGUIUtility.labelWidth, position.y, _texSize + EditorTools.GetIndentPixelsGap(), _texSize);
                prop.objectReferenceValue = EditorGUI.ObjectField(pos, prop.objectReferenceValue, typeof(Sprite), false);
            }
            else
            {
                EditorGUI.PropertyField(position, prop, true);
            }

            EditorGUI.EndProperty();
        }
    }
}