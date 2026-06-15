// 경로: Assets/MyDefence/Scripts/TowersScripts/MissileLauncher.cs
using UnityEngine;

namespace MyDefence
{
    /// <summary>
    /// 단일 발사관과 다중 발사관(교차 사격)을 모두 완벽하게 지원하는 범용 미사일 타워 클래스
    /// </summary>
    public class MissileLauncher : Tower
    {
        #region Variables
        [Header("미사일 발사 설정")]
        [SerializeField] private float fireRate = 4f;       // 발사 간격 (4초에 1회)
        [SerializeField] private GameObject missilePrefab;  // 발사할 미사일 프리팹

        // ★ 핵심 개조 1: 단일 Transform에서 '배열(Transform[])' 구조로 변경!
        [SerializeField] private Transform[] firePoints;

        private float fireCooldown = 0f;                    // 발사 쿨타임 계산용 변수

        // ★ 핵심 개조 2: 이번에 몇 번째 발사관에서 미사일이 나갈지 기억하는 인덱스 스위치 변수
        private int currentFirePointIndex = 0;
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
                fireCooldown = fireRate;
            }
        }
        #endregion

        #region Custom Methods
        /// <summary>
        /// 등록된 발사관들을 순서대로 번갈아가며 미사일을 안전하게 스폰하는 사격 함수
        /// </summary>
        private void Shoot()
        {
            // 방어 코드: 미사일 프리팹이 없거나, 등록된 발사관이 아예 없으면 리턴
            if (missilePrefab == null || firePoints == null || firePoints.Length == 0) return;

            // 1. 현재 발사 순서에 맞는 총구를 배열에서 쏙 꺼내옵니다.
            Transform activeFirePoint = firePoints[currentFirePointIndex];

            // 안전장치: 혹시 인스펙터에서 실수로 슬롯을 비워뒀다면 리턴
            if (activeFirePoint == null) return;

            Debug.Log($"{currentFirePointIndex + 1}번 발사관에서 미사일 발사!!");

            // 2. 선택된 발사관의 위치와 회전값으로 미사일 생성
            GameObject missileGO = Instantiate(missilePrefab, activeFirePoint.position, activeFirePoint.rotation);
            TargetGuidedMissile missile = missileGO.GetComponent<TargetGuidedMissile>();

            if (missile != null)
            {
                // 부모의 target 변수를 미사일에 그대로 매핑합니다.
                missile.SetTarget(target);
            }

            // ★ 핵심 개조 3: 다음 발사를 위해 발사관 번호(Index)를 1 증가시키고 무한 뺑뺑이 조절!
            // 만약 발사관이 1개라면 항상 0번 슬롯만 사용하므로 하위 호환성도 100% 완벽합니다.
            currentFirePointIndex = (currentFirePointIndex + 1) % firePoints.Length;
        }
        #endregion
    }
}