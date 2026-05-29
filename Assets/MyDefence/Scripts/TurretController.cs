using System.Collections;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    [Header("터렛 설정")]
    [SerializeField] private Transform turretHead;
    [SerializeField] private float attackRange = 7.0f;
    [SerializeField] private float rotationSpeed = 10.0f;

    [Header("발사 설정")]
    [SerializeField] private GameObject bulletPrefab; // 탄환 프리팹
    [SerializeField] private Transform firePoint;     // 총구 위치 (FirePoint)
    [SerializeField] private float fireRate = 1.0f;    // 발사 간격 (1초)

    private Transform targetEnemy;
    private bool isShooting = false; // 현재 총을 쏘고 있는 중인가?

    void Update()
    {
        FindClosestEnemy();

        if (targetEnemy != null)
        {
            RotateTowardsTarget();

            // 타겟이 있고, 아직 사격 코루틴이 돌고 있지 않다면 사격을 시작합니다!
            if (!isShooting)
            {
                StartCoroutine(ShootRoutine());
            }
        }
    }

    // [핵심 스킬] 1초마다 1발씩 쏘는 코루틴 함수
    IEnumerator ShootRoutine()
    {
        isShooting = true;

        while (targetEnemy != null)
        {
            Debug.Log("Shoot!!!!!");

            // 1. 총구(FirePoint) 위치와 회전값으로 탄환 객체를 생성(Instantiate)합니다.
            GameObject bulletGo = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            // 2. [핵심 스킬] 생성된 탄환에게 "저 녀석을 쫓아가!"라며 타겟을 전달해 줍니다.
            BulletMover bullet = bulletGo.GetComponent<BulletMover>();
            if (bullet != null)
            {
                bullet.SetTarget(targetEnemy);
            }

            // 1초 동안 대기(쉬기)
            yield return new WaitForSeconds(fireRate);
        }

        isShooting = false; // 타겟이 사라지면 사격을 멈춥니다.
    }

    void FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closestEnemy = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            if (enemy == null) continue; // 이미 파괴된 에너미는 패스
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

            if (distanceToEnemy < minDistance)
            {
                minDistance = distanceToEnemy;
                closestEnemy = enemy;
            }
        }

        if (closestEnemy != null && minDistance <= attackRange)
        {
            targetEnemy = closestEnemy.transform;
        }
        else
        {
            targetEnemy = null;
        }
    }

    void RotateTowardsTarget()
    {
        Vector3 direction = targetEnemy.position - turretHead.position;
        direction.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        turretHead.rotation = Quaternion.Slerp(turretHead.rotation, lookRotation, rotationSpeed * Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}