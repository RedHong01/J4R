using System.IO;
using UnityEngine;

public class LogManager : MonoBehaviour
{
    private StreamWriter logWriter;

    private void Awake()
    {
        // 设置日志文件的路径
        string logFilePath = Path.Combine(Application.persistentDataPath, "gameLog.txt");

        // 创建StreamWriter实例来写入日志；追加到现有文件
        logWriter = new StreamWriter(logFilePath, true);

        // 重定向Debug.Log到自定义方法
        Application.logMessageReceived += HandleLog;
    }

    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        // 写入日志和堆栈跟踪到文件
        logWriter.WriteLine($"[{System.DateTime.Now}] [{type}] {logString}");
        logWriter.WriteLine(stackTrace);
        logWriter.Flush(); // 确保立即写入文件
    }

    private void OnDestroy()
    {
        // 清理
        Application.logMessageReceived -= HandleLog;
        logWriter.Close();
    }
}
