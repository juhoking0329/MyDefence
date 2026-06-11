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

            [Header("인게임 UI 텍스트")]
            [SerializeField] private TextMeshProUGUI livesText;
            [SerializeField] private TextMeshProUGUI moneyText;

            [Header("게임오버 UI")]
            [SerializeField] private GameObject gameOverUI;
            [SerializeField] private TextMeshProUGUI roundsSurvivedText;

            [Header("일시정지 UI")]
            [SerializeField] private GameObject pauseUI;

            private bool isPaused = false;

            // [변경] static으로 만들어 씬이 완전히 새로 켜져도 이 대기 상태 상태를 기억할 수 있게 합니다.
            private static bool isWaitingForStart = false;

            private void Awake()
            {
                if (instance == null) instance = this;
                else Destroy(gameObject);
            }

            private void Start()
            {
                if (gameOverUI != null) gameOverUI.SetActive(false);
                if (pauseUI != null) pauseUI.SetActive(false);

                // [★핵심 수정] 씬이 새로 로드되었을 때, 메인메뉴 대기 상태인지 검사합니다.
                if (isWaitingForStart)
                {
                    Time.timeScale = 0f; // 메인메뉴 클릭 후 들어왔다면 안전하게 여기서 화면을 정지시킵니다.
                    Debug.Log("⏸️ 메인 메뉴 대기 상태: 아무 키나 누르면 시작합니다.");
                }
                else
                {
                    Time.timeScale = 1f; // 일반 시작이나 RESTART일 때는 정상 작동하도록 1로 세팅합니다.
                }
            }

            private void Update()
            {
                // 실시간 인게임 UI 텍스트 갱신
                if (livesText != null) livesText.text = $"LIVES: {GameData.lives}";
                if (moneyText != null) moneyText.text = $"{GameData.money} G";

                // 메인 메뉴 누른 후 대기 상태일 때 처리
                if (isWaitingForStart)
                {
                    // [★버그 완치 수정] 마우스 클릭을 제외하고, '키보드 아무 키'나 '스페이스바'를 누르면 시작하게 바꿉니다.
                    // 마우스 클릭을 조건에서 빼야 상점/타일 클릭이 꼬이지 않습니다!
                    if (Input.anyKeyDown && !Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1))
                    {
                        StartGameFromMenu();
                    }
                    return; // 대기 중에는 아래 ESC나 치트키 입력을 무시합니다.
                }

                // 치트키 O 누르면 게임오버 활성화
                if (Input.GetKeyDown(KeyCode.O))
                {
                    TriggerGameOver();
                }

                // ESC 키 입력시 Pause 토글
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    TogglePause();
                }
            }

            public void TriggerGameOver()
            {
                if (gameOverUI == null) return;

                GameData.isGameOver = true;

                if (roundsSurvivedText != null)
                {
                    roundsSurvivedText.text = $"{GameData.roundsSurvived} ROUNDS SURVIVED";
                }

                gameOverUI.SetActive(true);
                Time.timeScale = 0f;
            }

            public void TogglePause()
            {
                if (gameOverUI != null && gameOverUI.activeSelf) return;

                isPaused = !isPaused;
                pauseUI.SetActive(isPaused);
                Time.timeScale = isPaused ? 0f : 1f;
            }

            public void ClickRestartButton()
            {
                GameData.isGameOver = false;

                Debug.Log("Run RESTART");

                // 모든 데이터 리셋
                GameData.money = 400;
                GameData.lives = 10;
                GameData.roundsSurvived = 0;
                isWaitingForStart = false; // 재시작 시에는 대기하지 않음

                Time.timeScale = 1f; // 타임스케일을 확실히 풀고 로드!
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }

            public void ClickMainMenuButton()
            {
                GameData.isGameOver = false;

                Debug.Log("Goto Menu");

                // 1. 게임 데이터 초기화
                GameData.money = 400;
                GameData.lives = 10;
                GameData.roundsSurvived = 0;

                // 2. 씬이 로드된 후 대기하도록 플래그를 미리 true로 세팅
                isWaitingForStart = true;

                // 3. 타임스케일을 일단 1f로 둔 상태에서 안전하게 씬을 다시 로드합니다.
                Time.timeScale = 1f;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }

            private void StartGameFromMenu()
            {
                Debug.Log("▶️ 게임 시작! 타임스케일 정상 가동.");
                isWaitingForStart = false;
                Time.timeScale = 1f; // 대기가 풀리면서 정상적으로 1f로 작동하므로 타워 설치가 정상화됩니다!
            }
        }
    }