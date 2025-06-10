using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float moveSpeed = 2f;

    [SerializeField] Sprite spriteUp;
    [SerializeField] Sprite spriteDown;
    [SerializeField] Sprite spriteLeft;
    [SerializeField] Sprite spriteRight;

    Rigidbody2D rb;
    SpriteRenderer sr;

    Vector2 input;
    Vector2 velocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        rb.bodyType =  RigidbodyType2D.Kinematic;
    }

    private void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical"); //방향키 상하좌우 or WASD로 움직이기 가능

        velocity = input.normalized * moveSpeed;

        if(Mathf.Abs(input.x) > Mathf.Abs(input.y))
        {
            if (input.x > 0)
                sr.sprite = spriteRight;
            else if (input.x < 0)
                sr.sprite = spriteLeft;
        }
        else
        {
            if (input.y > 0)
                sr.sprite = spriteUp;
            else
                sr.sprite = spriteDown;
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }

}
