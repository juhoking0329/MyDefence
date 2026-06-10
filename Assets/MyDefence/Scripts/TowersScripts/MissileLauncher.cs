// 경로: Assets/MyDefence/Scripts/TowersScripts/MissileLauncher.cs
using UnityEngine;

namespace MyDefence
{
    /// <summary>
    /// 공통 타워(Tower) 기능을 상속받아 주기적으로 강력한 유도 미사일을 발사하는 클래스
    /// </summary>
    public class MissileLauncher : Tower // [★변경] MonoBehaviour -> Tower 상속
    {
        #region Variables
        [Header("미사일 발사 설정")]
        [SerializeField] private float fireRate = 4f;       // 발사 간격 (4초에 1회)
        [SerializeField] private GameObject missilePrefab;  // 발사할 미사일 프리팹
        [SerializeField] private Transform firePoint;       // 미사일이 나갈 총구 위치

        private float fireCooldown = 0f;                    // 발사 쿨타임 계산용 변수
        #endregion

        #region Unity Event Methods
        protected override void Start()
        {
            // [중요] 부모(Tower)의 Start를 호출하여 0.2초 타겟 탐색 루틴을 활성화합니다.
            // 기존의 0.5초 주기가 부모의 0.2초 주기로 업그레이드되어 타겟팅이 훨씬 빠릿해집니다!
            base.Start();
        }

        protected override void Update()
        {
            // 쿨타임 연산은 매 프레임 독립적으로 돕니다.
            fireCooldown -= Time.deltaTime;

            // [핵심] 부모의 Update(조준 및 공격 검사)를 실행시킵니다.
            base.Update();
        }
        #endregion

        #region Overridden Methods
        /// <summary>
        /// 부모인 Tower가 매 프레임 조준 완료 후 사정거리 조건이 맞을 때 실행해주는 공격 함수
        /// </summary>
        protected override void Attack()
        {
            // 쿨타임이 다 찼다면 미사일 슛!
            if (fireCooldown <= 0f)
            {
                Shoot();
                fireCooldown = fireRate; // 쿨타임 리셋
            }
        }
        #endregion

        #region Custom Methods
        /// <summary>
        /// 미사일을 스폰하고 부모가 찾은 target 정보를 주입하여 발사합니다.
        /// </summary>
        private void Shoot()
        {
            if (missilePrefab == null || firePoint == null) return;

            Debug.Log("미사일 발사!!");

            GameObject missileGO = Instantiate(missilePrefab, firePoint.position, firePoint.rotation);
            TargetGuidedMissile missile = missileGO.GetComponent<TargetGuidedMissile>();

            if (missile != null)
            {
                // 부모의 target 변수를 미사일에 그대로 매핑합니다.
                missile.SetTarget(target);
            }
        }
        #endregion
    }
}