using UnityEditor;

namespace Blartenix.EditorScripting
{
    internal static class EditorTools
    {
        public const int PIXELS_PER_INDENT_LEVEL = 14;

        public static int GetIndentPixelsGap() => EditorGUI.indentLevel * PIXELS_PER_INDENT_LEVEL;

        public static void DrawScriptPropertyInInspector(SerializedObject serializedObject)
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
            EditorGUI.EndDisabledGroup();
        }
    }
}
