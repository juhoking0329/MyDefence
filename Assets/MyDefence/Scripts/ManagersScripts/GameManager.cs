using UnityEngine;

namespace MyDefence
{
    /// <summary>
    /// 과제 2) 시스템 치트키 및 전반적인 게임 규칙을 관리하는 클래스
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        #region Unity Event Methods
        void Update()
        {
            if (GameData.isGameOver) return; // 게임오버 시 모든 입력 차단

            // 과제 2) M키를 누르면 100,000 골드를 보너스로 지급하는 치트키
            if (Input.GetKeyDown(KeyCode.M))
            {
                GameData.money += 100000;
                Debug.Log($"치트키 작동! 100,000 Gold가 지급되었습니다. 현재 돈: {GameData.money}");
            }
        }
        #endregion
    }
}