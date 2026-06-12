// 경로: Assets/MyDefence/Scripts/Managers/BuildManager.cs
using UnityEngine;

namespace MyDefence
{
    public class BuildManager : MonoBehaviour
    {
        public static BuildManager instance;

        #region Variables
        private TowerBlueprint towerToBuild = null;

        [Header("타워 블루프린트 등록")]
        [SerializeField] private TowerBlueprint machineGunBlueprint;
        [SerializeField] private TowerBlueprint RocketTowerBlueprint;
        [SerializeField] private TowerBlueprint LaserTowerBlueprint;

        private GameObject latestBuiltTower;
        #endregion

        #region Unity Event Methods
        private void Awake()
        {
            if (instance == null) instance = this;
            else Destroy(gameObject);
        }
        #endregion

        #region Custom Methods
        public void SelectTower(TowerBlueprint blueprint)
        {
            if (blueprint == null) return;
            towerToBuild = blueprint;
        }

        public void SelectMachineGunTower()
        {
            Debug.Log("머신건 타워 선택!");
            SelectTower(machineGunBlueprint);
        }

        public void SelectRocketTower()
        {
            Debug.Log("미사일 타워 선택!");
            SelectTower(RocketTowerBlueprint);
        }

        public void SelectLaserTower()
        {
            Debug.Log("레이저 타워 선택!");
            SelectTower(LaserTowerBlueprint);
        }
        public bool HasTowerSelected => towerToBuild != null;

        /// <summary>
        /// 타일에서 호출하여 타워를 최종 건설하는 함수 (돈 검사는 이미 UI에서 완료됨)
        /// </summary>
        public bool BuildTowerOn(Transform tileTransform)
        {
            //Debug.Log($"towerToBuild: {towerToBuild}, prefab: {towerToBuild?.towerPrefab}");

            if (towerToBuild == null) return false;

            // [안전장치] 만약 버튼 클릭 후 타일을 누르기 직전에 돈이 갑자기 줄어들었을 경우를 대비한 최소한의 방어코드
            if (GameData.money < towerToBuild.cost)
            {
                towerToBuild = null; // 선택 해제
                return false;
            }

            // 돈 차감 및 건설 진행
            GameData.money -= towerToBuild.cost;
            Debug.Log($"건설 완료! 남은 돈 : {GameData.money} Gold");

            // [★수정] 그냥 생성만 하던 코드에서, 생성된 오브젝트를 latestBuiltTower 상자에 받아두도록 수정합니다.
            latestBuiltTower = Instantiate(towerToBuild.towerPrefab, tileTransform.position, Quaternion.identity);

            // [선택 사항] 타워를 하나 지으면 선택을 초기화하고 싶다면 아래 주석을 해제하세요.
            // towerToBuild = null; 

            return true;
        }


        /// <summary>
        /// BuildTowerOn 함수 바로 아래에 이 배달 함수를 새로 추가합니다.
        /// </summary>
        public GameObject GetLatestBuiltTower()
        {
            return latestBuiltTower;
        }

        /// <summary>
        /// 5-3) 현재 선택된 타일의 타워를 업그레이드하는 함수
        /// </summary>
        public void UpgradeTowerOn(Tile tile, TowerBlueprint blueprint)
        {
            // 돈이 모자라면 컷
            if (GameData.money < blueprint.upgradeCost)
            {
                Debug.Log("❌ 돈이 부족하여 업그레이드할 수 없습니다!");
                return;
            }

            // 1. 돈 차감
            GameData.money -= blueprint.upgradeCost;

            // 2. 기존 타일에 설치되어 있던 1단계 타워 오브젝트 파괴
            Destroy(tile.installedTower);

            // 3. 그 자리에 2단계 업그레이드 타워 프리팹 소환
            GameObject upgradedTower = Instantiate(blueprint.upgradePrefab, tile.transform.position, Quaternion.identity);

            // 4. 타일 정보에 새 타워 등록 및 상태 갱신
            tile.installedTower = upgradedTower;

            Debug.Log("🎯 타워 업그레이드 완료! 성능이 대폭 상승했습니다.");
        }
        #endregion
    }
}