using UnityEngine;

namespace MySample
{
    public class CollisionTest : MonoBehaviour
    {
        public float movePower = 10f;

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log($"OnCollisionEnter : {collision.gameObject.tag}");

            MoveObject moveObject = collision.gameObject.GetComponent<MoveObject>();
            if (moveObject != null)
            {
                moveObject.MoveLeft(movePower); // ✅ 오른쪽 벽 충돌 → 왼쪽으로 튕김
                moveObject.ChangeMoveColor();
            }
        }

        private void OnCollisionStay(Collision collision)
        {
            Debug.Log($"OnCollisionStay : {collision.gameObject.tag}");
        }

        private void OnCollisionExit(Collision collision)
        {
            Debug.Log($"OnCollisionExit : {collision.gameObject.tag}");

            MoveObject moveObject = collision.gameObject.GetComponent<MoveObject>();
            if (moveObject != null)
            {
                moveObject.ChangeOriginalColor(); // ✅ 벗어나면 색 원복
            }
        }
    }
}