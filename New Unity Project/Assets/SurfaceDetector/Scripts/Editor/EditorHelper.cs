/********************************************
 * Copyright(c): 2017 Victor Klepikov       *
 *                                          *
 * Site: 	     http://u3d.as/5Fb		    *
 * Support: 	 http://bit.ly/vk-Support   *
 ********************************************/


using UnityEngine;
using UnityEditor;
using UnityEditorInternal;


namespace SurfaceDetection.Inspector
{
    public static class EditorHelper
    {
        // Show SimpleReorderableList
        public static void ShowSimpleReorderableList( ReorderableList list, string label, float space = 0f )
        {
            GUILayout.Space( 5f );

            if( space != 0f )
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space( space );
                GUILayout.BeginVertical();
            }

            list.drawHeaderCallback = ( Rect rect ) =>
            {
                EditorGUI.LabelField( rect, label );
            };

            list.drawElementCallback = ( Rect rect, int index, bool isActive, bool isFocused ) =>
            {
                rect.y += 2f;
                rect.height = EditorGUIUtility.singleLineHeight;

                EditorGUI.PropertyField( rect, list.serializedProperty.GetArrayElementAtIndex( index ), GUIContent.none );
            };
            list.DoLayoutList();

            if( space != 0f )
            {
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }
        }
       

        // Draw StringPopup
        public static void DrawStringPopup( SerializedProperty property, string[] names, string label, params GUILayoutOption[] options )
        {
            int id = GetStringId( property, names );
            id = EditorGUILayout.Popup( label, id, names, options );
            property.stringValue = ( id > -1 ) ? names[ id ] : string.Empty;
        }

        // Draw StringPopup
        public static void DrawStringPopup( Rect rect, SerializedProperty property, string[] names )
        {
            int id = GetStringId( property, names );
            id = EditorGUI.Popup( rect, id, names );
            property.stringValue = ( id > -1 ) ? names[ id ] : string.Empty;
        }
        // GetStringId
        static int GetStringId( SerializedProperty property, string[] names )
        {
            string propValue = property.stringValue;

            for( int i = 0; i < names.Length; i++ )
            {
                if( propValue == names[ i ] )
                {
                    return i;
                }
            }

            return -1;
        }


        // Draw StaticButton
        public static bool DrawStaticButton( GUIContent content, GUIStyle style, out Rect rect, params GUILayoutOption[] options )
        {
            rect = GUILayoutUtility.GetRect( content, style, options );
            Event ev = Event.current;

            if( ev.type == EventType.Repaint )
            {
                style.Draw( rect, content, false, false, false, false );
            }

            if( ev.type == EventType.MouseDown && rect.Contains( ev.mousePosition ) )
            {
                ev.Use();
                return true;
            }

            return false;
        }
    };
}