using System.Collections;
using UnityEngine;
using TMPro; // UI TextMeshPro를 사용하기 위해 꼭 필요해요!

public class SpawnManager : MonoBehaviour
{
    [Header("스폰 설정")]
    [SerializeField] private GameObject enemyPrefab;  // 소환할 Enemy 프리팹
    [SerializeField] private Transform spawnPoint;    // 소환할 시작지점(시점)의 위치

    [Header("UI 설정")]
    [SerializeField] private TMP_Text timerText;       // 화면에 시간을 표시할 텍스트 컴포넌트

    private int currentWave = 1; // 현재 웨이브 번호 (1번 웨이브부터 시작)

    void Start()
    {
        // 게임이 시작되면 자동으로 '웨이브 루프 코루틴'을 실행합니다.
        StartCoroutine(WaveRoutine());
    }

    // [핵심 스킬] 전체 웨이브와 타이머를 관리하는 코루틴 함수입니다.
    IEnumerator WaveRoutine()
    {
        // 게임이 실행되는 동안 무한히 반복합니다 (1웨이브, 2웨이브, 3웨이브...)
        while (true)
        {
            // --- [과제 3번] 해당 웨이브의 마리수만큼 Enemy 스폰 ---
            // currentWave가 1이면 1마리, 2면 2마리를 반복문을 통해 소환합니다.
            for (int i = 0; i < currentWave; i++)
            {
                // [핵심 스킬] Instantiate를 이용해 시작지점에 Enemy 프리팹을 생성합니다.
                Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            }

            Debug.Log($"[Wave {currentWave}] {currentWave}마리의 에너미가 스폰되었습니다!");

            // 다음 웨이브에는 소환될 마리수가 1마리 더 늘어나도록 증가시킵니다.
            currentWave++;

            // --- [과제 4번] 5초 카운트다운 타이머 구간 ---
            float countdown = 5.0f;

            // 5초 동안 매초(1초마다) UI 텍스트를 갱신하며 기다립니다.
            while (countdown > 0)
            {
                // UI 텍스트에 남은 시간을 정수 형태로 예쁘게 찍어줍니다 (5, 4, 3, 2, 1)
                timerText.text = $"다음 웨이브까지: {Mathf.CeilToInt(countdown)}초";

                // 1초 동안 이 함수의 실행을 잠시 멈추고 유니티에게 주도권을 넘깁니다.
                yield return new WaitForSeconds(1.0f);

                // 시간이 1초씩 줄어듭니다.
                countdown -= 1.0f;
            }

            // 5초 카운트다운이 끝나면 위쪽의 while(true) 처음으로 돌아가서 다음 웨이브를 시작합니다!
        }
    }
}