// 경로: Assets/MyDefence/Scripts/UIScripts/MainMenu.cs
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MyDefence
{
    /// <summary>
    /// 메인메뉴를 관리하는 클래스
    /// </summary>
    public class MainMenu : MonoBehaviour
    {
        #region Variables
        //다음 씬 이름
        [SerializeField]
        private string loadToScene = "PlayScene";
        #endregion

        #region Custom Method
        //플레이 버튼 클릭시 호출
        public void Play()
        {
            // ★ [페이드 연동] 씬 페이더가 존재한다면 페이드 아웃 연출 후 인게임 씬으로 이동합니다.
            if (SceneFader.instance != null)
            {
                SceneFader.instance.FadeTo(loadToScene);
            }
            else
            {
                // 혹시 메인메뉴 씬에 SceneFader 오브젝트가 없을 때를 대비한 방어 코드
                SceneManager.LoadScene(loadToScene);
            }
        }

        //게임 종료 버튼 클릭시 호출
        public void Quit()
        {
            Debug.Log("Game Quit");
            //어플 종료 명령 (에디터에서는 명령 무시, 실행파일에서는 명령 시행)
            Application.Quit();
        }
        #endregion
    }
}   