// 경로: Assets/MyDefence/Scripts/TowersScripts/LaserTurretController.cs
using UnityEngine;

namespace MyDefence
{
    /// <summary>
    /// 라인 렌더러를 사용해 타겟에게 실시간으로 소수점(float) 지속 데미지를 입히는 레이저 타워 클래스
    /// </summary>
    [RequireComponent(typeof(LineRenderer))]
    public class LaserTurretController : MonoBehaviour
    {
        #region Variables
        [Header("터렛 회전 설정")]
        [SerializeField] private Transform turretHead;
        [SerializeField] private float attackRange = 7f;
        [SerializeField] private float rotationSpeed = 10f;

        [Header("레이저 설정")]
        [SerializeField] private Transform firePoint;     // 레이저 출발 지점 (총구)
        [SerializeField] private float damagePerSecond = 50f; // 초당 베이스 데미지

        private LineRenderer lineRenderer;
        private Transform target;

        // [구조 개편] "레이저 공격 중" 로그가 중복으로 쏟아지는 것을 막기 위한 내부 스위치
        private bool isLogging = false;
        #endregion

        #region Unity Event Methods
        void Start()
        {
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.enabled = false; // 게임 시작 시에는 레이저를 꺼둡니다.

            // 성능 최적화를 위해 0.2초마다 타겟을 탐색합니다.
            InvokeRepeating("UpdateTarget", 0f, 0.2f);
        }

        void Update()
        {
            // 타겟이 사정거리를 벗어나거나 죽으면 레이저와 로그 스위치를 끄고 조기 종료
            if (target == null)
            {
                if (lineRenderer.enabled) lineRenderer.enabled = false;
                return;
            }

            // 1. 타겟 부드럽게 조준 회전
            LockOnTarget();

            // 2. 실시간 레이저 광선 발사 및 대미지 주입
            ShootLaser();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
        #endregion

        #region Custom Methods
        /// <summary>
        /// 범위 내에 있는 가장 가까운 적을 탐색하는 함수
        /// </summary>
        private void UpdateTarget()
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            float shortestDistance = Mathf.Infinity;
            GameObject nearestEnemy = null;

            foreach (GameObject enemy in enemies)
            {
                if (enemy == null) continue;

                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = enemy;
                }
            }

            // 가장 가까운 적이 사정거리 안에 들어왔을 때
            if (nearestEnemy != null && shortestDistance <= attackRange)
            {
                target = nearestEnemy.transform;
            }
            else
            {
                // 타겟을 잃어버리거나 적이 없다면 타겟과 로그 스위치를 모두 리셋합니다.
                target = null;
                isLogging = false;
            }
        }

        /// <summary>
        /// 타겟의 X, Z축 평면을 기준으로 대가리를 부드럽게 회전시키는 조준 함수
        /// </summary>
        private void LockOnTarget()
        {
            Vector3 dir = target.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(turretHead.rotation, lookRotation, Time.deltaTime * rotationSpeed).eulerAngles;
            turretHead.rotation = Quaternion.Euler(0f, rotation.y, 0f); // Y축 회전만 적용하여 수평 조준 유지
        }

        /// <summary>
        /// 라인 렌더러를 갱신하고, 주체에 맞게 '레이저 타워' 스크립트 내부에서 깔끔하게 로그를 처리하는 발사 함수
        /// </summary>
        private void ShootLaser()
        {
            if (!lineRenderer.enabled) lineRenderer.enabled = true;

            // 라인 렌더러의 두 점을 실시간 매핑 (0번: 총구위치, 1번: 적 몸통위치)
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, target.position + Vector3.up * 0.5f);

            // [주체 확립] 타워가 직접 지질 때 여기서 딱 한 줄만 로그를 출력합니다!
            if (!isLogging)
            {
                isLogging = true;
                Debug.Log("레이저 공격 중...");
            }

            Enemy enemy = target.GetComponent<Enemy>();
            if (enemy != null)
            {
                // 프레임 누수 없이 완벽한 정밀 소수점 데미지를 던집니다.
                enemy.TakeDamage(damagePerSecond * Time.deltaTime);
            }
        }
        #endregion
    }
}