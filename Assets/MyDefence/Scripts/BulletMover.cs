using UnityEngine;

public class BulletMover : MonoBehaviour
{
    [Header("탄환 설정")]
    [SerializeField] private float speed = 70.0f; // 이동 속도
    [SerializeField] private float hitDistance = 0.2f; // 타격 판정 거리

    [Header("이펙트 설정")]
    [SerializeField] private GameObject hitEffectPrefab; // 타격 파티클 프리팹

    private Transform target; // 쫓아갈 타겟 에너미

    // 터렛이 탄환을 스폰할 때 타겟을 쥐여주는 함수
    public void SetTarget(Transform enemyTarget)
    {
        target = enemyTarget;
    }

    void Update()
    {
        // 만약 도중에 타겟(에너미)이 다른 탄환에 맞아 먼저 죽었다면, 이 탄환도 스스로 소멸합니다.
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        // 1. 타겟을 향해 날아가기 (유도탄 로직)
        Vector3 direction = target.position - transform.position;
        Vector3 moveDir = direction.normalized;
        transform.Translate(moveDir * speed * Time.deltaTime, Space.World);

        // 2. [타겟 충돌 체크] 거리를 가지고 타격 판정
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        // 설정한 타격 거리보다 가까워졌다면? (Hit!)
        if (distanceToTarget <= hitDistance)
        {
            HitTarget();
        }
    }

    void HitTarget()
    {
        Debug.Log("Hit Target!!!");

        // 4단계: 타격 이펙트(파티클) 생성
        if (hitEffectPrefab != null)
        {
            GameObject effectGo = Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);

            // 생성된 타격 이펙트 오브젝트를 1초 뒤에 kill(Destroy) 합니다.
            Destroy(effectGo, 1.0f);
        }

        // 타겟(Enemy)과 탄환(자신) 게임오브젝트를 kill(Destroy) 합니다.
        Destroy(target.gameObject); // 적 파괴
        Destroy(gameObject);        // 탄환 파괴
    }
}