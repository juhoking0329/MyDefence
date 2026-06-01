using UnityEngine;

namespace MySample
{
    /// <summary>
    /// 기본 클래스의 싱글톤 패턴
    /// </summary>
    public class SingletonClass
    {
        // SingletonClass 클래스의 인스턴스(객체) 정적(static) 변수 선언
        private static SingletonClass instance;

        //public한 속성으로 private한 instance에 전역적으로 접근하기
        public static SingletonClass Instance
        {
            get
            {
                // instance가 null이면, 즉 객체가 생성되지 않았으면, 객체를 생성한다
                if (instance == null)
                {
                    //인스턴스 생성
                    instance = new SingletonClass();
                }
                // instance가 null이 아니면, 즉 객체가 이미 생성되어 있으면, 기존 객체를 반환한다
                return instance;
            }
        }


        //필드 : 인스턴스이름.number -> Instance.number      
        public int number;


    }
}