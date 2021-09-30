using UnityEditor;

namespace Blartenix.EditorScripting
{
    [CustomEditor(typeof(TriggerPictogram2D))]
    public class TriggerPictogram2DEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            SerializedProperty p = serializedObject.FindProperty("triggeringTags");
            
            if(p.arraySize == 0)
            {
                p.arraySize++;
                p.GetArrayElementAtIndex(0).stringValue = "Player";
                serializedObject.ApplyModifiedProperties();
            }

            DrawDefaultInspector();
        }
    }
}
