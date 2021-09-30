using System;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Blartenix.EditorScripting
{
    [CustomEditor(typeof(LanguageManager))]
    public class LanguageManagerEditor : Editor
    {
        SerializedObject singletonSerializedObject = null;

        private ReorderableList reorderableList = null;

        private void OnEnable()
        {
            singletonSerializedObject = new SerializedObject((SingletonBehaviour<LanguageManager>)target);
            reorderableList = new ReorderableList(serializedObject, serializedObject.FindProperty("languageFiles"), !Application.isPlaying, true, true, true);
            reorderableList.drawHeaderCallback = (Rect rect) => EditorGUI.LabelField(rect, "Language Files", EditorStyles.boldLabel);
            reorderableList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                rect.y += EditorGUIUtility.standardVerticalSpacing;
                rect.height = EditorGUIUtility.singleLineHeight;
                SerializedProperty[] itemProperties = new SerializedProperty[]
                {
                    reorderableList.serializedProperty.GetArrayElementAtIndex(index),
                };

                BlartenixLanguage temp = null;
                if (itemProperties[0].objectReferenceValue != null)
                {
                    temp = Utilities.DeserializeXML<BlartenixLanguage>(((TextAsset)itemProperties[0].objectReferenceValue).text, false);
                }
                
                EditorGUI.PropertyField(rect, itemProperties[0], new GUIContent(!string.IsNullOrEmpty(temp?.name)? temp.name :  "Language File"), true);
            };
        }


        public override void OnInspectorGUI()
        {
            SerializedProperty[] properties = new SerializedProperty[]
            {
                serializedObject.FindProperty("dontDestroyOnLoad"),//singleton behaviour
                serializedObject.FindProperty("gameSettings"),
                serializedObject.FindProperty("languageFiles"),
                serializedObject.FindProperty("selectedLanguage")
            };

            EditorGUILayout.Separator();
            EditorTools.DrawScriptPropertyInInspector(serializedObject);
            EditorGUILayout.Separator();

            

            EditorGUILayout.PropertyField(properties[0], true);
            EditorGUILayout.PropertyField(properties[1], true);
            reorderableList.DoLayoutList();
            if (properties[2].arraySize > 0)
            {
                if (properties[3].intValue == -1)
                    properties[3].intValue = 0;

                if (properties[3].intValue >= properties[2].arraySize)
                    properties[3].intValue = properties[2].arraySize - 1;


                string[] languageNames = new string[properties[2].arraySize];
                bool errorDeserializing = false;
                int indexFileError = -1;
                for (int i = 0; i < languageNames.Length; i++)
                {
                    try
                    {
                        string name = Utilities.DeserializeXML<BlartenixLanguage>(((TextAsset)properties[2].GetArrayElementAtIndex(i).objectReferenceValue).text, false).name;
                        languageNames[i] = !string.IsNullOrEmpty(name)  ? name : "Unnamed"; 

                    }
                    catch (Exception)
                    {
                        errorDeserializing = true;
                        indexFileError = i;
                        break;
                    }
                }




                if (!errorDeserializing)
                {
                    EditorGUI.BeginDisabledGroup(true);

                    properties[3].intValue = EditorGUILayout.Popup(properties[3].displayName, properties[3].intValue, languageNames);


                    EditorGUI.EndDisabledGroup();
                }
                else
                {
                    EditorGUILayout.PrefixLabel(properties[3].displayName);
                    EditorGUILayout.HelpBox($"There is an error with the language file at index {indexFileError}. It couldn't be deserializaed.", MessageType.Error);
                    properties[3].intValue = -1;
                }
            }
            else
            {
                EditorGUILayout.HelpBox($"No language files attached", MessageType.Warning);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}