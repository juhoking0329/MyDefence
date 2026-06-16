using UnityEngine;

namespace MyDefence
{
    /// <summary>
    /// 지정된 시간마다 반복적으로 파티클 이펙트 플레이 시켜주는 클래스
    /// </summary>
    public class IntervalParticlePlay : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particle;  // 재생할 파티클
        [SerializeField] private float interval = 1f;      // 반복 재생 간격 (초)
        [SerializeField] private float initialDelay = 0f;  // 처음 재생 전 딜레이 (초)

        private float timer = 0f;  // 경과 시간 누적

        private void Awake()
        {
            // 슬롯에 파티클이 없으면 자신의 컴포넌트에서 자동으로 찾기
            if (particle == null)
                particle = GetComponent<ParticleSystem>();

            // 딜레이만큼 타이머를 음수로 시작해서 첫 재생을 늦춤
            timer = -initialDelay;
        }

        private void Update()
        {
            timer += Time.deltaTime;

            // 경과 시간이 interval을 넘으면 파티클 재생 후 타이머 초기화
            if (timer >= interval)
            {
                particle.Play();
                timer = 0f;
            }
        }
    }
}