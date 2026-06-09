// 경로: Assets/MyDefence/Scripts/ProjectilesScripts/Bullet.cs
using UnityEngine;

namespace MyDefence
{
    /// <summary>
    /// 모든 직사형 탄환의 기준이 되는 부모 클래스
    /// </summary>
    public class Bullet : MonoBehaviour
    {
        #region Variables
        protected Transform target;
        [SerializeField] protected float moveSpeed = 50f;

        [Header("타격 및 공격력 설정")]
        [SerializeField] protected float attackDamage = 50f; // [수정] 호환성을 위해 float로 변경!
        [SerializeField] protected GameObject impactEffect;
        #endregion

        #region Unity Events Methods
        protected virtual void Update()
        {
            if (target == null)
            {
                Destroy(gameObject);
                return;
            }

            Vector3 dir = target.position - transform.position;
            if (dir != Vector3.zero) transform.LookAt(target);

            float distanceThisFrame = moveSpeed * Time.deltaTime;

            // 이번 프레임에 적에게 도달했다면 타격 처리
            if (dir.magnitude <= distanceThisFrame)
            {
                HitTarget();
                return;
            }

            transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        }
        #endregion

        #region Custom Methods
        public void SetTarget(Transform _target)
        {
            target = _target;
        }

        /// <summary>
        /// 적과 충돌했을 때 이펙트를 뿜고 한 방 대미지를 준 뒤 사라지는 함수
        /// </summary>
        protected virtual void HitTarget()
        {
            if (impactEffect != null)
            {
                GameObject effectGo = Instantiate(impactEffect, transform.position, Quaternion.identity);
                Destroy(effectGo, 0.6f);
            }

            // 개편된 float형 TakeDamage 함수를 안정적으로 호출합니다.
            Enemy enemy = target.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(attackDamage);
            }

            Destroy(gameObject);
        }
        #endregion
    }
}