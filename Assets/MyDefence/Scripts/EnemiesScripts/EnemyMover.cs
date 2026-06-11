using UnityEngine;

namespace MyDefence
{
    /// <summary>
    /// 에너미의 이동을 관리하는 클래스입니다.
    /// </summary>
    public class EnemyMover : MonoBehaviour
    {
        [SerializeField] private Transform targetTransform;
        [SerializeField] private float speed = 5.0f;
        [SerializeField] private float arrivalDistance = 0.1f;

        private Enemy enemy; // Enemy 컴포넌트 참조 추가

        // 매 프레임 계산하지 않고, 처음에 정해진 방향을 기억할 변수입니다.
        private Vector3 moveDir;
        private bool isDirectionSet = false;

        void Start()
        {
            enemy = GetComponent<Enemy>(); // 같은 오브젝트의 Enemy 컴포넌트 가져오기
        }

        // 타겟을 외부(SpawnManager 등)에서 지정해 줄 수도 있도록 만든 안전한 함수입니다.
        public void SetTarget(Transform target)
        {
            targetTransform = target;
            Vector3 direction = targetTransform.position - transform.position;
            moveDir = direction.normalized; // 방향 고정!
            isDirectionSet = true;
        }

        void Update()
        {
            // 타겟이나 방향이 설정되지 않았다면 멈춰섭니다.
            if (targetTransform == null || !isDirectionSet) return;

            // 매번 방향을 새로 구하지 않고, 처음에 고정된 moveDir 방향으로만 '똑바로' 전진합니다!
            // 이 덕분에 뒤 차가 앞 차의 궤적을 그대로 일정 간격 유지하며 따라가게 됩니다.
            float currentSpeed = (enemy != null) ? enemy.GetCurrentSpeed() : speed;
            transform.Translate(moveDir * currentSpeed * Time.deltaTime, Space.World);

            // 도착 판정 (기존과 동일)
            float distanceToTarget = Vector3.Distance(transform.position, targetTransform.position);
            if (distanceToTarget <= arrivalDistance)
            {
                Debug.Log("종점 도착!!!!");
                if (enemy != null)
                {
                    enemy.ReachEnd(); // ← ReachEnd()가 라이프 차감 + Destroy 모두 처리
                }
                else
                {
                    Destroy(gameObject); // enemy가 없을 경우 안전장치
                }
            }
        }
    }
}