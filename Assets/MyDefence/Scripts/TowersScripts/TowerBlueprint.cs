using UnityEngine;

namespace MyDefence
{
    /// <summary>
    /// 과제 1) 상점에서 판매할 타워 품목의 정보를 담는 직렬화 클래스
    /// </summary>
    [System.Serializable]
    public class TowerBlueprint
    {
        public GameObject towerPrefab;  // 생성할 타워 프리팹
        public int cost;                // 타워 설치 비용
    }
}