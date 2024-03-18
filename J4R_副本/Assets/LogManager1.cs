using System;
using System.IO;
using UnityEngine;

public class LogManager1 : MonoBehaviour
{
    private string logFilePath;

    private void Awake()
    {
        // 指定日志文件的路径
        logFilePath = Path.Combine(Application.persistentDataPath, "gameLog.txt");

        // 确保日志文件存在
        if (!File.Exists(logFilePath))
        {
            File.Create(logFilePath).Dispose();
        }

        // 注册日志回调
        Application.logMessageReceived += LogMessage;
    }

    private void OnDestroy()
    {
        // 取消注册日志回调
        Application.logMessageReceived -= LogMessage;
    }

    private void LogMessage(string logString, string stackTrace, LogType type)
    {
        // 构建日志信息，包括时间戳和日志类型
        string logEntry = $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] [{type}] {logString}\n";
        File.AppendAllText(logFilePath, logEntry);

        // 如果需要，也可以包含堆栈跟踪信息
        if (type == LogType.Exception)
        {
            File.AppendAllText(logFilePath, $"StackTrace: {stackTrace}\n");
        }
    }
}
