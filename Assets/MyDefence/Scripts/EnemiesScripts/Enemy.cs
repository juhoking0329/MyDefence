using UnityEngine;

namespace MyDefence
{
    /// <summary>
    /// 적의 체력, 피격, 사망 및 보상 지급을 관리하는 클래스
    /// </summary>
    public class Enemy : MonoBehaviour
    {
        #region Variables
        [Header("적 능력치 설정")]
        [SerializeField] private float hp = 100f;          // 실시간 소수점 데미지를 받기 위해 float 유지
        [SerializeField] private int rewardGold = 50;

        [Header("사망 효과")]
        [SerializeField] private GameObject deathEffectPrefab;
        #endregion

        #region Custom Methods
        /// <summary>
        /// 머신건, 미사일, 레이저 등 어떤 타워에 맞든 묵묵히 정밀하게 피만 깎아주는 공용 함수
        /// </summary>
        public void TakeDamage(float damage)
        {
            // 실시간 데미지 차감
            hp -= damage;

            // 체력이 0 이하가 되면 사망 처리
            if (hp <= 0f)
            {
                Die();
            }
        }

        /// <summary>
        /// 적이 죽을 때 호출되는 보상 지급 및 이펙트 처리 함수
        /// </summary>
        private void Die()
        {
            // 1. 리워드 골드 지급
            GameData.money += rewardGold;
            Debug.Log($"처치! {rewardGold} Gold 획득! 현재 소지금: {GameData.money} Gold");

            // 2. 사망 파티클 이펙트 생성
            if (deathEffectPrefab != null)
            {
                GameObject effect = Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
                Destroy(effect, 2f);
            }

            // 3. 본체 파괴
            Destroy(gameObject);
        }
        #endregion
    }
}