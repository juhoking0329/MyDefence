using UnityEngine;

namespace MyDefence
{
    /// <summary>
    /// 오브젝트 항상 카메라를 바라보도록 한다
    /// </summary>
    public class LookAtCamera : MonoBehaviour
    {
        #region Variables
        //메인카메라와 인스턴스
        private Camera mainCamera;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //참조
            mainCamera = Camera.main;
        }

        private void Update()
        {
            //항상 카메라를 바라본다
            transform.LookAt(mainCamera.transform.position);


        }
        #endregion
    }
}