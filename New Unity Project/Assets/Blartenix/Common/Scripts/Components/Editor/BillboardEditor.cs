using UnityEditor;

namespace Blartenix.EditorScripting
{
    [CustomEditor(typeof(Billboard))]
    public class BillboardEditor : Editor
    {
        SerializedProperty[] properties = null;

        private void OnEnable()
        {
            properties = new SerializedProperty[]
            {
                serializedObject.FindProperty("lookAtMainCamera"),
                serializedObject.FindProperty("lookAtTarget"),
                serializedObject.FindProperty("lookPositionOffset"),
                serializedObject.FindProperty("invertXScale"),
                serializedObject.FindProperty("smooth"),
                serializedObject.FindProperty("rotateX"),
            };
        }

        public override void OnInspectorGUI()
        {
            EditorTools.DrawScriptPropertyInInspector(serializedObject);

            if (properties[0].boolValue)
            {
                for (int i = 0; i < properties.Length; i++)
                {
                    if (i == 1) continue;

                    EditorGUILayout.PropertyField(properties[i], true);
                }
            }
            else
            {
                for (int i = 0; i < properties.Length; i++)
                {
                    EditorGUILayout.PropertyField(properties[i], true);
                }
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}