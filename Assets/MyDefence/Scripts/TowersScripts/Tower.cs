// 경로: Assets/MyDefence/Scripts/TowersScripts/Tower.cs
using UnityEngine;

namespace MyDefence
{
    /// <summary>
    /// 타워를 관리하는 클래스, 타워들의 공통기능을 가진 부모 클래스
    /// </summary>
    public class Tower : MonoBehaviour
    {
        #region Variables
        [Header("타워 기본 능력치")]
        [SerializeField] protected Transform turretHead;
        [SerializeField] protected float attackRange = 7f;
        [SerializeField] protected float rotationSpeed = 10f;

        // ★ [2단계 수정 핵심]: 모든 타워가 공통으로 가질 가격 데이터 변수 추가
        [Header("타워 경제 설정 (UI 연동용)")]
        [SerializeField] protected int upgradePrice; // 다음 단계 업그레이드 비용
        [SerializeField] protected int sellPrice;    // 판매 시 돌려받는 비용 (건설가의 반값!)

        // 자식 클래스나 UI 매니저가 가격을 안전하게 읽어갈 수 있도록 프로퍼티(Property) 개방
        public int UpgradePrice => upgradePrice;
        public int SellPrice => sellPrice;

        protected Transform target; // 부모가 탐색한 가장 가까운 적
        #endregion

        #region Unity Event Methods
        protected virtual void Start()
        {
            InvokeRepeating("UpdateTarget", 0f, 0.2f);
        }

        protected virtual void Update()
        {
            // [★구조 변경] 현재 조준해야 할 최종 타겟을 결정합니다.
            Transform currentLookTarget = GetCurrentLookTarget();

            if (currentLookTarget == null)
            {
                OnTargetLost();
                return;
            }

            // 부모가 탐색한 적이든, 자식이 고정한 적이든 결정된 대상을 향해 회전합니다.
            LockOnTarget(currentLookTarget);

            // 공격 실행
            Attack();
        }

        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
        #endregion

        #region Custom Methods
        protected void UpdateTarget()
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
        /// [★변경] 매개변수로 타겟을 받아 조준하도록 수정하여 자식의 고정 타겟 조준을 지원합니다.
        /// </summary>
        protected void LockOnTarget(Transform lookTarget)
        {
            Vector3 dir = lookTarget.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(turretHead.rotation, lookRotation, Time.deltaTime * rotationSpeed).eulerAngles;
            turretHead.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        }

        /// <summary>
        /// [★신규] 자식 타워가 조준 대상을 직접 제어할 수 있도록 징검다리를 열어둡니다.
        /// 기본적으로는 부모가 찾은 target을 반환합니다.
        /// </summary>
        protected virtual Transform GetCurrentLookTarget()
        {
            return target;
        }

        protected virtual void Attack() { }
        protected virtual void OnTargetLost() { }
        #endregion
    }
}