using System;
using System.IO;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Blartenix.EditorScripting
{
    [CustomEditor(typeof(LanguageTemplate))]
    public class LanguageTemplateEditor : Editor
    {
        private static bool exportFoldout = false;
        private static bool languageTextsFoldout = false;


        private LanguageTemplate template = null;
        private SerializedProperty[] properties = null;
        private ReorderableList groupedTemplatesList = null;
        private ReorderableList languageTextsList = null;
        private string assetPath = string.Empty;

        private void OnEnable()
        {
            template = (LanguageTemplate)target;
            assetPath = AssetDatabase.GetAssetPath(serializedObject.targetObject);
            properties = new SerializedProperty[]
            {
                serializedObject.FindProperty("id"),
                serializedObject.FindProperty("groupedTemplates"),
                serializedObject.FindProperty("language.languageTexts"),
                serializedObject.FindProperty("languageTextIdNames"),
            };

            groupedTemplatesList = new ReorderableList(serializedObject, properties[1], true, true, true, true);
            groupedTemplatesList.drawHeaderCallback = (Rect rect) => EditorGUI.LabelField(rect, "Grouped Templates", EditorStyles.boldLabel);
            groupedTemplatesList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                SerializedProperty element = groupedTemplatesList.serializedProperty.GetArrayElementAtIndex(index);
                rect.height = EditorGUIUtility.singleLineHeight;
                element.objectReferenceValue = EditorGUI.ObjectField(rect, element.objectReferenceValue, typeof(LanguageTemplate), false);
            };

            languageTextsList = new ReorderableList(serializedObject, properties[2], true, true, true, true);
            languageTextsList.drawHeaderCallback = (Rect rect) => EditorGUI.LabelField(rect, "Language Text ID Names", EditorStyles.boldLabel);
            languageTextsList.onAddCallback = (list) =>
            {
                properties[3].arraySize++;
                languageTextsList.serializedProperty.arraySize++;
                
                int newIndex = languageTextsList.serializedProperty.arraySize - 1;
                SerializedProperty newElement = languageTextsList.serializedProperty.GetArrayElementAtIndex(newIndex);
                
                string defaultIdName = $"Language_Text_{Guid.NewGuid()}";
                newElement.FindPropertyRelative("idName").stringValue = defaultIdName;
                properties[3].GetArrayElementAtIndex(newIndex).stringValue = defaultIdName;
                
                serializedObject.ApplyModifiedProperties();
                serializedObject.Update();
            };
            languageTextsList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                rect.y += EditorGUIUtility.standardVerticalSpacing;
                rect.height = EditorGUIUtility.singleLineHeight;
                SerializedProperty[] itemProperties = new SerializedProperty[]
                {
                    languageTextsList.serializedProperty.GetArrayElementAtIndex(index).FindPropertyRelative("idName"),
                    languageTextsList.serializedProperty.GetArrayElementAtIndex(index).FindPropertyRelative("value")
                };

                string oldIDName = itemProperties[0].stringValue;
                EditorGUI.BeginChangeCheck();
                string newIDName = EditorGUI.DelayedTextField(rect, "Language Text", itemProperties[0].stringValue);
                if (EditorGUI.EndChangeCheck())
                {
                    newIDName = newIDName.Trim();
                    bool existeIDName = false;
                    for (int i = 0; i < languageTextsList.serializedProperty.arraySize; i++)
                    {
                        existeIDName = languageTextsList.serializedProperty.GetArrayElementAtIndex(i).FindPropertyRelative("idName").stringValue == newIDName;
                        if (existeIDName)
                            break;
                    }

                //Si ya existe uno con ese nombre no se haace el cambio
                if (existeIDName || string.IsNullOrEmpty(newIDName))
                        newIDName = oldIDName;

                    if (oldIDName != newIDName)
                    {
                        properties[3].GetArrayElementAtIndex(index).stringValue = newIDName;
                        itemProperties[0].stringValue = newIDName;
                        itemProperties[1].stringValue = "#VALUE#";
                    }
                }
            };
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("Blartenix Language Template", MessageType.Info);
            EditorGUILayout.Separator();
            if (string.IsNullOrEmpty(properties[0].stringValue))
            {
                properties[0].stringValue = Guid.NewGuid().ToString();
            }
            EditorGUILayout.LabelField($"Template ID: {properties[0].stringValue}", EditorStyles.miniBoldLabel);
            EditorGUILayout.Separator();

            properties[1].isExpanded = EditorGUILayout.Foldout(properties[1].isExpanded, properties[1].displayName, true);
            if (properties[1].isExpanded)
            {
                EditorGUILayout.Separator();
                if (GUILayout.Button("Clear", EditorStyles.miniButton))
                {
                    if (groupedTemplatesList.count > 0)
                    {
                        if (EditorUtility.DisplayDialog("Clear List", "You are going to delete all the grouped language templates. Are you sure?", "Clear", "Cancel"))
                            groupedTemplatesList.serializedProperty.arraySize = 0;
                    }
                }
                groupedTemplatesList.DoLayoutList();
            }

            languageTextsFoldout = EditorGUILayout.Foldout(languageTextsFoldout, "Language Texts Definition", true);
            if (languageTextsFoldout)
            {
                EditorGUILayout.Separator();
                if(GUILayout.Button("Clear", EditorStyles.miniButton))
                {
                    if (languageTextsList.count > 0)
                    {
                        if (EditorUtility.DisplayDialog("Clear List", "You are going to delete all the texts definitions. Are you sure?", "Clear", "Cancel"))
                        {
                            languageTextsList.serializedProperty.arraySize = 0;
                            properties[3].arraySize = 0;

                        }
                    }
                }
                languageTextsList.DoLayoutList();
            }

            EditorGUILayout.Space();

            if (assetPath != string.Empty)
            {
                exportFoldout = EditorGUILayout.Foldout(exportFoldout, "Export Template", true);
                if (exportFoldout)
                {
                    GUILayout.BeginVertical(GUI.skin.box);
                    EditorGUILayout.LabelField("Folder", EditorStyles.miniBoldLabel);
                    string folderPath = assetPath.Remove(assetPath.LastIndexOf('/') + 1);
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.TextArea(folderPath, EditorStyles.miniLabel);
                    EditorGUI.EndDisabledGroup();
                    EditorGUILayout.LabelField("File", EditorStyles.miniBoldLabel);
                    string file = $"{serializedObject.targetObject.name}.xml";
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.TextArea(file, EditorStyles.miniLabel);
                    EditorGUI.EndDisabledGroup();
                    GUILayout.EndVertical();

                    string appDataPath = Application.dataPath;
                    appDataPath = appDataPath.Remove(appDataPath.LastIndexOf('/') + 1);

                    string filePath = appDataPath + folderPath + file;
                    if (GUILayout.Button("Export", EditorStyles.miniButton))
                    {
                        if (properties[2].arraySize > 0)
                        {
                            if (!File.Exists(filePath))
                                ExportTemplate(filePath);
                            else if ((File.Exists(filePath) && EditorUtility.DisplayDialog("Exsiting File", $"{file} already exist. Continue and override data?", "Override", "Cancel")))
                                ExportTemplate(filePath);
                        }
                        else
                            EditorUtility.DisplayDialog("Export Language Template", "Can't export a language template without at least one language text configured.", "OK");
                    }

                    EditorGUILayout.Space();
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void ExportTemplate(string filePath)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    sw.Write(template.Export());
                }
                Debug.Log($"Blartenix Language Template '{serializedObject.targetObject.name}' exported succesfully.");
            }
            catch (Exception e)
            {
                throw e;
            }
            AssetDatabase.Refresh();
        }
    }
}