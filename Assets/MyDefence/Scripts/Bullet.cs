using UnityEngine;

namespace MyDefence
{
    /// <summary>
    /// 모든 발사체의 기준이 되는 부모 클래스
    /// </summary>
    public class Bullet : MonoBehaviour
    {
        #region Variables
        // protected로 선언해야 상속받은 자식(미사일)이 이 변수를 자유롭게 가져다 씁니다.
        protected Transform target;          // 이동 목표 오브젝트의 트랜스폼 인스턴스
        [SerializeField] protected float moveSpeed = 50f; // 탄환 이동 속도

        [Header("타격 효과")]
        [SerializeField] protected GameObject impactEffect; // 타격 효과 프리팹
        #endregion

        #region Unity Events Methods
        // 자식이 Update를 따로 구현하지 않으면 부모의 Update가 자동으로 실행됩니다.
        // 만약 자식에서 Update를 입맛대로 바꾸고 싶다면 virtual을 붙여줍니다.
        protected virtual void Update()
        {
            // 타겟 검증
            if (target == null)
            {
                Destroy(gameObject);
                return;
            }

            // 타겟을 향해 이동
            Vector3 dir = target.position - transform.position;

            // 이동 처리 및 회전 (미사일에서도 자연스럽게 쓰이도록 방향 전환 코드 추가)
            if (dir != Vector3.zero)
            {
                transform.LookAt(target);
            }

            float distanceThisFrame = moveSpeed * Time.deltaTime;

            // 충돌 체크
            if (dir.magnitude <= distanceThisFrame)
            {
                HitTarget();
                return;
            }

            transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        }
        #endregion

        #region Custom Methods
        // 타겟 설정하기
        public void SetTarget(Transform _target)
        {
            target = _target;
        }

        /// <summary>
        /// 타겟 충돌 처리 (자식이 이 내용을 덮어쓸 수 있도록 virtual을 붙입니다.)
        /// </summary>
        protected virtual void HitTarget()
        {
            // 일반 탄환 타격 효과 생성
            if (impactEffect != null)
            {
                GameObject effectGo = Instantiate(impactEffect, transform.position, Quaternion.identity);
                Destroy(effectGo, 0.6f);
            }

            // 타겟, 탄환 게임오브젝트 kill
            Destroy(target.gameObject);
            Destroy(gameObject);
        }
        #endregion
    }
}