using UnityEngine;

namespace MySample
{
    //의자의 이동을 관리하는 클래스
    public class ChairMover : MonoBehaviour
    {
        // 의자의 이동 속도를 조절하는 변수입니다.
        // 인스펙터 창에서 우리가 원하는 대로 속도를 바꿀 수 있게 만듭니다.
        [SerializeField]
        private float moveSpeed = 2.0f;

        // Update는 게임이 실행되는 동안 '매 프레임(매 순간)'마다 계속 실행되는 마법의 구역입니다.
        void Update()
        {
            // transform.Translate는 오브젝트를 이동시키는 가장 쉬운 명령어예요!
            // Vector3.right는 '오른쪽 방향(X축)'을 뜻합니다.
            // Time.deltaTime은 컴퓨터 성능이 달라도 모두 똑같은 속도로 움직이게 해주는 '마법의 시간 조정관'이에요.
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }
    }
}