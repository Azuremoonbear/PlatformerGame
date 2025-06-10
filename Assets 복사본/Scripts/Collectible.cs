using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [Header("Collectible Settings")]
    public int pointValue = 10;
    public AudioClip collectSound;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Collect();
        }
    }
    
    void Collect()
    {
        // 점수 추가
        GameManager.Instance.AddScore(pointValue);
        
        // 사운드 재생
        if (collectSound != null)
        {
            AudioSource.PlayClipAtPoint(collectSound, transform.position);
        }
        
        // 아이템 제거
        Destroy(gameObject);
    }
}