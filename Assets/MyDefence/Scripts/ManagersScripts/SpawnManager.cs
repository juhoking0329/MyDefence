// 경로: Assets/MyDefence/Scripts/Managers/SpawnManager.cs
using MyDefence;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MyDefence
{
    [System.Serializable]
    public class WaveData
    {
        public GameObject enemyPrefab;  // 소환할 적 프리팹
        public int count;               // 생성 갯수
        public float spawnDelay;        // 발생 지연 시간
    }

    public class SpawnManager : MonoBehaviour
    {
        // 어디서나 적들이 전멸 처리를 보고할 수 있도록 싱글톤화
        public static SpawnManager instance;

        [Header("스폰 포인트 설정")]
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private Transform targetTransform;

        [Header("과제 5번 : 직렬화된 클래스 배열")]
        [SerializeField] private WaveData[] waves;

        [Header("UI 설정")]
        [SerializeField] private GameObject startButton;        // 웨이브 시작 버튼 오브젝트 (StartButton)
        [SerializeField] private GameObject waveInfo;           // enemy count 정보창 오브젝트 (WaveInfo)
        [SerializeField] private TMP_Text waveInfoText;         // WaveInfo의 자식 오브젝트에 있는 텍스트 컴포넌트

        // ★ [2번 요청사항] 중앙 상단으로 이사 간 타이머 텍스트 컴포넌트 연결
        [SerializeField] private TMP_Text timerText;

        private int currentWaveIndex = 0; // 현재 진행 중인 웨이브 인덱스 (0부터 시작)
        private int activeEnemyCount = 0; // 현재 맵에 살아 숨 쉬는 적 마릿수 (현재 마릿수)
        private int totalWaveEnemyCount = 0; // 이번 웨이브에 스폰되는 총 적의 마릿수 (총 마릿수)
        private bool isWaveActive = false; // 현재 웨이브가 진행 중인지 판별 플래그

        private void Awake()
        {
            if (instance == null) instance = this;
            else Destroy(gameObject);
        }

        private void Start()
        {
            GameData.ResetData();

            // 게임 최초 시작 시 UI 초기화
            if (startButton != null) startButton.SetActive(true);
            if (waveInfo != null) waveInfo.SetActive(false);
            if (timerText != null) timerText.text = ""; // 시작 시 타이머 글자는 비워둡니다.
        }

        /// <summary>
        /// ★ StartButton을 클릭하면 작동할 함수 (첫 웨이브 시작 버튼)
        /// </summary>
        public void ClickStartWaveButton()
        {
            // 이미 웨이브가 활성화 중이거나 레벨 클리어라면 중복 실행 방지
            if (isWaveActive || currentWaveIndex >= waves.Length) return;

            // 이제 전체 웨이브 루프를 제어하는 메인 코루틴을 실행합니다.
            StartCoroutine(MasterWaveRoutine());
        }

        /// <summary>
        /// ★ [1번 요청사항 반영] 첫 클릭 이후 모든 웨이브와 5초 대기를 체인처럼 이어주는 마스터 루틴
        /// </summary>
        IEnumerator MasterWaveRoutine()
        {
            isWaveActive = true;

            // 버튼을 클릭하면 그 즉시 StartButton을 끄고 WaveInfo 오브젝트로 변경! (이후 절대 버튼으로 안 돌아감)
            if (startButton != null) startButton.SetActive(false);
            if (waveInfo != null) waveInfo.SetActive(true);

            // 전체 웨이브 배열을 순회하며 자동 진행
            while (currentWaveIndex < waves.Length)
            {
                // 1. 이번 웨이브의 기획 데이터 로드 및 초기화
                WaveData currentWave = waves[currentWaveIndex];
                totalWaveEnemyCount = currentWave.count;
                activeEnemyCount = 0;

                // 스폰 시작 전 마릿수 UI 갱신 (예: 대기 끝나고 시작될 때 바로 Wave 1(0/1) 이 됨)
                UpdateWaveInfoUI();
                Debug.Log($"[Wave {currentWaveIndex + 1}] 스폰 가동!");

                // 2. 에너미 스폰 루프
                for (int i = 0; i < currentWave.count; i++)
                {
                    GameObject enemyGo = Instantiate(currentWave.enemyPrefab, spawnPoint.position, spawnPoint.rotation);
                    EnemyMover mover = enemyGo.GetComponent<EnemyMover>();
                    /*if (mover != null && targetTransform != null)
                    {
                        mover.SetTarget(targetTransform);
                    }*/

                    activeEnemyCount++;
                    UpdateWaveInfoUI();

                    yield return new WaitForSeconds(currentWave.spawnDelay);
                }

                // 3. 모든 스폰이 끝나고, 맵에 남은 에너미가 0이 될 때까지 대기
                while (activeEnemyCount > 0)
                {
                    yield return null;
                }

                Debug.Log($"<color=green>Wave {currentWaveIndex + 1} 전멸 완료!</color>");

                // 데이터 정비 (생존 라운드 수 증가)
                currentWaveIndex++;
                GameData.roundsSurvived++;

                // 4. 마지막 웨이브 유무 판별 (6번 규칙)
                if (currentWaveIndex >= waves.Length)
                {
                    Debug.Log("LEVEL CLEAR");
                    if (waveInfo != null) waveInfo.SetActive(false);
                    if (timerText != null) timerText.text = "";
                    break; // 모든 웨이브 종료이므로 마스터 루프 탈출
                }
                else
                {
                    // ★ [1, 2번 핵심 복구] 아직 다음 웨이브가 남아있다면? 
                    // 버튼으로 돌아가지 않고 "Wave 0/이전총수"를 유지한 상태로 중앙 상단에 5초 카운트다운 돌입!
                    yield return StartCoroutine(CountdownRoutine(5f));
                }
            }

            isWaveActive = false;
        }

        /// <summary>
        /// ★ [2번 요청사항 반영] 중앙 상단 카운트다운 실시간 처리 코루틴 (5, 4, 3, 2, 1)
        /// </summary>
        IEnumerator CountdownRoutine(float countdownTime)
        {
            float timer = countdownTime;

            while (timer > 0f)
            {
                if (timerText != null)
                {
                    // 소수점을 버리고 정수형태(5, 4, 3...)만 깔끔하게 표기합니다.
                    timerText.text = Mathf.CeilToInt(timer).ToString();
                }

                timer -= Time.deltaTime;
                yield return null;
            }

            // 카운트다운이 끝나면 타이머 텍스트는 다시 깨끗하게 비워줍니다.
            if (timerText != null)
            {
                timerText.text = "";
            }
        }

        /// <summary>
        /// 적 처치(Kill) 또는 탈출(Endpoint) 시 마릿수를 감소시키고 텍스트를 새로 그리는 함수
        /// </summary>
        public void OnEnemyDestroyed()
        {
            if (activeEnemyCount > 0)
            {
                activeEnemyCount--; // 적이 사라졌으므로 현재 마릿수 점차 감소
                UpdateWaveInfoUI(); // 바뀐 숫자로 UI 즉시 새로고침
            }
        }

        /// <summary>
        /// 자식 오브젝트 텍스트에 "Wave 현재웨이브(현재 마릿수/총 마릿수)" 형태로 실시간 매핑
        /// </summary>
        private void UpdateWaveInfoUI()
        {
            if (waveInfoText != null)
            {
                // 화면 표시용 웨이브 번호 (현재 인덱스 + 1)
                int waveDisplayNumber = currentWaveIndex + 1;

                // 출력 형식 예시: "Wave 1(5/5)" -> 적 전멸 시 "Wave 1(0/5)" 상태로 대기창 진입
                waveInfoText.text = $"Wave {waveDisplayNumber}({activeEnemyCount}/{totalWaveEnemyCount})";
            }
        }
    }
}