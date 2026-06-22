using UnityEngine;

namespace MySample
{
    public class MoveObject : MonoBehaviour
    {
        #region Variables
        private Rigidbody rb;
        private Material material;
        private Color originalColor;

        [SerializeField] private float movePower = 10f;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            material = GetComponent<Renderer>().material;
            originalColor = material.color;
        }

        private void FixedUpdate()
        {
            // 입력값 기반으로 좌우 이동 (Update 제거하고 여기서 처리)
            float moveX = Input.GetAxis("Horizontal");
            rb.AddForce(Vector3.right * moveX * movePower, ForceMode.Force);
        }
        #endregion

        #region Custom Method
        public void MoveRight(float power)
        {
            rb.AddForce(Vector3.right * power, ForceMode.Impulse); // ✅ power 파라미터 사용
        }

        public void MoveLeft(float power)
        {
            rb.AddForce(Vector3.left * power, ForceMode.Impulse); // ✅ power 파라미터 사용
        }

        public void ChangeMoveColor()
        {
            material.color = Color.red;
        }

        public void ChangeOriginalColor()
        {
            material.color = originalColor;
        }
        #endregion
    }
}