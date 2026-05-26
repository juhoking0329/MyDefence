using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    // 이동할 목적지(Target)의 위치 정보를 담을 변수입니다.
    [SerializeField]
    private Transform targetTransform;

    // 이동 속도
    [SerializeField]
    private float speed = 5.0f;

    // 도착했다고 판단할 거리 (너무 엄격하면 소수점 계산 때문에 도착 처리가 안 될 수 있어요!)
    [SerializeField]
    private float arrivalDistance = 0.1f;

    void Update()
    {
        // 만약 타겟이 지정되지 않았다면 에러를 방지하기 위해 리턴합니다.
        if (targetTransform == null) return;

        // 1. 방향 구하기 (목적지 위치 - 내 현재 위치)
        Vector3 direction = targetTransform.position - transform.position;

        // 2. 목적지를 향해 회전하지 않고 똑바로 이동하기 위해 '방향만(normalized)' 추출합니다.
        Vector3 moveDir = direction.normalized;

        // 3. Translate와 Time.deltaTime을 이용해 매 프레임 조금씩 이동합니다.
        transform.Translate(moveDir * speed * Time.deltaTime, Space.World);

        // 4. [도착 판정] 나와 목적지 사이의 거리를 계산합니다.
        float distanceToTarget = Vector3.Distance(transform.position, targetTransform.position);

        // 5. 설정한 도착 거리보다 가까워졌다면? (도착 완료!)
        if (distanceToTarget <= arrivalDistance)
        {
            // 콘솔창에 로그 출력
            Debug.Log("종점 도착!!!!");

            // 이 스크립트가 붙어있는 자기 자신(Enemy 게임 오브젝트)을 파괴합니다.
            Destroy(gameObject);
        }
    }
}