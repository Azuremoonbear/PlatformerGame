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
            Destroy(gameObject); //중복방지
        }
    }

    // 누락된 LoadData 메서드 추가
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

    // 누락된 SaveData 메서드 추가
    public void SaveData(PlayerData data)
    {
        string jsonData = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("PlayerData", jsonData);
        PlayerPrefs.Save();
        playerData = data; // 인스턴스 변수도 업데이트
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
            playerData.stage = 1; //스테이지 초기화

            foreach (string item in playerData.collectedItems.ToList())
            {
                if (UnityEngine.Random.Range(0, 1) == 0) // 50% 확률로 아이템 삭제
                {
                    playerData.collectedItems.Remove(item);
                }
            }

            SaveData(playerData);

        }
        SceneManager.LoadScene("Gameover");
    }
}
