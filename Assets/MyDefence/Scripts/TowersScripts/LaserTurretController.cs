// 경로: Assets/MyDefence/Scripts/TowersScripts/LaserTurretController.cs
using UnityEngine;

namespace MyDefence
{
    /// <summary>
    /// 공통 타워(Tower) 기능을 상속받아 오직 '레이저 빔 연출 및 틱 대미지'만 담당하는 클래스
    /// </summary>
    [RequireComponent(typeof(LineRenderer))]
    public class LaserTurretController : Tower // [★핵심★] MonoBehaviour가 아닌 부모 Tower를 상속받습니다!
    {
        #region Variables
        [Header("레이저 고유 설정")]
        [SerializeField] private Transform firePoint;     // 레이저 출발 지점 (총구)
        [SerializeField] private float damagePerSecond = 50f; // 초당 베이스 데미지

        [Header("레이저 세부 설정")]
        [SerializeField] private float tickDamage = 30f; // 1초당 줄 데미지
        private float damageTimer = 0f;                   // 타이머 변수

        private LineRenderer lineRenderer;
        private bool isLogging = false; // 로그 중복 방지 스위치
        #endregion

        #region Unity Event Methods
        protected override void Start()
        {
            base.Start(); // [필수] 부모(Tower)의 Start 함수(적 탐색 인보크)를 먼저 실행합니다.

            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.enabled = false;
        }
        #endregion

        #region Overridden Methods (부모의 기능을 입맛대로 채우는 곳)
        /// <summary>
        /// 부모인 Tower가 매 프레임 조준 후 실행해 주는 공격 로직
        /// </summary>
        protected override void Attack()
        {
            if (!lineRenderer.enabled) lineRenderer.enabled = true;

            // 부모가 실시간으로 찾아준 'target' 변수를 그대로 가져다 씁니다.
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, target.position + Vector3.up * 0.5f);

            // 레이저 타워 스크립트 본연의 주체성에 맞는 독자적 로그 처리
            if (!isLogging)
            {
                isLogging = true;
                Debug.Log($"{target.gameObject.name} 레이저 공격 중...");
            }

            Enemy enemy = target.GetComponent<Enemy>();
            if (enemy != null)
            {
                // 1. [과제 반영] 속도 40% 감속 적용 (0.4f)
                enemy.ApplySlow(0.4f);

                // 2. [과제 반영] 1초 타이머 작동
                damageTimer += Time.deltaTime;
                if (damageTimer >= 1f)
                {
                    enemy.TakeDamage(tickDamage);
                    Debug.Log($"{tickDamage} 레이저 추가 데미지 공격 중...");
                    damageTimer = 0f; // 타이머 초기화
                }
            }
        }

        /// <summary>
        /// 부모인 Tower가 타겟이 없다고 판단했을 때 실행해 주는 예외 처리
        /// </summary>
        protected override void OnTargetLost()
        {
            // 타겟이 없으면 레이저 광선을 끄고 로그 스위치도 초기화합니다.
            if (lineRenderer.enabled) lineRenderer.enabled = false;
            isLogging = false;
            damageTimer = 0f; // 타겟 놓치면 타이머 초기화

            // [중요] 타겟을 놓쳤으므로 이전에 지지던 녀석의 속도를 원래대로 돌려놔야 하지만, 
            // 이미 타겟이 null이므로 안전하게 처리하기 위해 묵인하거나 트리거에서 해결합니다.
        }
        #endregion
    }
}