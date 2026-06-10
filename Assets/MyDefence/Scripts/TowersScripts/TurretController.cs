// 경로: Assets/MyDefence/Scripts/TowersScripts/TurretController.cs
using UnityEngine;

namespace MyDefence
{
    /// <summary>
    /// 공통 타워(Tower) 기능을 상속받아 연쇄 사격 버그를 완벽히 해결한 머신건 타워 클래스
    /// </summary>
    public class TurretController : Tower
    {
        #region Variables
        [Header("머신건 발사 설정")]
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform firePoint;
        [SerializeField] private float fireRate = 1.5f;    // 발사 간격 (1.5초)

        private float fireCooldown = 0f;                   // 실제 발사 제어용 쿨타임 변수
        private Transform lockedTarget = null;             // 내가 고정한 타겟
        #endregion

        #region Unity Event Methods
        protected override void Update()
        {
            // 1. 매 프레임 독립적으로 쿨타임 타임라인 차감
            if (fireCooldown > 0f)
            {
                fireCooldown -= Time.deltaTime;
            }

            // 2. 부모(Tower)의 기본 추적 및 조준 기능 실행
            base.Update();
        }
        #endregion

        #region Overridden Methods
        /// <summary>
        /// 부모의 조준 대상을 결정하는 함수를 오버라이드하여, 내가 락온한 적이 사정거리 안에 살아있다면 조준 고정
        /// </summary>
        protected override Transform GetCurrentLookTarget()
        {
            if (lockedTarget != null)
            {
                // 고정한 적이 아직 사정거리 안에 있는지 검사
                float distance = Vector3.Distance(transform.position, lockedTarget.position);
                if (distance <= attackRange)
                {
                    return lockedTarget;
                }
                else
                {
                    // 사정거리를 벗어나면 고정 해제
                    lockedTarget = null;
                }
            }
            return base.GetCurrentLookTarget();
        }

        /// <summary>
        /// 부모가 매 프레임 호출해 주는 공격 함수. 코루틴 없이 매 프레임 타겟 생사를 정확히 확인합니다.
        /// </summary>
        protected override void Attack()
        {
            // 1. 현재 고정된 타겟이 없다면, 부모가 새로 포착한 가장 가까운 적을 내 고정 타겟으로 등록합니다.
            if (lockedTarget == null && target != null)
            {
                lockedTarget = target;
            }

            // 2. 고정 타겟이 확정되었고, 쿨타임이 0 이하로 다 찼다면 사격 실행!
            if (lockedTarget != null && fireCooldown <= 0f)
            {
                ShootBullet();
                fireCooldown = fireRate; // 1.5초 쿨타임 정확하게 작동!
            }
        }

        protected override void OnTargetLost()
        {
            // 부모가 타겟을 완전히 잃었을 때, 내가 수동으로 패던 고정 타겟도 없다면 초기화
            if (target == null)
            {
                lockedTarget = null;
            }
        }
        #endregion

        #region Custom Methods
        /// <summary>
        /// 코루틴 엇박자 없이 즉시 한 발을 안전하게 스폰하는 사격 함수
        /// </summary>
        private void ShootBullet()
        {
            if (bulletPrefab == null || firePoint == null || lockedTarget == null) return;

            Debug.Log($"🎯 [MACHINEGUN] 단일 타겟 사격: {lockedTarget.name} (다음 발사 대기: {fireRate}초)");

            // 총구 위치와 회전값으로 탄환 객체 생성
            GameObject bulletGo = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            // 탄환에게 락온한 타겟 주입
            Bullet bullet = bulletGo.GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.SetTarget(lockedTarget);
            }
        }
        #endregion
    }
}