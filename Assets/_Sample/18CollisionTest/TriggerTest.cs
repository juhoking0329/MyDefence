using UnityEngine;

namespace MySample
{
    public class TriggerTest : MonoBehaviour
    {
        public float movePower = 10f;

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log($"OnTriggerEnter : {other.tag}");

            MoveObject moveObject = other.GetComponent<MoveObject>();
            if (moveObject != null)
            {
                moveObject.MoveRight(movePower); // ✅ 왼쪽 트리거 진입 → 오른쪽으로 튕김
                moveObject.ChangeMoveColor();
            }
        }

        private void OnTriggerStay(Collider other)
        {
            Debug.Log($"OnTriggerStay : {other.tag}");
        }

        private void OnTriggerExit(Collider other)
        {
            Debug.Log($"OnTriggerExit : {other.tag}");

            MoveObject moveObject = other.GetComponent<MoveObject>();
            if (moveObject != null)
            {
                moveObject.ChangeOriginalColor(); // ✅ 벗어나면 색 원복
            }
        }
    }
}