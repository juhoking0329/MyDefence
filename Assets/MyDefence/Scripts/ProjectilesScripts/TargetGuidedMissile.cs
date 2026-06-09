// 경로: Assets/MyDefence/Scripts/ProjectilesScripts/TargetGuidedMissile.cs
using UnityEngine;

namespace MyDefence
{
    /// <summary>
    /// Bullet을 상속받아 타겟 도달 시 반경 내의 모든 적에게 방사형 float 대미지를 입히는 유도 미사일 클래스
    /// </summary>
    public class TargetGuidedMissile : Bullet
    {
        #region Variables
        [Header("미사일 전용 범위 설정")]
        [SerializeField] private float damageRange = 3.5f;
        #endregion

        #region Unity Event Methods
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.orange;
            Gizmos.DrawWireSphere(transform.position, damageRange);
        }
        #endregion

        #region Custom Methods
        /// <summary>
        /// 부모의 HitTarget을 오버라이딩(재정의)하여 가상 구체를 생성해 광역 대미지를 뿌립니다.
        /// </summary>
        protected override void HitTarget()
        {
            Debug.Log($"미사일 폭발!!");

            if (impactEffect != null)
            {
                GameObject effect = Instantiate(impactEffect, transform.position, Quaternion.identity);
                Destroy(effect, 2f);
            }

            // 가상의 물리 구체를 그려 범위 내에 걸린 모든 충돌체를 검출합니다.
            Collider[] colliders = Physics.OverlapSphere(transform.position, damageRange);

            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    Enemy enemy = collider.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        // 부모에게서 안전한 float로 변경된 대미지를 각각의 적들에게 전달!
                        enemy.TakeDamage(attackDamage);
                    }
                }
            }

            Destroy(gameObject); // 미사일 본체 소멸
        }
        #endregion
    }
}