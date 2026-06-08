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

            // 과제 4, 5번 연동: 돈이 충분해서 건설에 최종 성공했을 때만!
            bool isSuccess = BuildManager.instance.BuildTowerOn(transform);

            if (isSuccess)
            {
                isTurretBuilt = true; // 타일에 타워가 지어졌다고 박제!
                if (normalMaterial != null) rend.material = normalMaterial; // 색상 복구
            }
            // (만약 isSuccess가 false(돈 부족)라면, 아무 일도 안 일어나고 타일은 계속 설치 가능한 상태로 남습니다.)
        }
        #endregion
    }
}