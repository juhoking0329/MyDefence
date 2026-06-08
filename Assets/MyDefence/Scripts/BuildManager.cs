using UnityEngine;

namespace MyDefence
{
    public class BuildManager : MonoBehaviour
    {
        public static BuildManager instance;

        #region Variables
        [Header("과제 1, 2) 직렬화된 타워 품목 등록")]
        [SerializeField] private TowerBlueprint machineGunTower;
        [SerializeField] private TowerBlueprint rocketTower;

        // 현재 유저가 선택한 품목의 '청사진(정보)'을 저장하는 변수
        private TowerBlueprint towerToBuild = null;
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
        /// 첫 번째 버튼 클릭 시: 머신건 타워 청사진 선택
        /// </summary>
        public void SelectMachineGunTower()
        {
            Debug.Log("머신건 타워를 선택 하였습니다!!");
            towerToBuild = machineGunTower;
        }

        /// <summary>
        /// 두 번째 버튼 클릭 시: 로켓 타워 청사진 선택
        /// </summary>
        public void SelectAnotherTower()
        {
            Debug.Log("로켓 타워(다른 타워)를 선택 하였습니다!");
            towerToBuild = rocketTower;
        }

        /// <summary>
        /// 현재 설치 항목이 선택되어 있는지 확인
        /// </summary>
        public bool HasTowerSelected => towerToBuild != null;

        /// <summary>
        /// 과제 4, 5) 타일에서 호출하여 실제 돈을 검사하고 타워를 건설하는 함수
        /// </summary>
        public bool BuildTowerOn(Transform tileTransform)
        {
            if (towerToBuild == null) return false;

            // 과제 5) 소지금이 타워 가격보다 부족하면 건설 실패!
            if (GameData.money < towerToBuild.cost)
            {
                Debug.Log("돈이 부족합니다");
                return false; // 건설 실패를 타일에 알림
            }

            // 과제 4) 돈 계산 후 차감 및 건설 진행
            GameData.money -= towerToBuild.cost;
            Debug.Log($"건설하고 남은돈 : {GameData.money}");

            // 타워 스폰
            Instantiate(towerToBuild.towerPrefab, tileTransform.position, Quaternion.identity);
            return true; // 건설 성공!
        }
        #endregion
    }
}