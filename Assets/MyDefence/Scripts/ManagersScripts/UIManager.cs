// 경로: Assets/MyDefence/Scripts/Managers/UIManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

namespace MyDefence
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager instance;

        [Header("인게임 UI 텍스트 (과제 0번)")]
        [SerializeField] private TextMeshProUGUI livesText;
        [SerializeField] private TextMeshProUGUI moneyText;

        [Header("게임오버 UI (과제 1번)")]
        [SerializeField] private GameObject gameOverUI;
        [SerializeField] private TextMeshProUGUI roundsSurvivedText;

        [Header("일시정지 UI")]
        [SerializeField] private GameObject pauseUI;

        [Header("웨이브 타이머 UI")]
        [SerializeField] private TMP_Text timerText;

        private bool isPaused = false;

        // 치트키 테스트를 위해 잠시 false 처리하거나 상태를 확인하세요.
        private static bool isWaitingForStart = false;

        private void Awake()
        {
            if (instance == null) instance = this;
            else Destroy(gameObject);
        }

        private void Start()
        {
            // 과제 UI 초기화 세팅
            if (gameOverUI != null) gameOverUI.SetActive(false);
            if (pauseUI != null) pauseUI.SetActive(false);
            if (timerText != null) timerText.text = "";

            if (isWaitingForStart)
            {
                Time.timeScale = 0f;
                Debug.Log("⏸️ 메인 메뉴 대기 상태: 아무 키나 누르면 시작합니다.");
            }
            else
            {
                Time.timeScale = 1f;
            }
        }

        private void Update()
        {
            // UI 텍스트 실시간 동기화 (과제 0번)
            if (livesText != null) livesText.text = $"LIVES: {GameData.lives}";
            if (moneyText != null) moneyText.text = $"{GameData.money} G";

            // ========================================================
            // ★★★ [과제 3번 치트키 복구] ★
            // 대기 상태(return)보다 위에 배치하여, 게임이 멈춰있든 말든 무조건 감지합니다!
            // 혹시 모를 한글 상태('ㅐ') 입력까지 커버하기 위해 알파벳 O와 한글 ㅐ 자리를 둘 다 체크해 줍니다.
            if (Input.GetKeyDown(KeyCode.O) || Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.H))
            {
                Debug.Log("[치트키 발동] 게임오버 트리거!");
                TriggerGameOver();
            }
            // ========================================================

            // ESC 일시정지도 치트키급으로 상단 배치
            if (Input.GetKeyDown(KeyCode.Escape)) { TogglePause(); }

            // 메인메뉴 대기 플래그 체크 처리
            if (isWaitingForStart)
            {
                if (Input.anyKeyDown && !Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1))
                {
                    StartGameFromMenu();
                }
                return; // 대기 중일 땐 여기서 로직 차단
            }
        }

        /// <summary>
        /// ★ [과제 3번/1번] 게임오버 UI를 활성화하는 핵심 퍼블릭 함수
        /// </summary>
        public void TriggerGameOver()
        {
            if (gameOverUI != null)
            {
                // 1. 게임오버 창 켜기
                gameOverUI.SetActive(true);

                // 2. 버텨낸 라운드 수 UI에 연결 (과제 1번)
                if (roundsSurvivedText != null)
                {
                    // 예: "25 ROUNDS SURVIVED" 형태로 출력
                    roundsSurvivedText.text = $"{GameData.roundsSurvived} ROUNDS SURVIVED";
                }

                // 3. 게임 내부 시간 정지 (디펜스 국룰)
                Time.timeScale = 0f;
                Debug.Log("💀 GAME OVER : 게임이 종료되었습니다.");
            }
        }

        /// <summary>
        /// ★ [과제 2번] RESTART 버튼 클릭 시 호출될 함수 (인펙터 버튼 OnClick에 연결)
        /// </summary>
        public void ClickRestartButton()
        {
            // 과제 조건 요구사항 로그 출력
            Debug.Log("Run RESTART");

            GameData.money = 400;          // 기획서 상의 초기 소지금 입력 (예: 400골드)
            GameData.lives = 10;           // 기획서 과제 0번 조건인 라이프 10개 설정
            GameData.roundsSurvived = 0;   // 버틴 라운드 카운트도 0으로 완벽 초기화!

            // 시간 복구 후 현재 플레이 씬 재로드
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        /// <summary>
        /// ★ [과제 2번] MAIN MENU 버튼 클릭 시 호출될 함수 (인펙터 버튼 OnClick에 연결)
        /// </summary>
        public void ClickMainMenuButton()
        {
            // 과제 조건 요구사항 로그 출력
            Debug.Log("Goto Menu");

            Time.timeScale = 1f;

            // 만약 SceneFader가 있으면 페이드아웃 효과 연출하며 메인메뉴로 이동
            if (SceneFader.instance != null)
            {
                SceneFader.instance.FadeTo("MainMenu");
            }
            else
            {
                SceneManager.LoadScene("MainMenu");
            }
        }

        public void TogglePause()
        {
            isPaused = !isPaused;
            if (pauseUI != null) pauseUI.SetActive(isPaused);
            Time.timeScale = isPaused ? 0f : 1f;
        }

        private void StartGameFromMenu()
        {
            isWaitingForStart = false;
            Time.timeScale = 1f;
            Debug.Log("▶️ 게임 시작!");
        }
    }
}