using UnityEngine;
using UnityEngine.InputSystem;

namespace MySample
{
    /// <summary>
    /// 게임중 유저 인풋 값을 New Input System 가져와서 적용하기
    /// 2) SendMessage 방법
    /// </summary>
    public class NewInputTest2 : MonoBehaviour
    {
        #region Variables
        private NewActionsTest inputActions;

        public float moveSpeed = 10f;
        private Vector2 inputVector = Vector2.zero;

        public float bordor = 15f;
        private Vector2 mousePosition = Vector2.zero;

        private bool isCannotMove = false;
        #endregion

        #region Unity Event Method
        private void Update()
        {
            Vector3 dir = new Vector3(inputVector.x, 0f, inputVector.y);
            this.transform.Translate(dir * Time.deltaTime * moveSpeed, Space.World);
        }
        #endregion

        #region Custom Method
        public void OnMove(InputValue value)
        {
            inputVector = value.Get<Vector2>();
        }

        public void OnmousePosition(InputValue value)
        {

        }
        #endregion
    }
}