using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyTraceController : MonoBehaviour
{
    [Header("공격 설정")]
    public int damage = 10;
    public float attackCooldown = 1.5f;

    [Header("추적 설정")]
    public Transform player;
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float moveSpeed = 3.5f;

    [Header("상태 설정")]
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

        //이동 로직
        Vector2 direction = (player.position - transform.position);
        float distanceToPlayer = direction.magnitude;

        // 공격 범위 안에 있으면 공격
        if (distanceToPlayer <= attackRange)
        {
            // 공격 쿨다운 체크
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
        input.y = Input.GetAxisRaw("Vertical"); //방향키 상하좌우 or WASD로 움직이기 가능

        velocity = input.normalized * moveSpeed;
    }

    private void UpdateSpriteDirection(Vector2 moveDirection)
    {
        if (sr == null) return;

        // 이동 방향에 따라 스프라이트 변경
        if (Mathf.Abs(moveDirection.x) > Mathf.Abs(moveDirection.y))
        {
            // 좌우 이동이 더 큰 경우
            if (moveDirection.x > 0)
                sr.sprite = spriteRight;
            else
                sr.sprite = spriteLeft;
        }
        else
        {
            // 상하 이동이 더 큰 경우
            if (moveDirection.y > 0)
                sr.sprite = spriteUp;
            else
                sr.sprite = spriteDown;
        }
    }

    void AttackPlayer()
    {
        // 플레이어에게 데미지 주기
        HealingFactor playerHealth = player.GetComponent<HealingFactor>();
        if (playerHealth != null)
        {
            // HealingFactor 스크립트에 TakeDamage 함수가 있다면
            playerHealth.Health -= damage;
            playerHealth.Health = Mathf.Max(playerHealth.Health, 0); // 0 이하로 내려가지 않게
        }

        // 공격 이펙트나 사운드 재생 (선택사항)
        Debug.Log($"적이 플레이어를 공격! 데미지: {damage}, 플레이어 체력: {playerHealth?.Health}");

        // 공격 애니메이션 트리거 (Animator가 있다면)
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }
    }

    // 기즈모로 범위 표시 (Scene 뷰에서만 보임)
    void OnDrawGizmosSelected()
    {
        // 추적 범위
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, traceDistance);

        // 공격 범위
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // 레이캐스트 거리
        Gizmos.color = Color.blue;
        if (player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            Gizmos.DrawRay(transform.position, direction * raycastDistance);
        }
    }
}
