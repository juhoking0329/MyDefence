using Unity.Hierarchy;
using UnityEngine;

namespace MyDefence
{
    /// <summary>
    /// 타워를 관리하는 클래스
    /// </summary>
    public class Tower : MonoBehaviour
    {
        #region Variables
        private GameObject target;          //공격 범위안에 있는 가장 가까운 적
        public float attackRange = 7.0f;    //타워 공격 범위    

        //타워 회전
        public Transform partToRotate;      //타워의 회전을 관리하는 오브젝트
        public float turnSpeed = 10f;

        //SearchTimer 0.2초
        public float searchTimer = 0.2f;
        private float countdown = 0f;

        //발사 타이머 1초에 한발씩
        public float fireTimer = 1.0f;
        private float fireCountdown = 0f;

        //탄환 발사
        private GameObject bulletPrefab;        //탄환 오브젝트 프리팹   
        public Transform firePoint;             //탄환 발사 위치
        #endregion

        #region Unity Events Methods
        private void Update()
        {
            //0.2초마다 한번씩 공격 범위안에 있는 가장 가까운 적 찾기
            countdown += Time.deltaTime;
            if (countdown >= searchTimer)
            {
                //타이머 실행문
                UpdateTarget();

                //타이머 초기화
                countdown = 0f;
            }

            //타겟을 못 찾았으면
            if (target == null)
                return;

            //

            fireCountdown += Time.deltaTime;
            if(fireCountdown >= fireTimer)
            {
                //Debug.Log("Shoot");
                Shoot();
                fireCountdown = 0f; fireTimer = 1.0f;
            }

            //타겟(가장 가까운 Enemy)의 움직임에 따라 터렛 헤드가 타겟 방향으로 회전한다
            Vector3 dir = target.transform.position - partToRotate.position;
            //목표 방향에 해당되는 회전값 구하기
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            partToRotate.rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed);

        }

        //해당(this, 스크립트가 붙어있는) 오브젝트를 선택했을때만 기즈모를 그린다.
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            //타워를 중심으로 반경이 7인 구를 그린다
            //Gizmos.DrawSphere(transform.position, attackRange);
            //타워를 중심으로 반경이 7인 와이어 구를 그린다
            Gizmos.DrawWireSphere(transform.position, attackRange);     
        }
        #endregion
        #region 
        /* //항상 기즈모를 그린다
        private void OnDrawGizmos() 
        {
        
        }
        */
        #endregion

        #region Custom Method
        //3. 터렛에서 가장 가까운 적 찾아(Tag "Enemy") 타겟으로 설정
        void UpdateTarget()
        {
            GameObject[] enemies;
            enemies = GameObject.FindGameObjectsWithTag("Enemy");

            GameObject closest = null;          //가장 가까운 enemy
            float minDistance = Mathf.Infinity;    //최소 거리

            foreach (GameObject enemy in enemies)
            {
                //적과의 거리 구하기
                float distanceToEnemy = Vector3.Distance(enemy.transform.position, this.transform.position);
                if(distanceToEnemy < minDistance)
                {
                    minDistance = distanceToEnemy;
                    closest = enemy;        //최소 거리에 해당되는 적
                }
            }

            //closest 검증
            if (closest != null && minDistance <= attackRange)
            {
                target = closest;
            }
            else
            {
                target = null;
            }
        }

        /*//타겟(가장 가까운 Enemy)의 움직임에 따라 터렛 헤드가 타겟 방향으로 회전한다
        Vector3 dir = target.transform.position - partToRotate.position;
        //목표 방향에 해당되는 회전값 구하기
        private Quaternion lookRotation = Quaternion.LookRotation(dir);
        */

        //탄환 발사
        void Shoot()
        {
            //Debug.Log("Shoot");
            //총구 위치와 회전값에 탄환 프리팹 사본 생성(Instantiate)
            GameObject bulletGo = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            //탄환 오브젝트에 부착되어 있는 Bullet 클래스의 인스턴스 가져오기
            Bullet bullet = bulletGo.GetComponent<Bullet>();

            //타겟정보를 bullet에게 넘겨준다
            if (bullet != null)
            {
                bullet.SetTarget(target.transform);
            }
        }
        #endregion
    }
}