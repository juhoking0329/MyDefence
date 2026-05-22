using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float speed = 5.0f; // 카메라가 움직이는 속도입니다.

    void Update()
    {
        // 1. 키보드 입력 받기 (W, S, Up, Down -> 앞뒤 / A, D, Left, Right -> 좌우)
        // 입력을 받으면 -1.0에서 1.0 사이의 숫자가 반환돼요!
        float horizontal = Input.GetAxis("Horizontal"); // 좌우 움직임
        float vertical = Input.GetAxis("Vertical");     // 앞뒤 움직임

        // 2. 입력받은 방향으로 '방향 벡터' 만들기
        // Vector3(X축, Y축, Z축)인데, 우리는 좌우(X)와 앞뒤(Z)로만 움직일 거예요.
        Vector3 moveDirection = new Vector3(horizontal, 0, vertical);

        // 3. 카메라 이동시키기
        // normalized는 대각선으로 갈 때 너무 빨라지지 않게 '크기를 1로' 맞춰주는 꿀팁입니다.
        transform.Translate(moveDirection.normalized * speed * Time.deltaTime);
    }
}