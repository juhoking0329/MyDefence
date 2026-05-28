using UnityEngine;

namespace MySample
{
    /// <summary>
    /// 회전 테스트 예제 스크립트
    /// </summary>
    public class RotateTest : MonoBehaviour
    {
        private const int V = 0;
        #region Variables
        //회전 속도 
        public float turnSpeed = 5f;

        //이동 속도
        public float moveSpeed = 5f;

        //회전 값 변수
        private float x = V;

        public RotateTest(float x)
        {
            this.x = x;
        }

        //목표 오브젝트 트랜스폼 인스턴스
        public Transform target;
        #endregion


        #region Unity Events Methods
        private void Start()
        {
            //this.transform.rotation = Quaternion.Euler(0f, 90f, 0f);  // y축 회전하여 오른쪽 바라보기
            //this.transform.rotation = Quaternion.Euler(90f, 0f, 0f);    // x축 회전하여 위쪽 바라보기
            //this.transform.rotation = Quaternion.Euler(0f, 0f, 90f);    // z축 회전하여 앞쪽 바라보기
        }
        #endregion

        #region
        private void Update()
        {
            //축 회전
            //x += 1;
            //this.transform.rotation = Quaternion.Euler(x, 0, 0);      //x축
            //this.transform.rotation = Quaternion.Euler(0, x, 0);      //y축
            //this.transform.rotation = Quaternion.Euler(0, 0, x);      //z축

            //[1]Rotate 함수 사용(지구의 자전)
            //this.transform.Rotate(Vector3.right * turnSpeed * Time.deltaTime);
            //this.transform.Rotate(Vector3.up * turnSpeed * Time.deltaTime);
            //[1-1]RotateAround 함수 사용(지구의 공전)
            //this.transform.RotateAround(target.position, Vector3.up, turnSpeed * Time.deltaTime);

            /*//[2]원하는(목표) 방량으로 회전
            //목표 방향 구하기
            Vector3 dir = target.position - this.transform.position;
            //목표 방향에 해당되는 회전값 구하기
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            //트랜스폼의 회전값을 구한 회전값에 대입
            //this.transform.rotation = lookRotation;

            //this.transform.rotation (0,0,0) => lookRotation (0,41,0)
            //Quaternion.Lerp(Quaternion a, Quaternion b, float t) : a에서 b로 t의 비율만큼 회전값을 보간하여 반환하는 함수
            //this.transform.rotation = qRotation;

            Quaternion qRotation = Quaternion.Lerp(this.transform.rotation, lookRotation, turnSpeed * Time.deltaTime);
            //Quaternion로 부터 오일러값(x, y, z) 구하기
            Vector3 euler = qRotation.eulerAngles;
            //y축 회전하는 회전값을 구한다
            this.transform.rotation = Quaternion.Euler(0f, euler.y, 0f);*/

            //이동 dir * Time.deltaTime * moveSpeed
            Vector3 dir = target.position - this.transform.position;
            this.transform.rotation = Quaternion.LookRotation(dir);
            this.transform.Translate(dir.normalized * moveSpeed * Time.deltaTime, Space.World);
            //this.transform.Translate(dir.normalized * moveSpeed * Time.deltaTime, Space.Self);

        }
        #endregion
    }
}

/*

a = 0, b = 10, t = 0.1
a = Lerp(a, b, 0.1);


a = 1, b = 10, t = 0.1
a = Lerp(a, b, 0.1);

a = 1.9, b = 10, t = 0.1

*/