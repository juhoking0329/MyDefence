using UnityEngine;
using TMPro; // TextMeshPro를 제어하기 위해 필수 추가!

namespace MyDefence
{
    /// <summary>
    /// 게임의 전반적인 데이터와 실시간 UI 표시를 관리하는 클래스
    /// </summary>
    public class GameData : MonoBehaviour
    {
        #region Variables
        // static 변수는 게임 전체에서 단 하나만 공유됩니다.
        public static int money = 400;
        public static int lives = 10;        // 0번 과제: 라이프 10개 설정
        public static int roundsSurvived = 0; // 1번 과제: 버틴 라운드 수 카운트

        [Header("실시간 UI 연동")]
        [SerializeField] private TextMeshProUGUI goldTextUI; // 화면 우측 상단의 GoldText 오브젝트가 들어갈 칸
        #endregion

        #region Unity Event Methods
        void Update()
        {
            // 매 프레임마다 static money 변수의 값을 UI 텍스트에 실시간으로 반영합니다.
            if (goldTextUI != null)
            {
                goldTextUI.text = $"{money} Gold";
            }
        }
        #endregion
    }
}