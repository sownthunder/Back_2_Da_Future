using UnityEditor;

namespace Blartenix.EditorScripting
{
    [CustomEditor(typeof(LanguageText))]
    public class LanguageTextEditor : Editor
    {
        private SerializedProperty[] properties = null;


        public override void OnInspectorGUI()
        {
            properties = new SerializedProperty[]
            {
                serializedObject.FindProperty("languageTemplate"),
                serializedObject.FindProperty("idName"),
                serializedObject.FindProperty("idNameIndex"),
                serializedObject.FindProperty("text")
            };

            EditorTools.DrawScriptPropertyInInspector(serializedObject);
            EditorGUILayout.Separator();
            EditorGUILayout.PropertyField(properties[0], true);
            if (properties[0].objectReferenceValue != null)
            {

                SerializedObject templateSO = new SerializedObject(properties[0].objectReferenceValue);
                SerializedProperty idNamesProp = templateSO.FindProperty("languageTextIdNames");

                if (idNamesProp.arraySize != 0)
                {
                    if (properties[2].intValue == -1)
                        properties[2].intValue = 0;


                    string[] idNames = new string[idNamesProp.arraySize];

                    for (int i = 0; i < idNamesProp.arraySize; i++)
                    {
                        idNames[i] = idNamesProp.GetArrayElementAtIndex(i).stringValue;
                    }
                    properties[2].intValue = EditorGUILayout.Popup(properties[1].displayName, properties[2].intValue, idNames);
                    properties[1].stringValue = idNames[properties[2].intValue];
                }
                else
                    EditorGUILayout.LabelField("Template doesn't have any language texts defined", EditorStyles.miniLabel);

                EditorGUILayout.PropertyField(properties[3], true);

            }
            else
            {
                if (properties[2].intValue != -1)
                    properties[2].intValue = -1;

                EditorGUILayout.PropertyField(properties[1], true);
                EditorGUILayout.PropertyField(properties[3], true);
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}