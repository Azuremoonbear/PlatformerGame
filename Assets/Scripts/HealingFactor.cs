using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingFactor : MonoBehaviour
{
    public int Health = 100;
    public int MaxHealth = 100;  // �ִ� ü�� ����
    public float Timer = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Timer -= Time.deltaTime;

        if(Timer <= 0)
        {
            Timer = 1.0f;

            // Health�� MaxHealth���� ���� ���� ȸ��
            if (Health < MaxHealth)
            {
                Health += 1;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        Health = Mathf.Max(Health - damage, 0);
    }
}
