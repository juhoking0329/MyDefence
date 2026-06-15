// 경로: Assets/MyDefence/Scripts/EnvironmentScripts/Tile.cs
using UnityEngine;
using UnityEngine.EventSystems;

namespace MyDefence
{
    public class Tile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        #region Variables
        [Header("타일 상태별 머티리얼 설정")]
        [SerializeField] private Material normalMaterial;
        [SerializeField] private Material hoverMaterial;

        private Renderer rend;
        private bool isTurretBuilt = false;

        // [★과제 5번 연동 필수 변수] BuildManager가 접근할 수 있도록 public으로 선언합니다.
        // 이 타일에 실제로 생성되어 서 있는 타워 게임오브젝트를 기억하는 상자입니다.
        [HideInInspector] public GameObject installedTower;
        #endregion

        #region Unity Event Methods
        void Start()
        {
            rend = GetComponent<Renderer>();
            if (rend != null && normalMaterial != null)
            {
                rend.material = normalMaterial;
            }
        }
        #endregion

        #region Custom Methods
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (isTurretBuilt) return;

            // 과제 2-4) 만약 상점에서 선택한 타워가 없다면 하이라이트 안 함!
            if (!BuildManager.instance.HasTowerSelected) return;

            if (hoverMaterial != null) rend.material = hoverMaterial;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (isTurretBuilt) return;

            // 항상 안전하게 기본 머티리얼로 복구
            if (normalMaterial != null) rend.material = normalMaterial;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            // [★과제 2번 구현] 만약 이 타일 위에 이미 타워가 설치되어 있다면?
            if (isTurretBuilt)
            {
                Debug.Log("이 타일에는 이미 타워가 있습니다! 업그레이드 UI 조작 모드로 진입합니다.");
                TileUI.instance.SetTargetTile(this);
                return;
            }

            // 과제 2-1) 만약 선택된 타워가 없다면 설치 실패 처리
            if (!BuildManager.instance.HasTowerSelected)
            {
                Debug.Log("타워를 설치하지 못했습니다.!!");
                return;
            }

            // 과제 4, 5번 연동: 돈이 충분해서 건설에 최종 성공했을 때만!
            bool isSuccess = BuildManager.instance.BuildTowerOn(transform);

            if (isSuccess)
            {
                isTurretBuilt = true; // 타일에 타워가 지어졌다고 박제!
                if (normalMaterial != null) rend.material = normalMaterial; // 색상 복구

                installedTower = BuildManager.instance.GetLatestBuiltTower();
                return;
            }
        }

        /// <summary>
        /// ★ [신규 추가] 타워가 판매되었을 때 타일의 상태를 유령 없이 완전히 태초의 상태로 리셋하는 함수
        /// </summary>
        public void ResetTileState()
        {
            isTurretBuilt = false;       // 1. 설치 여부 거짓으로 리셋 (★유령 버그 해결 핵심!)
            installedTower = null;       // 2. 들고 있던 타워 오브젝트 참조 완벽 청소

            if (rend != null && normalMaterial != null)
            {
                rend.material = normalMaterial; // 3. 혹시나 꼬여있을 머티리얼 색상 원래대로 원상복구
            }

            Debug.Log("타일의 판매가 완전히 완료되었습니다!");
        }
        #endregion
    }
}