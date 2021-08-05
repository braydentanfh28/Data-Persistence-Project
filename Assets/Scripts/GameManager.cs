using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public string PlayerName;
    public string HighScorePlayerName;
    public int HighScore;
    public InputField PlayerNameInput;

    [Serializable]
    public class PlayerData
    {
        public int highScore;
        public string playerName;
        public string highScorePlayerName;
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadData();

        if (PlayerName.Length >= 0)
        {
            PlayerNameInput.text = PlayerName;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartNew()
    {
        SceneManager.LoadScene(1);
    }

    public void NewPlayerNameEntered()
    {
        GameManager.Instance.PlayerName = PlayerNameInput.text;
        GameManager.Instance.SaveData();
    }

    public bool SaveHighScoreIfHit(int score)
    {
        if (score > HighScore)
        {
            GameManager.Instance.HighScore = score;
            GameManager.Instance.HighScorePlayerName = PlayerName;
            GameManager.Instance.SaveData();

            return true;
        }

        return false;
    }

    public void SaveData()
    {
        PlayerData data = new PlayerData();
        data.playerName = PlayerName;
        data.highScore = HighScore;
        data.highScorePlayerName = HighScorePlayerName;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadData()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);

            PlayerName = data.playerName;
            HighScore = data.highScore;
            HighScorePlayerName = data.highScorePlayerName;
        }
    }
}
