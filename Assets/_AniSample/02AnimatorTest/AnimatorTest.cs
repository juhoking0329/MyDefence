using UnityEngine;

namespace MySample
{
    /// <summary>
    /// 라이트 애니메이션 1초마다 랜덤하게 플레이 시킨다
    /// </summary>
    public class AnimatorTest : MonoBehaviour
    {
        private Animator animator;
        private int lastRandomID = -1; // 직전에 어떤 숫자가 나왔는지 기억하는 상자

        void Start()
        {
            animator = GetComponent<Animator>();
            RandomizeId();
        }

        void Update()
        {
            // 현재 애니메이션이 98% 이상 완료되었을 때 다음 무작위 불꽃 준비
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.98f && !animator.IsInTransition(0))
            {
                RandomizeId();
            }
        }

        /// <summary>
        /// 직전 숫자와 절대 겹치지 않게 완벽한 하이퍼 랜덤 주사위를 굴리는 함수
        /// </summary>
        private void RandomizeId()
        {
            int currentRandomID = lastRandomID;

            // 핵심: 새로 뽑은 주사위 숫자가 직전 숫자와 똑같다면, "무조건 다를 때까지" 계속 다시 뽑습니다!
            // 이 처리가 들어가야 1번 불꽃 다음에 다시 1번이 나오는 어색한 정체 현상이 사라집니다.
            while (currentRandomID == lastRandomID)
            {
                currentRandomID = Random.Range(0, 3); // 0, 1, 2 중 랜덤 선택
            }

            // 새로운 번호를 기억해 둡니다.
            lastRandomID = currentRandomID;

            // 애니메이터에 최종 전달!
            animator.SetInteger("RandomID", currentRandomID);
        }
    }
}