using UnityEngine;

namespace MyDefence
{
    public class TargetGuidedMissile : MonoBehaviour
    {
        #region Variables
        [Header("미사일 수치 설정 (과제 2, 4번)")]
        [SerializeField] private float speed = 50f;          // 이동 속도 50
        [SerializeField] private float damageRange = 3.5f;   // 스플래시 범위 반경 3.5

        [Header("타격 시각 효과 (과제 6번)")]
        [SerializeField] private GameObject explosionEffectPrefab; // 파티클+조명이 합쳐진 폭발 이펙트 프리팹

        private Transform target;
        #endregion

        #region Unity Event Methods
        void Update()
        {
            if (target == null)
            {
                Destroy(gameObject); // 타겟이 사라지면 미사일도 파괴
                return;
            }

            // 타겟을 향해 날아가기
            Vector3 dir = target.position - transform.position;
            float distanceThisFrame = speed * Time.deltaTime;

            // 이번 프레임에 적에게 도달했다면 타격 처리
            if (dir.magnitude <= distanceThisFrame)
            {
                HitTarget();
                return;
            }

            // 적 방향으로 전진 및 회전
            transform.Translate(dir.normalized * distanceThisFrame, Space.World);
            transform.LookAt(target);
        }

        // 과제 5) 미사일의 폭발 범위(3.5)를 기즈모 선으로 표시
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.orange;
            Gizmos.DrawWireSphere(transform.position, damageRange);
        }
        #endregion

        #region Custom Methods
        /// <summary>
        /// 런처에서 미사일을 쏠 때 타겟 정보를 넘겨받는 함수입니다.
        /// </summary>
        public void SetTarget(Transform _target)
        {
            target = _target;
        }

        /// <summary>
        /// 과제 4-2) 타격 지점 반경 3.5 안의 모든 Enemy를 감지하여 파괴하고 이펙트를 생성합니다.
        /// </summary>
        private void HitTarget()
        {
            // [업그레이드] 미사일 전용 거대 폭발 로그 출력!
            Debug.Log($"💥 [MISSILE] 미사일이 {target.name}에 명중하여 대폭발을 일으켰습니다!!! 💥");

            // 이펙트 스폰 (과제 6번 연동)
            if (explosionEffectPrefab != null)
            {
                GameObject effect = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
                Destroy(effect, 2f); // 2초 뒤 이펙트 자동 삭제
            }

            // Physics.OverlapSphere로 가상의 구체를 그려 충돌한 콜라이더 수집
            Collider[] colliders = Physics.OverlapSphere(transform.position, damageRange);

            int killedCount = 0; // 몇 마리를 처치했는지 카운트할 변수

            foreach (Collider collider in colliders)
            {
                // 범위 안의 오브젝트 중 태그가 "Enemy"인 녀석들만 골라내기
                if (collider.CompareTag("Enemy"))
                {
                    killedCount++; // 적을 찾을 때마다 숫자 1씩 증가
                    Destroy(collider.gameObject);
                }
            }

            // [업그레이드] 폭발 범위 내에서 처치된 적의 총 마릿수를 콘솔에 시원하게 출력!
            Debug.Log($"🔥 [SPLASH DAMAGE] 폭발 반경 {damageRange}m 이내의 적 {killedCount}마리를 섬멸(Kill)하였습니다!");

            // 범위 안의 적들을 다 처리했으니 미사일 본체는 파괴
            Destroy(gameObject);
        }
        #endregion
    }
}