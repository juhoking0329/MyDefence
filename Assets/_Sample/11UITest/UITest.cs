using TMPro;
using UnityEngine;

namespace MyDefence
{
    public class UITest : MonoBehaviour
    {
        #region Variables
        [Header("실시간 점수 반영 UI 등록")]
        [SerializeField] private TextMeshProUGUI scoreText; // 화면에 있는 스코어 텍스트 UI

        private int currentScore = 0; // 현재 점수 (초기값: 과제 기준 100점)
        #endregion

        #region Unity Event Methods
        private void Start()
        {
            // 게임이 시작되자마자 화면에 초기 점수인 "SCORE : 0"을 띄워줍니다.
            UpdateScoreUI();
        }
        #endregion

        #region Custom Methods
        /// <summary>
        /// Fire 버튼을 누르면 실행되는 메서드
        /// </summary>
        public void Fire()
        {
            Debug.Log("발사 하였습니다");
        }

        /// <summary>
        /// 점프(Jump) 버튼을 누르면 실행되는 메서드 (점수 10점씩 증가!)
        /// </summary>
        public void Jump()
        {
            Debug.Log("플레이어가 점프하였습니다");

            // 점수를 10점 더하고 화면을 갱신합니다.
            currentScore += 10;
            UpdateScoreUI();
        }

        /// <summary>
        /// 점수 데이터를 기반으로 TextMeshPro UI 글자를 실시간으로 변경합니다.
        /// </summary>
        private void UpdateScoreUI()
        {
            if (scoreText != null)
            {
                // 글자(string) 사이에 변수를 넣어서 "SCORE : 10" 같은 형태로 만들어 꽂아줍니다.
                scoreText.text = $"SCORE : {currentScore}";
            }
        }
        #endregion
    }
}