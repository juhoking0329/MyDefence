using TMPro.EditorUtilities;
using UnityEngine;

namespace MySample
{
    /// <summary>
    /// 타겟을 관리하는 클래스
    /// </summary>
    public class TargetTest : MonoBehaviour
    {
        #region Variables
        public int a = 10;
        private int b;
        #endregion

        #region Unity Events Methods
        private void Start()
        {
            //필드 초기화
            b = 30;
        }
        #endregion

        #region Custom Methods
        public void SetB(int _b)
        {
            b = _b;
        }

        public int GetB()
        {
            return b;
        }
        #endregion
    }
}