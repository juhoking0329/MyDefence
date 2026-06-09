// 경로: Assets/MyDefence/Scripts/Managers/BuildManager.cs
using UnityEngine;

namespace MyDefence
{
    public class BuildManager : MonoBehaviour
    {
        public static BuildManager instance;

        #region Variables
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
        public void SelectTower(TowerBlueprint blueprint)
        {
            if (blueprint == null) return;
            towerToBuild = blueprint;
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

            // 타워 스폰
            Instantiate(towerToBuild.towerPrefab, tileTransform.position, Quaternion.identity);

            // [선택 사항] 타워를 하나 지으면 선택을 초기화하고 싶다면 아래 주석을 해제하세요.
            // towerToBuild = null; 

            return true;
        }
        #endregion
    }
}