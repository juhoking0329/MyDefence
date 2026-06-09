using System.Collections;
using UnityEngine;

namespace MyDefence
{
    /// <summary>
    /// 가장 가까운 적을 탐색하고 부드럽게 조준하여 탄환을 발사하는 머신건 타워 클래스
    /// </summary>
    public class TurretController : MonoBehaviour
    {
        #region Variables
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
        #endregion

        #region Unity Event Methods
        void Update()
        {
            // 1. 매 프레임 가장 가까운 적을 탐색
            FindClosestEnemy();

            if (targetEnemy != null)
            {
                // 2. 적을 향해 고개 회전
                RotateTowardsTarget();

                // 3. 타겟이 있고, 아직 사격 코루틴이 돌고 있지 않다면 사격 시작
                if (!isShooting)
                {
                    StartCoroutine(ShootRoutine());
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
        #endregion

        #region Custom Methods / Coroutines
        /// <summary>
        /// 지정된 발사 간격(fireRate)마다 탄환을 생성하고 타겟을 주입하는 루틴
        /// </summary>
        IEnumerator ShootRoutine()
        {
            isShooting = true;

            while (targetEnemy != null)
            {
                Debug.Log("머신건 공격 중...");

                // 총구(FirePoint) 위치와 회전값으로 탄환 객체를 생성
                GameObject bulletGo = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

                // [수정 완료] 옛날 BulletMover 대신 새롭고 튼튼한 부모 클래스 Bullet을 컴포넌트로 가져옵니다.
                Bullet bullet = bulletGo.GetComponent<Bullet>();
                if (bullet != null)
                {
                    // 생성된 탄환에게 쫓아갈 타겟을 쥐여줍니다.
                    bullet.SetTarget(targetEnemy);
                }

                // 1초(fireRate) 동안 대기
                yield return new WaitForSeconds(fireRate);
            }

            isShooting = false; // 타겟이 사정거리를 벗어나거나 죽으면 사격 중지
        }

        /// <summary>
        /// 범위 내에 있는 가장 가까운 적을 찾아 targetEnemy에 등록하는 함수
        /// </summary>
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

        /// <summary>
        /// Y축 기준으로만 부드럽게 적을 조준하도록 터렛 대가리를 회전시키는 함수
        /// </summary>
        void RotateTowardsTarget()
        {
            Vector3 direction = targetEnemy.position - turretHead.position;
            direction.y = 0; // 상하 회전 방지

            Quaternion lookRotation = Quaternion.LookRotation(direction);
            turretHead.rotation = Quaternion.Slerp(turretHead.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }
        #endregion
    }
}