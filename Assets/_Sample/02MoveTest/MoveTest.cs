using UnityEngine;

namespace MySample
{
    //게임오브젝트의 이동
    public class MoveTest : MonoBehaviour
    {
        //이동 목표지점 변수 선언 및 초기화
        private Vector3 targetPosition = new Vector3(7f, 1f, 8f);

        //이동 목표 위치에 있는 오브젝트의 트랜스폼 인스턴스 생성, 선언(new)
        public Transform target;

        //이동 속도를 저장하는 변수를 선언하고 초기화
        public float Speed = 10f;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            //this.gameObject, gameObject : MoveTest 스크립트가 붙어있는 게임 오브젝트의 객체(인스턴스)
            //this.gameObject.transform, gameObject.transform,
            //      : MoveTest 스크립트가 붙어있는 게임 오브젝트의 Transform의 객체(인스턴스)
            //this.transform, transform
            //this.transform.position = new Vector3(7f, 1f, 8f);
            //this.gameObject.transform.position = new Vector3(7f, 1f, 8f);

            //this.transform.position = targetPosition;

            //Debug.Log($"타겟 위치 : {target.position}");
            //this.transform.position = target.position;

        }

        // Update is called once per frame
        void Update()
        {
            //플레이어의 위치를 앞으로 이동 : z축 방향으로 이동
            //this.transform.position z축 값 증가 연산 Vector3 연산
            //this.transform.position.z = this.transform.position.z + 1.0f; error
            //this.transform.position = this.transform.position + new Vector3(0f, 0f, 1f);

            //앞,뒤,좌,우,위,아래
            //this.transform.position += Vector3.forward; //앞 Vector3(0f, 0f, 1f)
            //this.transform.position += Vector3.back;      //뒤 Vector3(0f, 0f, -1f)
            //this.transform.position += Vector3.left;      //좌 Vector3(-1f, 0f, 0f)
            //this.transform.position += Vector3.right;     //우 Vector3(1f, 0f, 0f)
            //this.transform.position += Vector3.up;        //위 Vector3(0f, 1f, 0f)
            //this.transform.position += Vector3.down;      //아래 Vector3(0f, -1f, 0f)

            //Vector3.one : Vector3(1f, 1f, 1f); - 단위 벡터   
            //Vector3.zero : Vector3(0f, 0f, 0f); - 초기값

            //앞방향으로 1초에 1unity 이동 
            //this.transform.position += Vector3.forward * Time.deltaTime;

            //앞방향으로 1초에 speed 만큼씩 이동
            //this.transform.position += new Vector3(0f, 0f, 1f) * Time.deltaTime * Speed;
            this.transform.position += Vector3.forward * Time.deltaTime * Speed;

            //이동 요소
            //방향 : 이동할 방향 지정(앞, 뒤, 좌, 우, 위, 아래)
            //Time.deltaTime : 동일한 시간에 동일한 거리를 이동하게 해준다
            //속도(Speed) : 이동 속도, 1초에 Speed 만큼 이동

            //Translate() : 이동 방향과 거리를 벡터로 지정하여 이동하는 함수
            //transform.Translate(Vector3.forward * Time.deltaTime * Speed);

            //타겟까지 이동(dir(방향), Time.deltaTime, Speed)
            //dir.normalized : dir 방향으로 크기 1인 벡터, 단위벡터, 정규화된 벡터
            //dir.magnitude : dir 벡터의 크기, 길이
            //이동 방향 구하기 : 타겟 위치 - 현재 위치, 도착 예정위치 - 출발(현재) 위치
            Vector3 dir = target.position - this.transform.position;
            //this.transform.Translate(dir.normalized * Time.deltaTime * Speed);
            //this.transform.Translate(dir.normalized * Time.deltaTime * Speed, Space.Self);
            this.transform.Translate(dir.normalized * Time.deltaTime * Speed, Space.World);

            //Space.Self, Space.World
            //this.transform.Translate(Vector3.forward * Time.deltaTime * Speed, Space.World);
            //this.transform.Translate(Vector3.forward * Time.deltaTime * Speed, Space.Self);

        }
    }
}

/*
n 프레임 : 초당 n번 실행(보여주기)
20 프레임 : 초당 20번 실행
20프레임이면 1 프레임당 걸리는 시간? : 1초 / 20프레임 = 0.05초

Time.deltaTime : 실제 1 프레임에 걸리는 시간(초 단위)

성능이 좋은 컴(PC1)
10 프레임
- Time.deltaTime을 고려하지 않을 경우 1초에 10만큼 이동
- Time.deltaTime을 고려할 경우 ( * Time.deltaTime) : 1초에 1만큼 이동

Time.deltaTime : 0.1초

this.transform.position += new Vector3(0f, 0f, 1f) * Time.deltaTime;    //0.1씩 증가
this.transform.position += new Vector3(0f, 0f, 1f) * Time.deltaTime;    //0.1씩 증가
this.transform.position += new Vector3(0f, 0f, 1f) * Time.deltaTime;    //0.1씩 증가
this.transform.position += new Vector3(0f, 0f, 1f) * Time.deltaTime;    //0.1씩 증가
this.transform.position += new Vector3(0f, 0f, 1f) * Time.deltaTime;    //0.1씩 증가
this.transform.position += new Vector3(0f, 0f, 1f) * Time.deltaTime;    //0.1씩 증가
this.transform.position += new Vector3(0f, 0f, 1f) * Time.deltaTime;    //0.1씩 증가
this.transform.position += new Vector3(0f, 0f, 1f) * Time.deltaTime;    //0.1씩 증가
this.transform.position += new Vector3(0f, 0f, 1f) * Time.deltaTime;    //0.1씩 증가
this.transform.position += new Vector3(0f, 0f, 1f) * Time.deltaTime;    //0.1씩 증가


성능이 나쁜 컴(PC2)
2 프레임
- Time.deltaTime을 고려하지 않을 경우 1초에 2만큼 이동
- Time.deltaTime을 고려할 경우 ( * Time.deltaTime) : 1초에 1만큼 이동

Time.deltaTime : 0.5초

this.transform.position += new Vector3(0f, 0f, 1f) * Time.deltaTime;    //0.5씩 증가
this.transform.position += new Vector3(0f, 0f, 1f) * Time.deltaTime;    //0.5씩 증가

*/