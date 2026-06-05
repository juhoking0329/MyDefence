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
            if (isTurretBuilt)
            {
                Debug.Log("여기에는 이미 터렛이 설치되어 있습니다!");
                return;
            }

            // 과제 2-1) 만약 선택된 타워가 없다면 설치 실패 처리
            if (!BuildManager.instance.HasTowerSelected)
            {
                Debug.Log("타워를 설치하지 못했습니다.!!");
                return;
            }

            // 과제 2-3, 4-3) 조건 통과 시 타워 설치 진행
            BuildManager.instance.BuildTowerOn(transform);
            isTurretBuilt = true;

            if (normalMaterial != null) rend.material = normalMaterial;
        }
        #endregion
    }
}