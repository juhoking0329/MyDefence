// 경로: Assets/MyDefence/Scripts/UIScripts/SceneFader.cs
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

namespace MyDefence
{
    /// <summary>
    /// 씬 페이더 기능 구현 클래스
    /// 씬 시작할때 페이드인 효과, 씬 종료시 페이드 아웃 효과, 페이드 아웃하면 다음 씬으로 이동
    /// </summary>
    public class SceneFader : MonoBehaviour
    {
        // 어디서나 공유할 수 있도록 싱글톤 인스턴스
        public static SceneFader instance;

        #region Variables
        public Image img;
        public AnimationCurve curve;
        public bool isFadeIn = false; // (에디터 확인용 변수)
        #endregion

        #region Unity Methods
        private void Awake()
        {
            // 싱글톤 초기화
            if (instance == null) instance = this;
            else Destroy(gameObject);
        }

        private void Start()
        {
            // [★요청사항 반영] 게임을 처음 켰을 때(메인메뉴 포함) 너무 갑자기 켜지지 않도록 
            // 0.2초 정도 아주 짧은 대기(delayTime)를 주고 페이드인이 시작되도록 세팅합니다.
            FadeStart(0.2f);
        }
        #endregion

        #region Custom Methods
        public void FadeStart(float delayTime = 0f)
        {
            StartCoroutine(FadeIn(delayTime));
        }

        // 과제 3번: FadeIn 기능 구현 (1초동안 검정에서 완전 투명하게 a:1 -> a:0)
        IEnumerator FadeIn(float delayTime)
        {
            // 시작 시 화면을 완전 검정색(a:1)으로 초기화
            img.color = new Color(0f, 0f, 0f, 1f);

            // 지정된 딜레이 시간이 있다면 검은 화면 상태로 대기합니다.
            if (delayTime > 0f)
            {
                yield return new WaitForSeconds(delayTime);
            }

            // 1초 동안 t값을 1에서 0으로 줄여나가며 알파값 조절
            float t = 1f;
            while (t > 0f)
            {
                t -= Time.deltaTime;
                float a = curve.Evaluate(t); // 커브를 이용해서 알파값 계산 (1 -> 0)
                img.color = new Color(0f, 0f, 0f, a);
                yield return null;
            }

            // 확실하게 투명화 마무리(안전장치)
            img.color = new Color(0f, 0f, 0f, 0f);
        }

        // 기존 빌드 인덱스(int) 버전 유지
        public void FadeTo(int builddex)
        {
            StartCoroutine(FadeOut(builddex));
        }

        // 편리하게 씬 이름(string)으로도 호출할 수 있게 오버로딩 함수
        public void FadeTo(string sceneName)
        {
            StartCoroutine(FadeOut(sceneName));
        }

        // 과제 4번: FadeOut 기능 구현 (1초동안 투명에서 완전 검정으로 a:0 -> a:1)
        IEnumerator FadeOut(int builddex)
        {
            // 1초 동안 t값을 0에서 1로 늘려나가며 알파값 조절
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime;

                // [★요청사항 반영] 커브 계산을 제거하고, 0에서 1로 정속으로 증가하는 t값을 알파값에 직접 대입합니다.
                img.color = new Color(0f, 0f, 0f, t);
                yield return null;
            }

            img.color = new Color(0f, 0f, 0f, 1f);

            // ★ [요청사항 반영] 페이드아웃(암전)이 완료된 후, 다음 씬으로 넘어가기 전 1초 동안 검은 화면 유지!
            yield return new WaitForSeconds(1.0f);

            // 페이드 아웃과 1초 대기가 모두 끝나면 다음 씬으로 이동
            if (builddex >= 0)
            {
                SceneManager.LoadScene(builddex);
            }
        }

        // 씬 이름 기반 페이드아웃 루틴 (동일하게 1초 딜레이 및 정속 페이드 적용)
        IEnumerator FadeOut(string sceneName)
        {
            // 1초 동안 t값을 0에서 1로 늘려나가며 알파값 조절
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime;

                // [★요청사항 반영] 커브 계산을 제거하고, 0에서 1로 정속으로 증가하는 t값을 알파값에 직접 대입합니다.
                img.color = new Color(0f, 0f, 0f, t);
                yield return null;
            }

            img.color = new Color(0f, 0f, 0f, 1f);

            // ★ [요청사항 반영] 플레이씬으로 넘어가기 전 암전 상태로 1초 동안 딜레이(숨고르기)
            yield return new WaitForSeconds(1.0f);

            SceneManager.LoadScene(sceneName);
        }
        #endregion
    }
}
