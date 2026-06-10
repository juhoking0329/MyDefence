// 경로: Assets/MyDefence/Scripts/Managers/UIManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

namespace MyDefence
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager instance;

        [Header("상점 품목 데이터 관리 (기존)")]
        [SerializeField] private TowerBlueprint machineGunTower;
        [SerializeField] private TowerBlueprint rocketTower;
        [SerializeField] private TowerBlueprint laserTower;

        [Header("0번 과제: 인게임 UI 텍스트")]
        [SerializeField] private TextMeshProUGUI livesText;
        [SerializeField] private TextMeshProUGUI moneyText;

        [Header("1~4번 과제: 게임오버 UI")]
        [SerializeField] private GameObject gameOverUI;
        [SerializeField] private TextMeshProUGUI roundsSurvivedText;
        [SerializeField] private Animator gameOverAnimator; // 4번 과제용 애니메이터 (선택)

        [Header("5~7번 과제: 일시정지 UI")]
        [SerializeField] private GameObject pauseUI;
        private bool isPaused = false;

        #region Unity Event Methods
        private void Awake()
        {
            if (instance == null) instance = this;
            else Destroy(gameObject);
        }

        private void Start()
        {
            // 게임 시작 시 모든 UI 초기 상태 설정
            if (gameOverUI != null) gameOverUI.SetActive(false);
            if (pauseUI != null) pauseUI.SetActive(false);

            // 일시정지 해제 상태로 시작
            Time.timeScale = 1f;
        }

        private void Update()
        {
            // 실시간 인게임 텍스트 갱신 (0번 과제)
            UpdateInGameUI();

            // 3번 과제: 치트키 "O" 키를 누르면 게임오버 UI 활성화
            if (Input.GetKeyDown(KeyCode.O))
            {
                TriggerGameOver();
            }

            // 6번 과제: ESC 키 입력시 Pause 활성화/비활성화 토글
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePause();
            }
        }
        #endregion

        #region Custom UI Methods
        /// <summary>
        /// 0번 과제: 라이프와 소지금을 화면에 그려주는 함수
        /// </summary>
        private void UpdateInGameUI()
        {
            if (livesText != null) livesText.text = $"LIVES: {GameData.lives}";
            if (moneyText != null) moneyText.text = $"{GameData.money} G";
        }

        /// <summary>
        /// 3번 과제: 게임오버 조건 발동 시 UI를 켜는 함수
        /// </summary>
        public void TriggerGameOver()
        {
            if (gameOverUI == null) return;

            // 1번 과제: 버틴 라운드 수 반영
            if (roundsSurvivedText != null)
            {
                roundsSurvivedText.text = $"{GameData.roundsSurvived} ROUNDS SURVIVED";
            }

            gameOverUI.SetActive(true);

            // 4번 과제: 애니메이션 재생 (트리거명이 있으면 작동)
            if (gameOverAnimator != null)
            {
                gameOverAnimator.SetTrigger("GameOverIn");
            }

            // 게임오버 시 게임을 멈추고 싶다면 하단 주석 해제
            Time.timeScale = 0f; 
        }

        /// <summary>
        /// 6번 과제: ESC 눌렀을 때 시간SCALE을 제어하여 일시정지하는 함수
        /// </summary>
        public void TogglePause()
        {
            // 만약 게임오버 창이 켜져있다면 일시정지 금지
            if (gameOverUI != null && gameOverUI.activeSelf) return;

            isPaused = !isPaused;
            pauseUI.SetActive(isPaused);

            if (isPaused)
            {
                Time.timeScale = 0f; // 적들의 움직임, 카운트다운 전체 정지 (6번 과제 핵심 스킬)
                Debug.Log("⏸️ 게임 일시 정지 (TimeScale = 0)");
            }
            else
            {
                Time.timeScale = 1f; // 게임 다시 진행
                Debug.Log("▶️ 게임 재개 (TimeScale = 1)");
            }
        }
        #endregion

        #region Button Event Methods (2번 및 7번 과제)
        /// <summary>
        /// RESTART 버튼 클릭 시 실행되는 함수
        /// </summary>
        public void ClickRestartButton()
        {
            Debug.Log("Run RESTART"); // 과제 요구사항 출력

            // 타임스케일을 정상 복구하고 현재 씬을 새로고침(재시작)합니다.
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        /// <summary>
        /// MAIN MENU 버튼 클릭 시 실행되는 함수
        /// </summary>
        public void ClickMainMenuButton()
        {
            Debug.Log("Goto Menu"); // 과제 요구사항 출력

            // 타임스케일을 정상 복구하고 메인메뉴 씬으로 이동합니다.
            Time.timeScale = 1f;
            // SceneManager.LoadScene("MainMenuScene"); // 나중에 메인메뉴 씬을 만들면 주석해제 하세요!
        }

        // --- 기존 상점 버튼들 (유지) ---
        public void ClickMachineGunButton()
        {
            if (GameData.money < machineGunTower.cost) 
            { 
                Debug.Log("❌ 돈이 부족합니다!"); 
                return; 
            }
            BuildManager.instance.SelectTower(machineGunTower);
        }
        public void ClickRocketTowerButton()
        {
            if (GameData.money < rocketTower.cost) 
            { 
                Debug.Log("❌ 돈이 부족합니다!"); 
                return; 
            }
            BuildManager.instance.SelectTower(rocketTower);
        }
        public void ClickLaserTowerButton()
        {
            if (GameData.money < laserTower.cost) 
            { 
                Debug.Log("❌ 돈이 부족합니다!"); 
                return; 
            }
            BuildManager.instance.SelectTower(laserTower);
        }
        #endregion
    }
}