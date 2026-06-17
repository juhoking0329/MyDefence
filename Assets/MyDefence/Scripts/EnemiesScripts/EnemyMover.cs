// 경로: Assets/MyDefence/Scripts/EnemiesScripts/EnemyMover.cs
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace MyDefence
{
    /// <summary>
    /// 에너미의 이동 및 웨이포인트 경로 추적을 관리하는 클래스입니다.
    /// </summary>
    public class EnemyMover : MonoBehaviour
    {
        // 인스펙터 청소 상태 유지 (코드 내부 변수로만 활용)
        private Transform targetTransform;

        [Header("도착 판단 기준 설정")]
        [SerializeField] private float arrivalDistance = 0.2f; // 웨이포인트를 스치고 지나치지 않도록 살짝 거리를 0.2f 정도로 잡는게 안정적입니다.

        private Enemy enemy; // Enemy 컴포넌트 참조

        // 현재 적이 추적 중인 웨이포인트의 배열 인덱스 번호
        private int wavePointIndex = 0;

        void Start()
        {
            enemy = GetComponent<Enemy>(); // 같은 오브젝트의 Enemy 컴포넌트 가져오기

            // ★ [요구사항 반영] 기존 "End" 오브젝트 자동 탐색 코드를 삭제하고,
            // 웨이포인트 클래스가 정상적으로 수집해 둔 첫 번째(0번) 포인트를 첫 목적지로 자동 세팅합니다.
            if (targetTransform == null && WayPoints.points != null && WayPoints.points.Length > 0)
            {
                wavePointIndex = 0;
                SetTarget(WayPoints.points[wavePointIndex]);
            }
        }

        /// <summary>
        /// 목적지를 갱신하고 고정할 때 외부 및 내부에서 호출하는 안전한 함수
        /// </summary>
        public void SetTarget(Transform target)
        {
            targetTransform = target;
        }

        void Update()
        {
            // 타겟이 없다면 이동을 멈춥니다.
            if (targetTransform == null) return;

            // 목적지(현재 쫓아가는 웨이포인트)를 바라봅니다.
            this.transform.LookAt(targetTransform);

            // ★ [웨이포인트 실시간 반영] 이제 매 꺾임점마다 방향이 바뀌어야 하므로 
            // 현재 내 위치에서 다음 웨이포인트를 향하는 방향을 실시간(Update)으로 계산합니다!
            Vector3 direction = targetTransform.position - transform.position;
            Vector3 moveDir = direction.normalized;

            // Enemy.cs에 세팅된 실시간 속도를 가져옵니다.
            float currentSpeed = (enemy != null) ? enemy.GetCurrentSpeed() : 3f;
            transform.Translate(moveDir * currentSpeed * Time.deltaTime, Space.World);

            // 현재 추적 중인 웨이포인트와의 거리 계산
            float distanceToTarget = Vector3.Distance(transform.position, targetTransform.position);

            // 목적지(웨이포인트)에 도달했는가 판정
            if (distanceToTarget <= arrivalDistance)
            {
                // ★ 다음 웨이포인트로 목적지를 갈아끼우는 전용 메서드 작동
                GetNextWaypoint();
            }
        }

        /// <summary>
        /// ★ 웨이포인트를 밟았을 때 다음 경로로 넘겨주거나 종점 처리를 맡는 함수
        /// </summary>
        private void GetNextWaypoint()
        {
            // 만약 현재 밟은 웨이포인트가 이 맵의 가장 마지막(최종 종점) 웨이포인트라면?
            if (wavePointIndex >= WayPoints.points.Length - 1)
            {
                Debug.Log("최종 종점 도착, 라이프가 깎입니다.");

                if (enemy != null)
                {
                    enemy.ReachEnd(); // 라이프 차감 + Destroy 처리
                }
                else
                {
                    Destroy(gameObject); // 안전장치
                }
                return;
            }

            // 아직 갈 길이 남았다면, 다음 웨이포인트 인덱스로 번호를 올려서 타겟을 교체합니다.
            wavePointIndex++;
            SetTarget(WayPoints.points[wavePointIndex]);
        }
    }
}