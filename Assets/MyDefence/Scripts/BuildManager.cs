using UnityEngine;

namespace MyDefence
{
    public class BuildManager : MonoBehaviour
    {
        public static BuildManager instance;

        #region Variables
        [Header("타워 프리팹 등록")]
        [SerializeField] private GameObject machineGunTowerPrefab;
        [SerializeField] private GameObject anotherTowerPrefab;

        // 현재 유저가 버튼을 눌러 선택한 타워 프리팹을 임시 저장하는 변수 (선택 안 하면 null)
        private GameObject towerToBuild = null;
        #endregion

        #region Unity Event Methods
        private void Awake()
        {
            if (instance == null) instance = this;
            else Destroy(gameObject);
        }
        #endregion

        #region Custom Methods
        /// <summary>
        /// 과제 2-2) 첫 번째 버튼 클릭 시 실행: 머신건 타워 선택
        /// </summary>
        public void SelectMachineGunTower()
        {
            Debug.Log("머신건 타워를 선택 하였습니다!!");
            towerToBuild = machineGunTowerPrefab;
        }

        /// <summary>
        /// 과제 4-1, 2) 두 번째 버튼 클릭 시 실행: 다른 타워 선택
        /// </summary>
        public void SelectAnotherTower()
        {
            Debug.Log("다른 타워 선택 하였습니다!");
            towerToBuild = anotherTowerPrefab;
        }

        /// <summary>
        /// 과제 2-1) 현재 타워가 선택되어 있는지 확인하는 함수
        /// </summary>
        public bool HasTowerSelected => towerToBuild != null;

        /// <summary>
        /// 과제 2-3, 4-3) 타일에서 호출하여 타워를 진짜 설치하는 함수
        /// </summary>
        public void BuildTowerOn(Transform tileTransform)
        {
            if (towerToBuild == null) return;

            // 선택된 타워 프리팹을 타일 위치에 생성
            Instantiate(towerToBuild, tileTransform.position, Quaternion.identity);

            // ★ 중요: 타워를 설치하고 나면 선택을 초기화하고 싶다면 아래 주석을 해제하세요.
            // towerToBuild = null; 
        }
        #endregion
    }
}