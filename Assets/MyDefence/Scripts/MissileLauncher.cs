using UnityEngine;

namespace MyDefence
{
    public class MissileLauncher : MonoBehaviour
    {
        #region Variables
        [Header("런처 설정 수치 (과제 1번)")]
        [SerializeField] private float attackRange = 12f;  // 공격 범위 (과제 수치 적용: 12)
        [SerializeField] private float fireRate = 4f;       // 발사 간격 (4초에 1회)

        [Header("발사체 및 발사구")]
        [SerializeField] private GameObject missilePrefab;  // 발사할 미사일 프리팹
        [SerializeField] private Transform firePoint;       // 미사일이 나갈 총구 위치

        private Transform target = null;                    // 현재 LockOn할 타겟 적
        private float fireCooldown = 0f;                    // 발사 쿨타임 계산용 변수
        #endregion

        #region Unity Event Methods
        void Start()
        {
            // 0.5초마다 한 번씩 가장 가까운 적을 찾도록 최적화 호출
            InvokeRepeating(nameof(UpdateTarget), 0f, 0.5f);
        }

        void Update()
        {
            // 쿨타임 돌리기
            fireCooldown -= Time.deltaTime;

            if (target == null) return;

            // 타겟 락온 (적을 바라보게 회전)
            LockOnTarget();

            // 4초 주기가 되었고 공격 범위 안에 적이 있다면 슛!
            if (fireCooldown <= 0f && Vector3.Distance(transform.position, target.position) <= attackRange)
            {
                Shoot();
                fireCooldown = fireRate; // 4초 쿨타임 리셋
            }
        }

        // 과제 5) 런처의 공격 범위를 에디터 씬 창에 선으로 표시
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
        #endregion

        #region Custom Methods
        /// <summary>
        /// 맵에 있는 "Enemy" 태그를 가진 적들 중 가장 가까운 타겟을 검색합니다.
        /// </summary>
        private void UpdateTarget()
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            float shortestDistance = Mathf.Infinity;
            GameObject nearestEnemy = null;

            foreach (GameObject enemy in enemies)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = enemy;
                }
            }

            // 가장 가까운 적이 공격 범위 안에 있을 때만 타겟으로 지정
            if (nearestEnemy != null && shortestDistance <= attackRange)
            {
                target = nearestEnemy.transform;
            }
            else
            {
                target = null;
            }
        }

        /// <summary>
        /// 타겟을 부드럽게 조준(LockOn)합니다.
        /// </summary>
        private void LockOnTarget()
        {
            Vector3 dir = target.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            // Y축 회전만 부드럽게 적용 (포탑 회전 느낌)
            Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 10f).eulerAngles;
            transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        }

        /// <summary>
        /// 미사일을 스폰하고 타겟 정보를 주입하여 발사합니다.
        /// </summary>
        private void Shoot()
        {
            if (missilePrefab == null || firePoint == null) return;

            GameObject missileGO = Instantiate(missilePrefab, firePoint.position, firePoint.rotation);
            TargetGuidedMissile missile = missileGO.GetComponent<TargetGuidedMissile>();

            if (missile != null)
            {
                missile.SetTarget(target);
            }
        }
        #endregion
    }
}