using System.IO;
using UnityEngine;

public class LogManager : MonoBehaviour
{
    private StreamWriter logWriter;

    private void Awake()
    {
        // ������־�ļ���·��
        string logFilePath = Path.Combine(Application.persistentDataPath, "gameLog.txt");

        // ����StreamWriterʵ����д����־��׷�ӵ������ļ�
        logWriter = new StreamWriter(logFilePath, true);

        // �ض���Debug.Log���Զ��巽��
        Application.logMessageReceived += HandleLog;
    }

    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        // д����־�Ͷ�ջ���ٵ��ļ�
        logWriter.WriteLine($"[{System.DateTime.Now}] [{type}] {logString}");
        logWriter.WriteLine(stackTrace);
        logWriter.Flush(); // ȷ������д���ļ�
    }

    private void OnDestroy()
    {
        // ����
        Application.logMessageReceived -= HandleLog;
        logWriter.Close();
    }
}
