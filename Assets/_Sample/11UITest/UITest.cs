using UnityEngine;

namespace MyDefence
{
    public class UITest : MonoBehaviour
    {
        #region Custom Methods
        // Fire 버튼과 연동할 메서드
        public void Fire()
        {
            Debug.Log("발사 하였습니다");
        }

        // Jump 버튼과 연동할 메서드
        public void Jump()
        {
            Debug.Log("플레이어가 점프하였습니다");
        }
        #endregion
    }
}