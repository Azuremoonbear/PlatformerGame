using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [Header("Game Settings")]
    public int playerLives = 3;
    public int score = 0;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void AddScore(int points)
    {
        score += points;
        UpdateUI();
    }
    
    public void GameOver()
    {
        playerLives--;
        if (playerLives <= 0)
        {
            RestartGame();
        }
        else
        {
            RespawnPlayer();
        }
    }
    
    void UpdateUI()
    {
        // UI 업데이트 로직
    }
    
    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    void RespawnPlayer()
    {
        // 플레이어 리스폰 로직
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            player.transform.position = Vector3.zero; // 시작 위치
        }
    }
}