using UnityEngine;
using UnityEngine.UI;

namespace MyDefence
{
    /// <summary>
    /// LevelSelect(레벨 선택) 씬을 관리하는 클래스
    /// </summary>
    public class LevelSelect : MonoBehaviour
    {
        #region Variables        
        // 씬페이더 인스턴스
        public SceneFader fader;

        // 레벨 버튼의 부모 트랜스폼 인스턴스 (Content 오브젝트)
        public Transform content;

        // 레벨 버튼 배열 인스턴스 선언
        private Button[] levelButtons;

        // ★ [추가] 유저가 현재 도달한 최대 플레이 가능 레벨 (기본값 1)
        private int nowLevel = 1;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            // ★ [추가] 1. 저장된 유저 데이터(도달한 레벨) 불러오기 (데이터 없으면 기본값 1)
            nowLevel = PlayerPrefs.GetInt("ReachedLevel", 1);
            Debug.Log($"💾 [Load] 유저가 플레이 가능한 현재 Max 레벨(nowLevel): {nowLevel}");

            // 레벨 버튼 배열 초기화
            levelButtons = new Button[content.childCount];

            for (int i = 0; i < levelButtons.Length; i++)
            {
                Transform child = content.GetChild(i);
                levelButtons[i] = child.GetComponent<Button>();

                // ★ [과제 3, 4, 5번 반영] 
                // 배열 인덱스는 0부터 시작하므로 레벨 번호는 (i + 1)입니다.
                int levelNum = i + 1;

                // 내가 가려는 레벨이 유저가 도달한 최대 레벨(nowLevel)보다 높다면 잠궈버립니다.
                if (levelNum > nowLevel)
                {
                    levelButtons[i].interactable = false; // Lock (비활성화)
                }
                else
                {
                    levelButtons[i].interactable = true;  // Unlock (활성화)
                }
            }
        }
        #endregion

        #region Custom Method
        // 레벨 버튼 선택시 호출되는 메서드
        public void LevelButton(string loadToScene)
        {
            // ★ [과제 2번 요구사항] 버튼 클릭 시 로그 출력 분기 처리
            // 문자열에서 숫자만 쏙 뽑아서 로그를 찍어줍니다 (예: "Level01" -> "Level 1 버튼 클릭")
            string displayNum = loadToScene.Replace("Level0", " ").Replace("Level", " ");
            Debug.Log($"Level{displayNum} 버튼 클릭");

            // 선택한 레벨 씬으로 이동
            if (fader != null)
            {
                fader.FadeTo(loadToScene);
            }
            else
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(loadToScene);
            }
        }

        // 뒤로가기 버튼 선택시 호출되는 메서드
        public void BackButton()
        {
            // 메인메뉴 씬으로 이동
            if (fader != null)
            {
                fader.FadeTo("MainMenu");
            }
            else
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
            }
        }

        // ★ [테스트용 치트키] 세이브 데이터를 리셋하고 싶을 때 인스펙터 컴포넌트 우클릭으로 실행 가능
        [ContextMenu("Clear Save Data")]
        public void ClearSaveData()
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("🔄 세이브 데이터가 완전히 초기화되었습니다. (1레벨 빼고 다 잠김)");
        }
        #endregion
    }
}


/*
1. 유저 게임 데이터 : 유저가 게임 플레이하면 생산한 데이터
- 게임 어플리케이션 종료시에도 유지되어야 한다
- 세이브/로드
- 서버 저장, 파일시스템, PlayerPrefs

2. 세이브/로드 정책
1) 저장할 데이터
 - 클리어 레벨

2) 세이브 시점
 - 레벨 클리어시

3) 로드 시점
 - 레벨 셀렉트 씬 시작할때

3. 게임 데이터 로드시 체크 사항
- 저장파일 유무 체크
파일이 없으면 : 저장할 데이터를 설정 값으로 초기화
파일이 있으면 : 파일을 읽어서 읽어들인 값으로 초기화

4. 게임 데이터 세이브시 체크 사항
- 생산한 데이터와 저장된 데이터를 비교해서 저장해야 되는것을 체크
- 클리어 레벨 데이터는 저장된 데이터와 비교해서 저장된 데이터보다 크면 저장한다


PlayerPrefs save/load
PlayerPrefs.SetInt(KeyName, Value); //KeyName 이름으로 정수형 value 값 저장
PlayerPrefs.GetInt(KeyName);        //KeyName 이름으로 저장된 정수형 value 값 가져오기

PlayerPrefs.Setfloat(KeyName, Value); //KeyName 이름으로 실수형 value 값 저장
PlayerPrefs.Getfloat(KeyName);        //KeyName 이름으로 저장된 실수형 value 값 가져오기

PlayerPrefs.SetString(KeyName, Value); //KeyName 이름으로 문자열 value 값 저장
PlayerPrefs.GetString(KeyName);        //KeyName 이름으로 저장된 문자열 value 값 가져오기
*/