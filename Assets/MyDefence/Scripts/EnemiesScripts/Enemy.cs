// 경로: Assets/MyDefence/Scripts/EnemiesScripts/Enemy.cs
using UnityEngine;
using UnityEngine.UI;

namespace MyDefence
{
    public class Enemy : MonoBehaviour
    {
        [Header("적 능력치 설정 (과제 4번)")]
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private float baseSpeed = 5f;
        [SerializeField] private int rewardMoney = 50; // 과제 조건: 각 프리팹마다 다르게 세팅할 골드 보상

        [Header("이펙트 설정")]
        [SerializeField] private GameObject deathEffectPrefab;

        public Image healthBarImage;

        private float laserHitTimer = 0f;
        private bool isSlowed = false;

        private float currentHealth;
        private float currentSpeed;
        private bool isSlowedThisFrame = false;

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
            // [0번 버그 해결] 감속 복구 로직 유지
            if (!isSlowedThisFrame)
            {
                laserHitTimer = 0f;
                isSlowed = false;
                currentSpeed = Mathf.MoveTowards(currentSpeed, baseSpeed, Time.deltaTime * 5f);
            }

            isSlowedThisFrame = false;
        }

        public void ApplySlow(float slowPercent)
        {
            isSlowedThisFrame = true;
            laserHitTimer += Time.deltaTime;

            if (laserHitTimer >= 1f && !isSlowed)
            {
                isSlowed = true;
                currentSpeed = baseSpeed * (1f - slowPercent);
            }
        }

        public void TakeDamage(float damage)
        {
            currentHealth -= damage;

            if (healthBarImage != null)
                healthBarImage.fillAmount = (float)currentHealth / maxHealth;

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            // ★ [과제 4번/2번 반영] 기획서상의 보상 지급 및 SpawnManager에게 마리수 감소 알림
            GameData.money += rewardMoney;

            if (deathEffectPrefab != null)
            {
                // 적이 있던 현재 위치(transform.position)와 회전값에 파티클을 뿅! 하고 생성합니다.
                GameObject effectGo = Instantiate(deathEffectPrefab, transform.position, transform.rotation);

                // 생성된 이펙트 오브젝트를 2초 뒤에 메모리에서 자동 삭제
                Destroy(effectGo, 2f);
            }

            if (SpawnManager.instance != null)
            {
                SpawnManager.instance.OnEnemyDestroyed();
            }

            Destroy(gameObject);
        }

        public void ReachEnd()
        {
            if (GameData.lives > 0)
            {
                GameData.lives--;
                Debug.Log($"💥 적 침입! 남은 라이프: {GameData.lives}");

                // ★ [과제 2번 반영] 종점에 닿아 사라질 때도 SpawnManager에게 마리수 감소 알림
                if (SpawnManager.instance != null)
                {
                    SpawnManager.instance.OnEnemyDestroyed();
                }

                if (GameData.lives <= 0)
                {
                    UIManager.instance.TriggerGameOver();
                }
            }
            Destroy(gameObject);
        }
    }
}