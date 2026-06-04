using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace MySample
{
    /// <summary>
    /// 게임중 유저 인풋 값을 New Input System 가져와서 적용하기
    /// </summary>
    public class NewInputTest : MonoBehaviour
    {
        #region Variables
        // New Input System에서 만들어진 Class의 객체(인스턴스) 선언
        private NewActionsTest inputActions;

        //카메라 이동
        public float moveSpeed = 10f;

        //화면 경계 테두리 두께
        private float screenBorderThickness = 10f;
        Vector3 moveDir = Vector3.zero;

        #endregion

        #region Unity Event Methods
        private void Awake()
        {
            // New Input System에서 만들어진 Class의 객체(인스턴스) 생성
            inputActions = new NewActionsTest();
        }

        private void OnEnable()
        {
            // New Input System에서 만들어진 Class의 객체(인스턴스) 활성화
            inputActions.Enable();

            inputActions.Camera.EscToggle.performed += Toggle;
        }

        private void OnDisable()
        {
            // New Input System에서 만들어진 Class의 객체(인스턴스) 비활성화
            inputActions.Disable();
            inputActions.Camera.EscToggle.performed -= Toggle;
        }

        private void Update()
        {
            //wasd, arrow 입력값을 받아와서 카메라 이동
            //인스턴스이름.액션맵이름.액션이름.ReadValue<데이터타입>()
            Vector2 inputVector = inputActions.Camera.Move.ReadValue<Vector2>();
            //inputVector.x, inputVector.y

            //이동방향 * Time.deltaTime * speed
            Vector3 dir = new Vector3(inputVector.x, 0f, inputVector.y);
            this.transform.Translate(dir * Time.deltaTime * moveSpeed, Space.World);

            //마우스 위치 가져와서 카메라 이동 처리
            Vector2 mousePos = inputActions.Camera.MousePosition.ReadValue<Vector2>();
            float mousePosX = mousePos.x;
            float mousePosY = mousePos.y;

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

        }
        #endregion

        #region Custom Method
        public void Toggle(InputAction.CallbackContext context)
        {
            Debug.Log("토글버튼이 눌렸다");

        }
        #endregion
    }
}

/*
New Input System

1. Input Action Editor 창 세팅하기(Action Map 설계)
- Action Map 설정(정의) - Player, UI, Vehicle, Camera 등등
- Action 설정(정의, input 값과 바인딩) - Move, Jump, Fire 등등

2. 게임중 유저 인풋 값을 New Input System 가져와서 적용하기
1) 스크립트를 이용하여 값 가져오기
- Input Action Editor 창에서 설정한 값을 Class 파일로 만들어서 처리
- 만들어진 Class의 객체(인스턴스)를 생성해서 인풋 처리


2) SendMessage 방법
- PlayerInput 컴포넌트를 대상 오브젝트에 추가한다
- Actions에 설계한 Actions를 등록한다 (인스펙터 창에 드래그하여 바인딩)
- Behavior를 SendMessage로 설정한다
- 스크립트에 유저 인풋 값을 받아오는 함수를 만든다 (규칙에 맞게 만든다)
: 함수이름은 On + 액션이름(InputValue value)

3) Unity Event 등록 방법
- PlayerInput 컴포넌트를 대상 오브젝트에 추가한다
- Actions 에 설계한 Actions를 등록한다 (인스펙터 창에 드래그하여 바인딩)
- Behavior를 Invoke Unity Event로 설정한다
- 스크립트에 유저 인풋 값을 받아오는 함수를 만든다 (규칙에 맞게 만든다)
: 함수이름 규칙이 없음, 매개변수는 규칙이 있다
public void 함수이름 (InputAction.CallbackContext context)
- 만든 함수를 Action에 대응하는 이벤트에 등록한다

...
*/