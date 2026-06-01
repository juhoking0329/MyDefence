using UnityEngine;

namespace MyDefence
{
    /// <summary>
    /// 탄환을 관리하는 클래스
    /// </summary>
    public class Bullet : MonoBehaviour
    {
        #region Variables
        private Transform target;          //이동 목표 오브젝트의 트랜스폼 인스턴스
        public float moveSpeed = 50f;          //탄환 이동 속도

        //타격 효과
        public GameObject impactEffect;    //타격 효과 프리팹
        #endregion

        #region Unity Events Methods
        private void Update()
        {
            //타겟 검증
            if (target == null)
            {
                Destroy(gameObject);
                return;
            }

            // 타겟을 향해 이동 : dir, Time.deltaTime, speed
            Vector3 dir = target.position - transform.position;
            //충돌 체크
            float distance = Vector3.Distance(transform.position, target.position);
            float distanceThisFrame = moveSpeed * Time.deltaTime;

            if (distance <= distanceThisFrame)
            {
                HitTarget();
                return;
            }
        }
        #endregion

        #region Custom Methods
        //타겟 설정하기
        public void SetTarget(Transform _target)
        {
            target = _target;
        }

        //타겟 충돌
        void HitTarget()
        {
            // 불렛이 적을 타격할때 불렛이 부서져서 파편이 날아가는 효과
            if (impactEffect != null)
            {
                GameObject effectGo = Instantiate(impactEffect, transform.position, Quaternion.identity);
                Destroy(effectGo, 3f); // 3초 후에 효과 파괴
            }

            //타겟, 탄환 게임오브젝트 kill(Destroy)
            //Debug.Log("Hit Target!!!");
            Destroy(target.gameObject); // 적 파괴
            Destroy(gameObject);        // 탄환 파괴
        }

        #endregion
    }
}