using UnityEngine;

namespace MyDefence
{
    /// <summary>
    /// 과제 3) 게임의 전반적인 데이터(재화 등)를 관리하는 클래스
    /// </summary>
    public class GameData : MonoBehaviour
    {
        #region Variables
        // static으로 선언하여 어떤 스크립트에서도 GameData.money로 쉽게 접근할 수 있게 합니다.
        public static int money = 400; // 과제 3) 소지금 초기값 400
        #endregion
    }
}