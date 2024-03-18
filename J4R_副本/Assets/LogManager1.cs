using System;
using System.IO;
using UnityEngine;

public class LogManager1 : MonoBehaviour
{
    private string logFilePath;

    private void Awake()
    {
        // ָ����־�ļ���·��
        logFilePath = Path.Combine(Application.persistentDataPath, "gameLog.txt");

        // ȷ����־�ļ�����
        if (!File.Exists(logFilePath))
        {
            File.Create(logFilePath).Dispose();
        }

        // ע����־�ص�
        Application.logMessageReceived += LogMessage;
    }

    private void OnDestroy()
    {
        // ȡ��ע����־�ص�
        Application.logMessageReceived -= LogMessage;
    }

    private void LogMessage(string logString, string stackTrace, LogType type)
    {
        // ������־��Ϣ������ʱ�������־����
        string logEntry = $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] [{type}] {logString}\n";
        File.AppendAllText(logFilePath, logEntry);

        // �����Ҫ��Ҳ���԰�����ջ������Ϣ
        if (type == LogType.Exception)
        {
            File.AppendAllText(logFilePath, $"StackTrace: {stackTrace}\n");
        }
    }
}
