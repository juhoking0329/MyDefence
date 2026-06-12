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

            // 과제 2-4) 만약 상점에서 선택한 타워가 없다면 하이라이트(머티리얼 변경) 안 함!
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
            //Debug.Log($"TileUI: {TileUI.instance}, BuildManager: {BuildManager.instance}");
            // -------------------------------------------------------------
            // [★과제 2번 구현] 만약 이 타일 위에 이미 타워가 설치되어 있다면?
            // -------------------------------------------------------------
            if (isTurretBuilt)
            {
                Debug.Log("이 타일에는 이미 타워가 있습니다! 업그레이드 UI 조작 모드로 진입합니다.");

                // TODO: 여기에 과제 2-1, 2-2번인 NodeUI를 띄우는 코드를 연결하게 됩니다.
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

                //Debug.Log($"BuildManager: {BuildManager.instance}");
                // [★추가] 방금 BuildManager가 이 타일 위에 소환해준 따끈따끈한 타워 오브젝트를
                // 이 타일의 installedTower 상자에 실시간으로 쏙 저장해 둡니다!
                // (이 처리가 되어야 나중에 BuildManager에서 이 타워를 조종하여 파괴하거나 바꿀 수 있습니다.)
                installedTower = BuildManager.instance.GetLatestBuiltTower();
                return;
            }
        }
        #endregion
    }
}