using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{
    public static MainManager instance;
    public static string playerName;

    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text BestScoreText;
    public GameObject GameOverText;
    private static string bestPlayerName;
    private static int bestScore;

    public static float ballSpeed = 1.5f;

    public struct SaveData
    {
        public int highScore;
        public string playerName;
    }
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        LoadHighScore();

        if (bestPlayerName == null)
        {
            bestPlayerName = playerName;
            bestScore = 0;
        }

        UpdateBestScoreText();
    }

    void UpdateBestScoreText()
    {
        BestScoreText.text = "Best Score : " + bestPlayerName + " :" + bestScore;
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");
        NewStart();
    }

    void NewStart()
    {
        Debug.Log("NewStart");

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
               brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }

        ScoreText.text = "Score : " + playerName + " : " + m_Points;
     }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                //Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
                Ball.AddForce(forceDir * ballSpeed, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                m_GameOver = false;
                SceneManager.LoadScene(2);
                NewStart();
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = "Score : " + playerName + " : " + m_Points;

        if (m_Points > bestScore)
        {
            bestScore = m_Points;
            bestPlayerName = playerName;
            UpdateBestScoreText();
        }
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        SaveHighScore();
    }

    public void ExitGame()
    {
        Debug.Log("Quit application");
        Application.Quit();
    }

    public void SaveHighScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        SaveData data = new SaveData();
        data.highScore = bestScore;
        data.playerName = bestPlayerName;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);

        Debug.Log("Write to: " + path + " Data: " + json);
    }

    public void LoadHighScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            bestScore = data.highScore;
            bestPlayerName = data.playerName;

            Debug.Log("Read From: " + path + " Data: " + json);
        }
        else
        {
            bestScore = 0;
            bestPlayerName = null;
        }
    }
}
