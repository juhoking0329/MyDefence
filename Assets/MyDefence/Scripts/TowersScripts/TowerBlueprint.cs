using UnityEngine;

namespace MyDefence
{
    /// <summary>
    /// 과제 1) 상점에서 판매할 타워 품목의 정보를 담는 직렬화 클래스
    /// </summary>
    [System.Serializable]
    public class TowerBlueprint
    {
        #region Variables
        [Header("타워 정보")]
        public GameObject towerPrefab;  // 설치할 타워 프리팹
        public int cost;                // 설치 가격 (머신건: 100, 로켓: 250)
        #endregion
    }
}