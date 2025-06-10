using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using System;

[Serializable]

public class PlayerData
{
    public List<string> collectedItems = new List<string>();
    public int stage = 1;
}
public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance;
    public PlayerData playerData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadData();
        }
        else
        {
            Destroy(gameObject); //�ߺ�����
        }
    }

    // ������ LoadData �޼��� �߰�
    public PlayerData LoadData()
    {
        if (PlayerPrefs.HasKey("PlayerData"))
        {
            string jsonData = PlayerPrefs.GetString("PlayerData");
            playerData = JsonUtility.FromJson<PlayerData>(jsonData);
            return playerData;
        }
        return null;
    }

    // ������ SaveData �޼��� �߰�
    public void SaveData(PlayerData data)
    {
        string jsonData = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("PlayerData", jsonData);
        PlayerPrefs.Save();
        playerData = data; // �ν��Ͻ� ������ ������Ʈ
    }

    public void GameStart()
    {
        PlayerData playerData = LoadData();
        if (playerData == null)
        {
            playerData = new PlayerData();
            SceneManager.LoadScene("Level_1");
        }
        else
        {
            SceneManager.LoadScene("Level1_" + playerData.stage);
        }
    }

    public void PlayerDead()
    {
        PlayerData playerData = LoadData();
        if (playerData != null)
        {
            playerData.stage = 1; //�������� �ʱ�ȭ

            foreach (string item in playerData.collectedItems.ToList())
            {
                if (UnityEngine.Random.Range(0, 1) == 0) // 50% Ȯ���� ������ ����
                {
                    playerData.collectedItems.Remove(item);
                }
            }

            SaveData(playerData);

        }
        SceneManager.LoadScene("Gameover");
    }
}
