using UnityEngine;

namespace MyDefence
{
    /// <summary>
    /// Bullet 부모 클래스를 상속받아 광역 스플래시 데미지를 주는 미사일 클래스
    /// </summary>
    public class TargetGuidedMissile : Bullet
    {
        #region Variables
        [Header("미사일 전용 수치 설정")]
        [SerializeField] private float damageRange = 3.5f;   // 스플래시 범위 반경 3.5
        #endregion

        #region Unity Event Methods
        // Update()는 부모(Bullet)에 이미 완벽하게 구현되어 있으므로 
        // 굳시 새로 적지 않아도 부모의 기능을 그대로 물려받아 자동 작동합니다!

        // 과제 5) 미사일의 폭발 범위(3.5)를 기즈모 선으로 표시
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.orange;
            Gizmos.DrawWireSphere(transform.position, damageRange);
        }
        #endregion

        #region Custom Methods
        /// <summary>
        /// 부모의 HitTarget() 메서드를 미사일 전용 광역 폭발 로직으로 덮어씁니다(Override).
        /// </summary>
        protected override void HitTarget()
        {
            // [업그레이드] 미사일 전용 거대 폭발 로그 출력!
            Debug.Log($"💥 [MISSILE] 미사일이 {target.name}에 명중하여 대폭발을 일으켰습니다!!! 💥");

            // 부모의 impactEffect 변수를 그대로 활용하여 폭발 이펙트 생성
            if (impactEffect != null)
            {
                GameObject effect = Instantiate(impactEffect, transform.position, Quaternion.identity);
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