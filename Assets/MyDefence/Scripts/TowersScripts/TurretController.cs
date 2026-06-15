// 경로: Assets/MyDefence/Scripts/TowersScripts/TurretController.cs
using UnityEngine;

namespace MyDefence
{
    /// <summary>
    /// 단일 총구와 다중 총구(교차 사격)를 모두 완벽하게 지원하는 범용 머신건 타워 클래스
    /// </summary>
    public class TurretController : Tower
    {
        #region Variables
        [Header("머신건 발사 설정")]
        [SerializeField] private GameObject bulletPrefab;

        // ★ 핵심 개조 1: 단일 Transform에서 '배열(Transform[])' 구조로 변경!
        [SerializeField] private Transform[] firePoints;
        [SerializeField] private float fireRate = 1.5f;    // 발사 간격 (1.5초)

        private float fireCooldown = 0f;                   // 실제 발사 제어용 쿨타임 변수
        private Transform lockedTarget = null;             // 내가 고정한 타겟

        // ★ 핵심 개조 2: 이번에 몇 번째 총구에서 나갈지 기억하는 인덱스 스위치 변수
        private int currentFirePointIndex = 0;
        #endregion

        #region Unity Event Methods
        protected override void Update()
        {
            if (fireCooldown > 0f)
            {
                fireCooldown -= Time.deltaTime;
            }

            base.Update();
        }
        #endregion

        #region Overridden Methods
        protected override Transform GetCurrentLookTarget()
        {
            if (lockedTarget != null)
            {
                float distance = Vector3.Distance(transform.position, lockedTarget.position);
                if (distance <= attackRange)
                {
                    return lockedTarget;
                }
                else
                {
                    lockedTarget = null;
                }
            }
            return base.GetCurrentLookTarget();
        }

        protected override void Attack()
        {
            if (lockedTarget == null && target != null)
            {
                lockedTarget = target;
            }

            if (lockedTarget != null && fireCooldown <= 0f)
            {
                ShootBullet();
                fireCooldown = fireRate;
            }
        }

        protected override void OnTargetLost()
        {
            if (target == null)
            {
                lockedTarget = null;
            }
        }
        #endregion

        #region Custom Methods
        /// <summary>
        /// 등록된 총구들을 순서대로 번갈아가며 탄환을 안전하게 스폰하는 사격 함수
        /// </summary>
        private void ShootBullet()
        {
            // 방어 코드: 총알 프리팹이 없거나, 등록된 총구가 아예 없으면 리턴
            if (bulletPrefab == null || firePoints == null || firePoints.Length == 0 || lockedTarget == null) return;

            // 1. 현재 순서에 맞는 총구를 배열에서 쏙 꺼내옵니다.
            Transform activeFirePoint = firePoints[currentFirePointIndex];

            // 안전장치: 혹시 인스펙터에서 실수로 슬롯을 비워뒀다면 리턴
            if (activeFirePoint == null) return;

            Debug.Log("머신건 공격 중...");

            // 2. 선택된 총구의 위치와 회전값으로 탄환 생성
            GameObject bulletGo = Instantiate(bulletPrefab, activeFirePoint.position, activeFirePoint.rotation);

            // 탄환에게 타겟 주입
            Bullet bullet = bulletGo.GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.SetTarget(lockedTarget);
            }

            // ★ 핵심 개조 3: 다음 발사를 위해 총구 번호(Index)를 1 증가시킵니다.
            // 나머지 연산자(%)를 사용하면 인덱스가 0 -> 1 -> 0 -> 1 로 알아서 무한 뺑뺑이를 돕니다!
            // 만약 총구가 1개라면 (0 + 1) % 1 = 0 이 되므로 항상 0번 총구만 씁니다. (완벽 호환!)
            currentFirePointIndex = (currentFirePointIndex + 1) % firePoints.Length;
        }
        #endregion
    }
}