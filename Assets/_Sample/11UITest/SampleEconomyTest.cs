using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MySample
{
    public class SampleEconomyTest : MonoBehaviour
    {
        #region Variables
        [Header("UI 연동")]
        [SerializeField] private TextMeshProUGUI goldText; // 상단 Gold Text
        [SerializeField] private Button purchaseBtn1000;  // 1000원 구매 버튼
        [SerializeField] private Button purchaseBtn9000;  // 9000원 구매 버튼

        private int sampleMoney = 3000; // 샘플 씬 시작 소지금 3000원
        #endregion

        #region Unity Event Methods
        private void Start()
        {
            UpdateUI();
        }
        #endregion

        #region Custom Methods
        /// <summary>
        /// 1. 저축 버튼 클릭 시 호출
        /// </summary>
        public void SaveGold()
        {
            sampleMoney += 1000;
            Debug.Log("1000 Gold Save.");
            UpdateUI();
        }

        /// <summary>
        /// 2. 1000원짜리 아이템 구매 버튼 클릭 시 호출
        /// </summary>
        public void PurchaseItem1000()
        {
            if (sampleMoney >= 1000)
            {
                sampleMoney -= 1000;
                Debug.Log("1000 Gold Purchase");
            }
            UpdateUI();
        }

        /// <summary>
        /// 3. 9000원짜리 아이템 구매 버튼 클릭 시 호출
        /// </summary>
        public void PurchaseItem9000()
        {
            if (sampleMoney >= 9000)
            {
                sampleMoney -= 9000;
                Debug.Log("9000 Gold Purchase");
            }
            UpdateUI();
        }

        /// <summary>
        /// 실시간 텍스트 및 버튼 컬러 화이트/레드 조건부 변경 (구매 제한 구현)
        /// </summary>
        private void UpdateUI()
        {
            // 상단 텍스트 갱신
            if (goldText != null) goldText.text = $"{sampleMoney} Gold";

            // 1000원 구매 버튼 예외 처리
            if (purchaseBtn1000 != null)
            {
                if (sampleMoney >= 1000)
                {
                    purchaseBtn1000.image.color = Color.white;
                    purchaseBtn1000.interactable = true; // 버튼 클릭 켬
                }
                else
                {
                    purchaseBtn1000.image.color = Color.red;
                    purchaseBtn1000.interactable = false; // 돈 없으면 클릭 무시!
                }
            }

            // 9000원 구매 버튼 예외 처리
            if (purchaseBtn9000 != null)
            {
                if (sampleMoney >= 9000)
                {
                    purchaseBtn9000.image.color = Color.white;
                    purchaseBtn9000.interactable = true;
                }
                else
                {
                    purchaseBtn9000.image.color = Color.red;
                    purchaseBtn9000.interactable = false; // 돈 없으면 빨간색에 클릭 불가
                }
            }
        }
        #endregion
    }
}