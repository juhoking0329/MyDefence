// 경로: Assets/MyDefence/Scripts/EnvironmentScripts/TileUI.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MyDefence
{
    public class TileUI : MonoBehaviour
    {
        [Header("UI 오브젝트 부품")]
        [SerializeField] private GameObject uiParent;

        [Header("과제용 UI 컴포넌트 등록")]
        [SerializeField] private TextMeshProUGUI upgradePriceText;
        [SerializeField] private TextMeshProUGUI sellPriceText;
        [SerializeField] private Button upgradeButton;

        [Header("SELL 파티클 이펙트 프리팹")]
        [SerializeField] private GameObject sellEffectPrefab;

        private Animator animator;
        private Tile currentTile;
        public static TileUI instance;

        void Awake()
        {
            if (instance == null) instance = this;
            else Destroy(gameObject);

            transform.rotation = Quaternion.Euler(70f, 0f, 0f);
            animator = GetComponent<Animator>();
            Hide();
        }

        void Update()
        {
            if (currentTile == null) return;

            transform.position = currentTile.transform.position + Vector3.up * 1.5f + Vector3.forward * 3f;
            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
        }

        public void SetTargetTile(Tile tile)
        {
            if (currentTile == tile)
            {
                Hide();
                return;
            }
            currentTile = tile;

            uiParent.SetActive(true);
            if (animator != null) animator.Play("TileUI_Show", 0, 0f);

            UpdateUIContents();
        }

        private void UpdateUIContents()
        {
            if (currentTile == null || currentTile.installedTower == null) return;

            Tower towerData = currentTile.installedTower.GetComponent<Tower>();

            if (towerData != null)
            {
                if (sellPriceText != null)
                    sellPriceText.text = $"{towerData.SellPrice}G";

                if (towerData.UpgradePrice <= 0)
                {
                    if (upgradePriceText != null) upgradePriceText.text = "MAX";  // 가격 부분
                    if (upgradeButton != null) upgradeButton.interactable = false;
                }
                else
                {
                    if (upgradePriceText != null) upgradePriceText.text = $"{towerData.UpgradePrice}G";  // 가격 부분
                    if (upgradeButton != null) upgradeButton.interactable = true;
                }
            }
        }

        /// <summary>
        /// 인게임 Upgrade 버튼을 누르면 작동할 기능
        /// </summary>
        public void OnUpgradeButtonClick()
        {
            Debug.Log($"업그레이드 버튼 클릭됨, tower: {currentTile?.installedTower?.name}");
            if (currentTile == null || currentTile.installedTower == null) return;

            Tower towerData = currentTile.installedTower.GetComponent<Tower>();
            if (towerData == null) return;

            Debug.Log($"업그레이드 버튼 클릭됨, tower: {currentTile?.installedTower?.name}");    //주석처리해도 됨

            // 1. 소지금 검사
            if (GameData.money < towerData.UpgradePrice)
            {
                Debug.Log("❌ 돈이 부족하여 업그레이드할 수 없습니다!");
                return;
            }

            // 2. 복구된 BuildManager의 마스터 매칭 함수를 안전하게 격발합니다.
            BuildManager.instance.UpgradeTowerByName(currentTile, currentTile.installedTower.name);

            // 3. 업그레이드가 끝난 뒤 UI 창을 자연스럽게 닫아 데이터를 클리어합니다.
            Hide();
        }

        /// <summary>
        /// 인게임 SELL 버튼을 누르면 작동할 기능
        /// </summary>
        public void OnSellButtonClick()
        {
            if (currentTile == null || currentTile.installedTower == null) return;

            Tower towerData = currentTile.installedTower.GetComponent<Tower>();
            if (towerData == null) return;

            Debug.Log($"towerData: {towerData}, SellPrice: {towerData?.SellPrice}");    //주석처리해도 됨

            // ★ [정산 버그 완전 방어] 파괴 전 프로퍼티를 통해 안전하게 골드 데이터 복사
            int cashBack = towerData.SellPrice;
            GameData.money += cashBack;
            Debug.Log($"💰 [정산 완료] +{cashBack}G가 소지금에 추가되었습니다! (현재 잔액: {GameData.money}G)");

            // 이펙트 소환
            if (sellEffectPrefab != null)
            {
                GameObject effect = Instantiate(sellEffectPrefab, currentTile.transform.position, Quaternion.identity);
                Destroy(effect, 2.0f);
            }

            // 타워 오브젝트 파괴 및 타일 청소
            Destroy(currentTile.installedTower);
            currentTile.ResetTileState();

            Hide();
        }

        public void Hide()
        {
            currentTile = null;
            uiParent.SetActive(false);
        }
    }
}