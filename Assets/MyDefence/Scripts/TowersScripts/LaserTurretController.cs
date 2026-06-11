// 경로: Assets/MyDefence/Scripts/TowersScripts/LaserTurretController.cs
using UnityEngine;

namespace MyDefence
{
    public class LaserTurretController : Tower
    {
        [Header("레이저 설정")]
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private Transform firePoint;
        [SerializeField] private float tickDamage = 50f;

        private float damageTimer = 0f;

        [Header("레이저 타격 이펙트 설정")]
        [SerializeField] private GameObject impactEffectPrefab; // [★추가] 방금 만든 LaserImpact 프리팹 슬롯
        private GameObject spawnedImpactEffect;                  // [★추가] 현재 월드에 켜져 있는 이펙트 인스턴스

        protected override void Attack()
        {
            if (target == null) return;

            if (!lineRenderer.enabled) lineRenderer.enabled = true;

            // 레이저 끝점을 적의 가슴 위치(+Vector3.up * 0.5f)로 지정하셨으므로, 이펙트도 이 위치를 추적합니다.
            Vector3 targetImpactPosition = target.position;

            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, targetImpactPosition);

            // [★이펙트 실시간 처리부]
            if (impactEffectPrefab != null)
            {
                // 현재 맵에 생성된 이펙트가 없다면 처음 지지기 시작한 순간이므로 새로 생성
                if (spawnedImpactEffect == null)
                {
                    spawnedImpactEffect = Instantiate(impactEffectPrefab, targetImpactPosition, Quaternion.identity);
                }
                else
                {
                    // 이미 켜져 있다면, 적이 움직이는 동선(가슴 위치)을 따라 매 프레임 위치 동기화
                    spawnedImpactEffect.transform.position = targetImpactPosition;

                    // 레이저가 날아오는 반대 방향(타워 쪽)을 바라보도록 이펙트 회전각 조절 (스파크가 이쁘게 튀기 위함)
                    Vector3 dirToTower = firePoint.position - targetImpactPosition;
                    if (dirToTower != Vector3.zero)
                    {
                        spawnedImpactEffect.transform.rotation = Quaternion.LookRotation(dirToTower);
                    }
                }
            }

            Enemy enemy = target.GetComponent<Enemy>();
            if (enemy != null)
            {
                // [0번 버그 해결] 매 프레임 실시간으로 40% 감속을 부여합니다.
                enemy.ApplySlow(0.4f);

                // 1초 타이머 데미지 처리
                damageTimer += Time.deltaTime;
                if (damageTimer >= 1f)
                {
                    enemy.TakeDamage(tickDamage);
                    damageTimer = 0f;
                }
            }
        }

        protected override void OnTargetLost()
        {
            if (lineRenderer.enabled) lineRenderer.enabled = false;
            damageTimer = 0f;

            // [★이펙트 소멸] 적을 놓치거나 적이 죽었다면 지지직거리는 피격 이펙트도 즉시 청소합니다.
            ClearImpactEffect();
        }

        private void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        // [★추가] 타워가 파괴되거나 비활성화될 때 잔여 이펙트가 맵에 쓰레기로 남지 않도록 청소하는 안전장치
        private void OnDisable()
        {
            ClearImpactEffect();
        }

        /// <summary>
        /// 켜져 있는 타격 이펙트를 안전하게 파괴하고 변수를 비우는 함수
        /// </summary>
        private void ClearImpactEffect()
        {
            if (spawnedImpactEffect != null)
            {
                Destroy(spawnedImpactEffect);
                spawnedImpactEffect = null;
            }
        }
    }
}