using System;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Blartenix.Demos.Common
{
    public class bxMyLoggerClass : IBlartenixLogger
    {
        public void DisplayMessage(string message, BlartenixLogType type = BlartenixLogType.Info)
        {
            switch (type)
            {
                case BlartenixLogType.Info:
                    Debug.Log(message);
                    break;
                case BlartenixLogType.Warning:
                    Debug.LogWarning(message);
                    break;
                case BlartenixLogType.Error:
                    Debug.LogError(message);
                    break;
            }
        }

        public void Log(string message, BlartenixLogType type = BlartenixLogType.Info, [CallerMemberName] string callMember = null, [CallerFilePath] string file = null, [CallerLineNumber] int codeLine = -1)
        {
            string log = $"{message} [{DateTime.Now}] [{type}] [Scene: {SceneManager.GetActiveScene().name}] inside [{callMember}] in file [{file.Substring(file.LastIndexOf('\\'))}] at line [{codeLine}]";

            string folderPath = $"{Application.dataPath}/Logs";
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string fileName = DateTime.Now.ToString("dd-MM-yyyy HH-mm");
            string filePath = $"{folderPath}/{fileName}.txt";

            using (StreamWriter sw = new StreamWriter(filePath, true))
            {
                sw.WriteLine(log);
            }
        }
    }
}