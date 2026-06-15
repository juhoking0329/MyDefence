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

            // 생성된 오브젝트를 latestBuiltTower 상자에 받아둡니다.
            latestBuiltTower = Instantiate(towerToBuild.towerPrefab, tileTransform.position, Quaternion.identity);

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
        /// ★ [신규 추가] 타일 위에 서 있는 타워의 이름을 분석해서 알맞은 블루프린트로 자동 업그레이드해주는 마스터 함수
        /// </summary>
        public void UpgradeTowerByName(Tile tile, string currentTowerName)
        {
            TowerBlueprint blueprint = null;

            if (currentTowerName.Contains("MachineGun")) blueprint = machineGunBlueprint;
            else if (currentTowerName.Contains("Rocket")) blueprint = RocketTowerBlueprint;
            else if (currentTowerName.Contains("Laser")) blueprint = LaserTowerBlueprint;

            if (blueprint == null)
            {
                Debug.LogWarning("⚠️ 알맞은 블루프린트를 찾지 못했습니다.");
                return;
            }

            // _1이 포함 = 2단계 타워 → 3단계로 업그레이드
            if (currentTowerName.Contains("_1"))
            {
                UpgradeTowerOn(tile, blueprint, 3);
            }
            // _1, _2 없음 = 1단계 타워 → 2단계로 업그레이드
            else
            {
                UpgradeTowerOn(tile, blueprint, 2);
            }
        }

        /// <summary>
        /// 5-3) 현재 선택된 타일의 타워를 업그레이드하는 함수 (데이터 완전 갱신형 보완)
        /// </summary>
        public void UpgradeTowerOn(Tile tile, TowerBlueprint blueprint, int targetStage)
        {
            GameObject prefabToSpawn = null;
            int cost = 0;

            if (targetStage == 2)
            {
                prefabToSpawn = blueprint.upgradePrefab;
                cost = blueprint.upgradeCost;
            }
            else if (targetStage == 3)
            {
                prefabToSpawn = blueprint.upgradePrefab2;
                cost = blueprint.upgradeCost2;
            }

            if (prefabToSpawn == null)
            {
                Debug.Log("❌ 업그레이드 프리팹이 없습니다!");
                return;
            }

            if (GameData.money < cost)
            {
                Debug.Log("❌ 돈이 부족하여 업그레이드할 수 없습니다!");
                return;
            }

            GameData.money -= cost;
            Destroy(tile.installedTower);
            tile.installedTower = Instantiate(prefabToSpawn, tile.transform.position, Quaternion.identity);

            Debug.Log($"🎯 {targetStage}단계 업그레이드 완료!");
        }
        #endregion
    }
}