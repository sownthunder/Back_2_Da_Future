using UnityEditor;

namespace Blartenix.EditorScripting
{
    [CustomEditor(typeof(TriggerPictogram))]
    public class TriggerPictogramEditor : Editor
    {
        SerializedProperty[] properties = null;

        private void OnEnable()
        {
            properties = new SerializedProperty[]
            {
                serializedObject.FindProperty("pictograms"),
                serializedObject.FindProperty("defaultPictogram"),
                serializedObject.FindProperty("triggeringTags"),
                serializedObject.FindProperty("lookForTagAt"),
                serializedObject.FindProperty("lookAtCamera"),
                serializedObject.FindProperty("lookAtMainCamera"),
                serializedObject.FindProperty("targetCam"),
            };
        }

        public override void OnInspectorGUI()
        {
            EditorTools.DrawScriptPropertyInInspector(serializedObject);

            if(properties[2].arraySize == 0)
            {
                properties[2].arraySize++;
                properties[2].GetArrayElementAtIndex(0).stringValue = "Player";
            }

            for (int i = 0; i < 5; i++)
            {
                EditorGUILayout.PropertyField(properties[i], true);
            }

            if (properties[4].boolValue)
                EditorGUILayout.PropertyField(properties[5], true);
            else if (properties[5].boolValue)
                properties[5].boolValue = false;

            if (properties[4].boolValue && !properties[5].boolValue)
                EditorGUILayout.PropertyField(properties[6], true);
            else if (properties[6].objectReferenceValue != null)
                properties[6].objectReferenceValue = null;
            

            serializedObject.ApplyModifiedProperties();
        }
    }
}