using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyTraceController : MonoBehaviour
{
    [Header("���� ����")]
    public int damage = 10;
    public float attackCooldown = 1.5f;

    [Header("���� ����")]
    public Transform player;
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float moveSpeed = 3.5f;

    [Header("���� ����")]
    public float patrolRadius = 5f;
    public float waitTime = 2f;
    public float raycastDistance = 2f;
    public float traceDistance = 10f;

    [SerializeField] Sprite spriteUp;
    [SerializeField] Sprite spriteDown;
    [SerializeField] Sprite spriteLeft;
    [SerializeField] Sprite spriteRight;

    SpriteRenderer sr;

    Vector2 input;
    Vector2 velocity;

    private float lastAttackTime;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (player == null) return;

        //�̵� ����
        Vector2 direction = (player.position - transform.position);
        float distanceToPlayer = direction.magnitude;

        // ���� ���� �ȿ� ������ ����
        if (distanceToPlayer <= attackRange)
        {
            // ���� ��ٿ� üũ
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                AttackPlayer();
                lastAttackTime = Time.time;
            }
        }

        if (direction.magnitude > traceDistance)
            return;

        Vector2 directionNormalized = direction.normalized;

        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, directionNormalized, raycastDistance);
        Debug.DrawRay(transform.position, directionNormalized * raycastDistance, Color.red);

        foreach(RaycastHit2D rHit in hits)
        {
            if (rHit.collider != null && rHit.collider.CompareTag("Obstacle"))
            {
                Vector3 alternativeDirect = Quaternion.Euler(0f, 0f, -90f) * direction;
                transform.Translate(alternativeDirect.normalized * moveSpeed * Time.deltaTime);
            }
            else
            {
                transform.Translate(direction.normalized * moveSpeed * Time.deltaTime);
            }
        }

        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical"); //����Ű �����¿� or WASD�� �����̱� ����

        velocity = input.normalized * moveSpeed;
    }

    private void UpdateSpriteDirection(Vector2 moveDirection)
    {
        if (sr == null) return;

        // �̵� ���⿡ ���� ��������Ʈ ����
        if (Mathf.Abs(moveDirection.x) > Mathf.Abs(moveDirection.y))
        {
            // �¿� �̵��� �� ū ���
            if (moveDirection.x > 0)
                sr.sprite = spriteRight;
            else
                sr.sprite = spriteLeft;
        }
        else
        {
            // ���� �̵��� �� ū ���
            if (moveDirection.y > 0)
                sr.sprite = spriteUp;
            else
                sr.sprite = spriteDown;
        }
    }

    void AttackPlayer()
    {
        // �÷��̾�� ������ �ֱ�
        HealingFactor playerHealth = player.GetComponent<HealingFactor>();
        if (playerHealth != null)
        {
            // HealingFactor ��ũ��Ʈ�� TakeDamage �Լ��� �ִٸ�
            playerHealth.Health -= damage;
            playerHealth.Health = Mathf.Max(playerHealth.Health, 0); // 0 ���Ϸ� �������� �ʰ�
        }

        // ���� ����Ʈ�� ���� ��� (���û���)
        Debug.Log($"���� �÷��̾ ����! ������: {damage}, �÷��̾� ü��: {playerHealth?.Health}");

        // ���� �ִϸ��̼� Ʈ���� (Animator�� �ִٸ�)
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }
    }

    // ������ ���� ǥ�� (Scene �信���� ����)
    void OnDrawGizmosSelected()
    {
        // ���� ����
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, traceDistance);

        // ���� ����
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // ����ĳ��Ʈ �Ÿ�
        Gizmos.color = Color.blue;
        if (player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            Gizmos.DrawRay(transform.position, direction * raycastDistance);
        }
    }
}
