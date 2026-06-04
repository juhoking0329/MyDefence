using UnityEngine;

namespace MyDefence
{
    public class CameraController : MonoBehaviour
    {
        [Header("카메라 이동 속도")]
        [SerializeField] private float moveSpeed = 20f;
        [SerializeField] private float scrollSpeed = 500f; // 마우스 휠 줌 속도

        [Header("마우스 화면 끝 감지 폭 (과제 2번 기준: 10)")]
        [SerializeField] private float screenBorderThickness = 10f;

        [Header("줌인 줌아웃 한계치 (과제 3번 기준)")]
        [SerializeField] private float minY = 10f;
        [SerializeField] private float maxY = 40f;

        // 과제 4번: 이동 가능 여부를 판단하는 토글 변수 (기본값: true - 이동 가능)
        private bool isMovementEnabled = true;

        void Update()
        {
            // -------------------------------------------------------------
            // 과제 4: ESC 키 토글 (누를 때마다 온/오프 체인지)
            // -------------------------------------------------------------
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                isMovementEnabled = !isMovementEnabled; // true면 false로, false면 true로 전환
                Debug.Log($"카메라 이동 상태 변경: {isMovementEnabled}");
            }

            // 이동이 금지된 상태(false)라면 아래 이동 로직을 전부 건너뜁니다!
            if (!isMovementEnabled) return;


            // -------------------------------------------------------------
            // 과제 1 & 2: 키보드 입력 및 마우스 화면 끝 스크롤 통합
            // -------------------------------------------------------------
            Vector3 moveDir = Vector3.zero;

            // W 키 또는 상단 화살표 키 OR 마우스가 화면 맨 위(Screen.height - 10)에 닿았을 때
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.mousePosition.y >= Screen.height - screenBorderThickness)
            {
                moveDir += Vector3.forward; // 앞으로 전진 (Z축 +)
            }
            // S 키 또는 하단 화살표 키 OR 마우스가 화면 맨 아래(10 이하)에 닿았을 때
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || Input.mousePosition.y <= screenBorderThickness)
            {
                moveDir += Vector3.back; // 뒤로 후진 (Z축 -)
            }
            // D 키 또는 우측 화살표 키 OR 마우스가 화면 맨 오른쪽(Screen.width - 10)에 닿았을 때
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || Input.mousePosition.x >= Screen.width - screenBorderThickness)
            {
                moveDir += Vector3.right; // 오른쪽으로 (X축 +)
            }
            // A 키 또는 좌측 화살표 키 OR 마우스가 화면 맨 왼쪽(10 이하)에 닿았을 때
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || Input.mousePosition.x <= screenBorderThickness)
            {
                moveDir += Vector3.left; // 왼쪽으로 (X축 -)
            }

            // 프레임이 독립적으로 일정하게 부드럽게 움직이도록 Time.deltaTime을 곱해 이동시킵니다.
            transform.Translate(moveDir * moveSpeed * Time.deltaTime, Space.World);


            // -------------------------------------------------------------
            // 과제 3: 마우스 휠 스크롤을 이용한 줌인 줌아웃 (높이 Y 조절)
            // -------------------------------------------------------------
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");

            if (scrollInput != 0f)
            {
                Vector3 pos = transform.position;

                // 휠을 굴린 만큼 Y축 값을 변경 (마우스 휠을 위로 굴리면 소수점 +값이 들어오므로 Y가 낮아져야 줌인)
                pos.y -= scrollInput * scrollSpeed * Time.deltaTime;

                // 유니티 공식 한계치 가두기(Clamping) 함수: 과제 기준 10이상, 40이하로 제한
                pos.y = Mathf.Clamp(pos.y, minY, maxY);

                transform.position = pos;
            }
        }
    }
}