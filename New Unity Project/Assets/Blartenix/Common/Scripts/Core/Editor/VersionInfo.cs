using UnityEditor;
using UnityEngine;

namespace Blartenix.EditorScripting.Internal
{
    internal static class VersionInfo
    {
        internal const string CURRENT_VERSION = "2.0";

        [MenuItem("Help/Blartenix/Common Package Version")]
        internal static void ShowCurrentVersion()
        {
            Debug.Log($"Blartenix Common Package Version: {CURRENT_VERSION}");
        }
    }
}