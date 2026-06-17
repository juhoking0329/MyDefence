// 경로: Assets/MyDefence/Scripts/TowersScripts/LaserTurretController.cs
using System;
using UnityEngine;

namespace MyDefence
{
    public class LaserTurretController : Tower
    {
        [Header("레이저 설정")]
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private Transform firePoint;
        [SerializeField] private float tickDamage = 50f;

        [SerializeField] private float damageRampSpeed = 0.5f; // 1초당 늘어날 배율 (0.5면 1초마다 50%씩 강해짐)
        [SerializeField] private float maxDamageMultiplier = 3f; // 최대 증폭 제한 (최대 3배)
        private float currentDamageMultiplier = 1f; // 현재 데미지 배율 (기본 1배)

        [Header("레이저 타격 이펙트 설정")]
        [SerializeField] private GameObject impactEffectPrefab; // [★추가] 방금 만든 LaserImpact 프리팹 슬롯
        private GameObject spawnedImpactEffect;                  // [★추가] 현재 월드에 켜져 있는 이펙트 인스턴스

        // 공격 개시 로그를 딱 1번만 찍기 위한 추적 플래그 변수 추가
        private bool isAttacking = false;

        // 타겟이 중간에 사라졌을 때 에러 폭사를 막고 부모의 조준을 안전하게 호출하는 Update문 추가
        new void Update()
        {
            if (target == null)
            {
                OnTargetLost();
                return;
            }

            // 부모(Tower)의 조준 함수에 현재 타겟을 명확히 찔러 넣어줍니다.
            LockOnTarget(target);

            // 타겟이 살아있으니 공격 메서드 가동
            Attack();
        }

        protected override void Attack()
        {
            if (target == null) return;

            if (!lineRenderer.enabled) lineRenderer.enabled = true;

            // 매 프레임 연사되던 로그를 최초 1회만 찍히도록 가두었습니다.
            if (!isAttacking)
            {
                isAttacking = true;
                Debug.Log("레이저 공격 중...");
            }

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
                // 1. 매 프레임 실시간 감속 부여
                enemy.ApplySlow(0.4f);

                // 2. 데미지 증폭 배율 상승
                currentDamageMultiplier += damageRampSpeed * Time.deltaTime;
                if (currentDamageMultiplier > maxDamageMultiplier)
                {
                    currentDamageMultiplier = maxDamageMultiplier;
                }

                // 1초 타이머 데미지 처리
                // 1초를 기다리지 않고, 현재 프레임에 흘러간 시간(Time.deltaTime)만큼 
                // 데미지를 아주 잘게 쪼개서 매 프레임 가합니다!
                // 공식: (기본 1초당 데미지 * 현재 증폭 배율) * 현재 프레임 시간
                float finalDamage = (tickDamage * currentDamageMultiplier) * Time.deltaTime;

                // 적에게 실시간 데미지 전달!
                enemy.TakeDamage(finalDamage);

            }
        }


        protected override void OnTargetLost()
        {
            if (lineRenderer.enabled) lineRenderer.enabled = false;
            currentDamageMultiplier = 1f; // 배율 초기화!

            // [★이펙트 소멸] 적을 놓치거나 적이 죽었다면 지지직거리는 피격 이펙트도 즉시 청소합니다.
            ClearImpactEffect();

            // ★ [대장님 요청 수정분 4] 타겟을 놓쳤으니 다음 적을 만났을 때 로그를 다시 찍을 수 있도록 복구합니다.
            isAttacking = false;
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