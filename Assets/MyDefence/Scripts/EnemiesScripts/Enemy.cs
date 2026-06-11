// 경로: Assets/MyDefence/Scripts/EnemiesScripts/Enemy.cs
using UnityEngine;

namespace MyDefence
{
    public class Enemy : MonoBehaviour
    {
        [Header("적 능력치 설정")]
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private float baseSpeed = 5f;

        private float laserHitTimer = 0f;          // 레이저 피격 누적 시간
        private bool isSlowed = false;             // 현재 감속 중인지

        private float currentHealth;
        private float currentSpeed;
        private bool isSlowedThisFrame = false; // 이번 프레임에 레이저를 맞았는지 체크

        public float GetCurrentSpeed()
        {
            return currentSpeed;
        }

        void Start()
        {
            currentHealth = maxHealth;
            currentSpeed = baseSpeed;
        }

        void Update()
        {
            // --- 적 이동 로직 (기존 코드 유지) ---
            // 예: transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);

            // [0번 버그 해결] 이번 프레임에 레이저 공격을 안 받았다면 속도를 서서히 원래대로 복구
            if (!isSlowedThisFrame)
            {
                laserHitTimer = 0f;   // 레이저 안 맞으면 누적 시간 초기화
                isSlowed = false;      // 감속 상태 해제
                currentSpeed = Mathf.MoveTowards(currentSpeed, baseSpeed, Time.deltaTime * 5f);
            }

            // 프레임 종료 전 감속 체크 초기화
            isSlowedThisFrame = false;

            // [2번 버그 해결] 적이 End 지점에 도달했는지 검사 (거리나 조건에 맞게 수치 조절 필요)
            // 여기서는 예시로 Z축 특정 위치나 목적지 도달 조건을 체크합니다.
            // if (목적지 도달 조건) { ReachEnd(); }
        }

        // [0번 버그 해결] 레이저 타워가 매 프레임 호출해줄 감속 함수
        public void ApplySlow(float slowPercent)
        {
            isSlowedThisFrame = true;
            laserHitTimer += Time.deltaTime;       // 피격 시간 누적

            if (laserHitTimer >= 1f && !isSlowed) // 1초 이상 맞았을 때 감속 적용
            {
                isSlowed = true;
                currentSpeed = baseSpeed * (1f - slowPercent);
            }
        }

        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            GameData.money += 50;
            Destroy(gameObject);
        }

        /// <summary>
        /// [2번 버그 해결] 적이 기지(종점)에 도달했을 때 실행되는 함수
        /// </summary>
        public void ReachEnd()
        {
            if (GameData.lives > 0)
            {
                GameData.lives--; // 라이프 1 차감 (실시간 반영)
                Debug.Log($"💥 적 침입! 남은 라이프: {GameData.lives}");

                // 만약 라이프가 0이 되면 즉시 게임오버 트리거
                if (GameData.lives <= 0)
                {
                    UIManager.instance.TriggerGameOver();
                }
            }
            Destroy(gameObject); // 적 오브젝트 파괴
        }
    }
}