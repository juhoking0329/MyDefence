using UnityEngine;

public class TurretController : MonoBehaviour
{
    [Header("터렛 설정")]
    [SerializeField] private Transform turretHead; // 회전시킬 터렛의 머리 부품
    [SerializeField] private float attackRange = 7.0f; // 공격 범위 (과제 조건: 7)
    [SerializeField] private float rotationSpeed = 10.0f; // 회전 속도

    private Transform targetEnemy; // 현재 조준하고 있는 가장 가까운 적

    void Update()
    {
        // 1. 매 순간 맵에서 가장 가까운 적을 찾습니다.
        FindClosestEnemy();

        // 2. 조준할 적이 있다면 터렛 머리를 그 방향으로 회전시킵니다.
        if (targetEnemy != null)
        {
            RotateTowardsTarget();
        }
    }

    // [핵심 스킬] 최소값 구하기 알고리즘을 이용한 가장 가까운 적 탐색 함수
    void FindClosestEnemy()
    {
        // 1) 현재 맵 위에 있는 태그가 "Enemy"인 모든 게임 오브젝트 정보를 배열로 가져옵니다.
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        GameObject closestEnemy = null;
        float minDistance = Mathf.Infinity; // 최소값을 구하기 위해 처음엔 무한대(가장 큰 값)로 설정합니다.

        // 2) 정보를 이용해서 모든 Enemy 중에 거리가 가장 가까운 Enemy를 찾습니다.
        foreach (GameObject enemy in enemies)
        {
            // 터렛과 적 사이의 거리를 계산합니다.
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

            // [최소값 알고리즘 적용] 방금 계산한 거리가 여태까지 찾은 최소 거리보다 작다면?
            if (distanceToEnemy < minDistance)
            {
                minDistance = distanceToEnemy; // 새로운 최소 거리로 업데이트
                closestEnemy = enemy;          // 가장 가까운 적 오브젝트 기억
            }
        }

        // 3) 발견한 가장 가까운 적이 공격 범위(7) 안에 있을 때만 타겟으로 인정합니다.
        if (closestEnemy != null && minDistance <= attackRange)
        {
            targetEnemy = closestEnemy.transform;
        }
        else
        {
            targetEnemy = null; // 범위 안에 적이 없으면 타겟을 비웁니다.
        }
    }

    // [핵심 스킬] Quaternion을 이용해 타겟 방향으로 고개 돌리기
    void RotateTowardsTarget()
    {
        // 타겟이 있는 방향의 화살표(방향 벡터)를 구합니다.
        Vector3 direction = targetEnemy.position - turretHead.position;

        // 터렛이 아래위(Y축)로 꺾이지 않고 수평으로만 회전하도록 Y축 값을 고정합니다. (원치 않으면 삭제 가능)
        direction.y = 0;

        // [Quaternion.LookRotation] 이 방향 화살표를 바라보는 유니티 내부 회전값(Quaternion)으로 변환합니다.
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        // [Quaternion.Slerp] 딱딱하게 툭 꺾이지 않고 부드럽게 스르륵 회전하도록 보간해 줍니다.
        turretHead.rotation = Quaternion.Slerp(turretHead.rotation, lookRotation, rotationSpeed * Time.deltaTime);
    }

    // [핵심 스킬] Gizmos를 이용하여 에디터 화면에 공격 범위 그리기
    private void OnDrawGizmosSelected()
    {
        // 터렛 오브젝트를 클릭했을 때만 공격 범위를 나타내는 와이어 구체를 그려줍니다.
        Gizmos.color = Color.red; // 빨간색 선으로 표시
        Gizmos.DrawWireSphere(transform.position, attackRange); // 터렛 위치 기준, 반지름 7
    }
}