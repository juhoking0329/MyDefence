// 경로: Assets/MyDefence/Scripts/Managers/UIManager.cs
using UnityEngine;

namespace MyDefence
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager instance;

        #region Variables
        [Header("상점 품목 데이터 관리")]
        [SerializeField] private TowerBlueprint machineGunTower;
        [SerializeField] private TowerBlueprint rocketTower;
        [SerializeField] private TowerBlueprint laserTower;
        #endregion

        #region Unity Event Methods
        private void Awake()
        {
            if (instance == null) instance = this;
            else Destroy(gameObject);
        }
        #endregion

        #region Custom Methods
        public void ClickMachineGunButton()
        {
            // [변경] 버튼 클릭 시점에 돈 검사
            if (GameData.money < machineGunTower.cost)
            {
                Debug.Log($"❌ 돈이 부족합니다! (필요: {machineGunTower.cost} Gold / 현재: {GameData.money} Gold)");
                return; // 함수를 즉시 종료하여 선택되지 않게 막습니다.
            }

            Debug.Log("머신건 타워를 선택하였습니다!");
            BuildManager.instance.SelectTower(machineGunTower);
        }

        public void ClickRocketTowerButton()
        {
            // [변경] 버튼 클릭 시점에 돈 검사
            if (GameData.money < rocketTower.cost)
            {
                Debug.Log($"❌ 돈이 부족합니다! (필요: {rocketTower.cost} Gold / 현재: {GameData.money} Gold)");
                return;
            }

            Debug.Log("로켓 타워를 선택하였습니다!");
            BuildManager.instance.SelectTower(rocketTower);
        }

        public void ClickLaserTowerButton()
        {
            // [변경] 버튼 클릭 시점에 돈 검사
            if (GameData.money < laserTower.cost)
            {
                Debug.Log($"❌ 돈이 부족합니다! (필요: {laserTower.cost} Gold / 현재: {GameData.money} Gold)");
                return;
            }

            Debug.Log("레이저 타워를 선택하였습니다!");
            BuildManager.instance.SelectTower(laserTower);
        }
        #endregion
    }
}