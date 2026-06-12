using UnityEngine;

namespace MyDefence
{
    public class TileUI : MonoBehaviour
    {
        [Header("UI 오브젝트 부품")]
        [SerializeField] private GameObject uiParent; // 버튼들을 감싸고 있는 실제 연출용 자식 오브젝트
        private Animator animator;
        private Tile currentTile; // 현재 이 UI가 띄워져 있는 타일 정보
        public static TileUI instance;

        void Awake()
        {
            if (instance == null) instance = this;
            else Destroy(gameObject);

            transform.rotation = Quaternion.Euler(70f, 0f, 0f);

            animator = GetComponentInChildren<Animator>();
            // 2-3) 게임을 시작할 때 UI가 안 보인다.
            Hide();
        }
        void Update()
        {
            if (currentTile == null) return;

            transform.position = currentTile.transform.position + Vector3.up * 1.5f + Vector3.forward * 3f;
            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
            
            // Scale 고정
            transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        }


        /// <summary>
        /// 2-1) 선택한 타일 위로 UI를 보여준다 (위치 설정)
        /// </summary>
        public void SetTargetTile(Tile tile)
        {
            //Debug.Log($"SetTargetTile 호출됨, currentTile: {currentTile}, tile: {tile}");
            // 만약 이미 열려있는 타일을 또 선택했다면? ➡️ 2-2) UI가 안보이게 해준다
            if (currentTile == tile)
            {
                //Debug.Log("같은 타일 선택 - Hide 호출");
                Hide();
                return;
            }
            currentTile = tile;

            //Debug.Log($"uiParent: {uiParent}, activeSelf: {uiParent?.activeSelf}");
            uiParent.SetActive(true);
            if (animator != null) animator.SetTrigger("Show"); // 3) 나타나는 애니메이션 실행
        }

        public void Hide()
        {
            currentTile = null;
            uiParent.SetActive(false); // UI 숨기기
        }
    }
}