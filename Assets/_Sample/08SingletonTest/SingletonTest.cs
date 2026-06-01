using UnityEngine;

namespace MySample
{
    /// <summary>
    /// 싱글톤 패턴 연습 클래스
    /// </summary>
    public class SingletonTest : MonoBehaviour
    {
        #region Unity Event Methods
        private void Start()
        {
            // 싱글톤 클래스의 인스턴스(객체) 가져오기
            SingletonClass.Instance.number = 18;
            // 싱글톤 클래스의 인스턴스(객체)의 number 필드 값 출력하기
            Debug.Log(SingletonClass.Instance.number.ToString());
        }
        #endregion
    }
}

/*
디자인 패턴

싱글톤(Singleton) 패턴
: 프로젝트 내에서 하나의 인스턴스만 존재하게 한다, new를 한번만 한다
: 클래스의 인스턴스에게 전역적인(어디서나) 접근을 제공한다, 인스턴스 변수를 static으로 선언

: 싱글톤 클래스의 인스턴스 변수는 자신 클래스의 코드블록 안에서 선언하고 객체를 가져온다

*/