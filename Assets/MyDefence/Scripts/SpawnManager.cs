using MyDefence;
using System.Collections;
using TMPro;
using UnityEngine;

namespace MyDefence
{
    public class SpawnManager : MonoBehaviour
    {
        [Header("스폰 설정")]
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private Transform targetTransform;

        [Header("UI 설정")]
        [SerializeField] private TMP_Text timerText;

        [Header("스폰 간격 설정")]
        [SerializeField] private float spawnDelay = 0.5f;

        private int currentWave = 1;

        void Start()
        {
            StartCoroutine(WaveRoutine());
        }

        IEnumerator WaveRoutine()
        {
            while (true)
            {
                // --- 소환을 시작하자마자(첫 몬스터 나오기 직전) 로그를 띄웁니다! ---
                Debug.Log($"[Wave {currentWave}] {currentWave}마리의 에너미가 스폰됩니다!");

                // --- 이번 웨이브에서 소환할 마리수를 미리 기억해 둡니다 ---
                int spawnCount = currentWave;

                // --- 에너미 스폰 루프 ---
                for (int i = 0; i < spawnCount; i++)
                {
                    // 1. 에너미를 생성하고 타겟을 지정합니다.
                    GameObject enemyGo = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
                    EnemyMover mover = enemyGo.GetComponent<EnemyMover>();
                    if (mover != null && targetTransform != null)
                    {
                        mover.SetTarget(targetTransform);
                    }

                    // 2. 다음 에너미 생성 전까지 지정된 딜레이만큼 대기합니다.
                    yield return new WaitForSeconds(spawnDelay);
                }

                // ★★★ [새로 추가된 곳] 모든 에너미가 소멸할 때까지 실시간 감시하며 대기 ★★★
                // GameObject.FindGameObjectsWithTag("Enemy").Length 는 현재 맵에 있는 에너미의 총 개수입니다.
                while (GameObject.FindGameObjectsWithTag("Enemy").Length > 0)
                {
                    // 아직 적이 남아있다면 딱 1프레임(약 0.01초)만 쉬고 다음 프레임에 다시 개수를 검사합니다.
                    // 이 덕분에 유니티가 멈추지(렉 걸리지) 않고 부드럽게 대기할 수 있어요!
                    yield return null;
                }

                // ★★★ [새로 추가된 곳] 위의 while문(대기)을 빠져나왔다는 건 적이 0마리가 되었다는 뜻! ★★★
                Debug.Log($"<color=green>{currentWave}웨이브 클리어!</color>");

                // --- 다음 웨이브 준비를 '소환이 모두 완전히 끝난 뒤'에 합니다 ---
                currentWave++;

                // --- 5초 카운트다운 타이머 구간 ---
                float countdown = 5.0f;
                while (countdown > 0)
                {
                    timerText.text = $"다음 웨이브까지: {Mathf.CeilToInt(countdown)}초";
                    yield return new WaitForSeconds(1.0f);
                    countdown -= 1.0f;
                }
            }
        }
    }
}