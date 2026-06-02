using UnityEngine;

namespace MySample
{
    /// <summary>
    /// MonoBehaviour를 상속받은 클래스의 싱글톤 패턴 디자인
    /// 게임오브젝트에 부착되는 싱글톤 패턴 클래스 디자인
    /// 하이라키 창에서 싱글톤 패턴 클래스가 부착되어 있는 게임오브젝트가 1개만 존재하도록 디자인
    /// </summary>
    public class SingletonMono : MonoBehaviour
    {
        //singletonMono 클래스의 인스턴스(객체) 정적(static) 변수 선언
        private static SingletonMono instance;

        //public한 속성으로 private한 instance에 전역적으로 접근하기
        public static SingletonMono Instance
        {
            get
            {
                return instance;
            }
        }

        //가장 먼저 호출되는 함수 - 오브젝트가 하나만 존재하게 해준다
        private void Awake()
        {
            // 오브젝트 존재 체크
            if (instance != null)
            {
                Destroy(this.gameObject);
                return;
            }

            //null 이면 
            instance = this;

            //씬이 바뀌어도 오브젝트가 파괴되지 않도록 한다
            //DontDestroyOnLoad(this.gameObject);
        }

        //필드변수, 필드 : 인스턴스이름.number -> Instance.number
        public int number;





    }
}