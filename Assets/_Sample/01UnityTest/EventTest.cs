using UnityEngine;

namespace MySample
{
    //유니티 MonoBehaviour 스크립트 이벤트 함수 테스트
    public class EventTest : MonoBehaviour
    {
        private void Awake()
        {
            Debug.Log("[1] Awake 실행");      //1회만 실행, 가장 먼저 실행
        }

        void OnEnable()
        {
            Debug.Log("[7-1] OnEnable 실행");    //(활성화 될때마다) 1번만 실행
        }

        private void Start()
        {
            Debug.Log("[2] Start 실행");      //1회만 실행, Awake 다음에 실행
        }

        private void FixedUpdate()
        {
            Debug.Log("[3] FixedUpdate 실행");  //1초에 50회 실행, 물리 연산과 관련된 업데이트
        }

        private void Update()
        {
            Debug.Log("[4] Update 실행");       //매 프레임마다 호출, 게임 로직 연산
        }

        private void LateUpdate()
        {
            Debug.Log("[5] LateUpdate 실행");   //Update() 실행 뒤에 바로 따라서 실행, 카메라 이동 등 Update()에서 처리된 후에 실행되어야 하는 로직에 사용
        }

        private void OnDisable()
        {
            Debug.Log("[7-2] OnDisable 실행");   //(비활성화 될때마다) 1번만 실행
        }

        private void OnDestroy()
        {
            Debug.Log("[6] OnDestroy 실행");    //게임 오브젝트가 파괴될 때 호출, 하이라키에서 제거될 때(킬) 실행
        }
    }
}