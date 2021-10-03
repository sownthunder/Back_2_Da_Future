/********************************************
 * Copyright(c): 2017 Victor Klepikov       *
 *                                          *
 * Site: 	     http://u3d.as/5Fb		    *
 * Support: 	 http://bit.ly/vk-Support   *
 ********************************************/


using UnityEngine;
using UnityEditor;


namespace SurfaceDetection.Inspector
{
    public sealed class SurfaceDetectorWindow : EditorWindow
    {
        static SurfaceDetectorWindow window;

        internal static string mainDirectory { get; private set; }

        static bool dirty, needReinit, needSave;


        // On ScriptsRecompiled
        [UnityEditor.Callbacks.DidReloadScripts]
        static void OnScriptsRecompiled()
        {
            needReinit = true;
        }

        // OnInspectorUpdate
        void OnInspectorUpdate()
        {
            if( needReinit )
            {
                needReinit = false;
                Init();
            }

            Repaint();
        }


        // SetDirty Data
        public static void SetDirtyData()
        {
            needSave = true;
        }


        // SetDirty Data
        public static void MarkDirty()
        {
            dirty = true;
        }


        // OnDestroy
        void OnDestroy()
        {
            if( needSave )
            {
                int closeId = EditorUtility.DisplayDialogComplex( "Save changes", "Warning: You have not saved changes! Save?", "Save", "No", "Cancel" );

                if( closeId == 0 )
                {
                    SurfaceDetectorTab.SaveSettings();
                }
                else if( closeId == 2 )
                {
                    window = CreateInstance<SurfaceDetectorWindow>();
                    Init();
                    return;
                }

                needSave = false;
            }            

            SurfaceDetectorTab.FullReset();
            AssetDatabase.DeleteAsset( mainDirectory + "/tmp" );
        }



        // Init
        [MenuItem( "Window/Victor's Assets/Surface Detector" )]
        public static void Init()
        {
            window = GetWindow<SurfaceDetectorWindow>( "Surfaces" );
            window.minSize = new Vector2( 725f, 535f );
            window.Focus();

            mainDirectory = GetResourcesPath( MonoScript.FromScriptableObject( window ) );

            SurfaceDetectorTab.SetupTab();
        }


        // OnGUI
        void OnGUI()
        {
            if( dirty )
            {
                dirty = false;
                Repaint();                
            }


            EditorGUILayout.BeginHorizontal();
            SurfaceDetectorTab.OnWindowGUI();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            const float height = 30f;
            bool reload = GUILayout.Button( "Reload Settings", GUILayout.Height( height ) );
            bool save = GUILayout.Button( "Save Settings", GUILayout.Height( height ) );
            EditorGUILayout.EndHorizontal();

            if( save )
            {
                SurfaceDetectorTab.SaveSettings();
                needSave = false;
            }

            if( reload && EditorUtility.DisplayDialog( "Warning!", "Warning: All changes will be reset! Сontinue?", "Yes", "No" ) )
            {
                SurfaceDetectorTab.ReloadSettings();
                needSave = false;
            }
        }


        // Get ResourcesPath
        private static string GetResourcesPath( MonoScript monoScript )
        {
            string assetPath = AssetDatabase.GetAssetPath( monoScript );
            const string startFolder = "Assets";
            const string endFolder = "/Scripts";
            const string resFolder = "Resources";

            if( assetPath.Contains( startFolder ) && assetPath.Contains( endFolder ) )
            {
                int startIndex = assetPath.IndexOf( startFolder, 0 ) + startFolder.Length;
                int endIndex = assetPath.IndexOf( endFolder, startIndex );

                string between = assetPath.Substring( startIndex, endIndex - startIndex );
                string projectFolder = startFolder + between;
                string resPath = projectFolder + "/" + resFolder;

                bool refresh = false;

                if( AssetDatabase.IsValidFolder( resPath ) == false )
                {
                    AssetDatabase.CreateFolder( projectFolder, resFolder );
                    refresh = true;
                }

                if( AssetDatabase.IsValidFolder( resPath + "/tmp" ) == false )
                {
                    AssetDatabase.CreateFolder( resPath, "tmp" );
                    refresh = true;
                }

                if( refresh )
                {
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }

                return resPath;
            }

            return string.Empty;
        }
    };
}