using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scencermanger : MonoBehaviour
{
    public GameMain gameMainPf;

    // Start is called before the first frame update
    void Start()
    {
        if (GameMain.Instance == null)
        {
            Instantiate(gameMainPf);
            DontDestroyOnLoad(GameMain.Instance.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startgame()
    {

        GameMain.Instance.OnGameStart();
        SceneManager.LoadScene(1);
    }

    public void quitgame()
    {
        Application.Quit();
    }
}
