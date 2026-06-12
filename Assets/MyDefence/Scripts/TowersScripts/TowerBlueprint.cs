using UnityEngine;

namespace MyDefence
{
    [System.Serializable]
    public class TowerBlueprint
    {
        [Header("1단계 기본 데이터")]
        public GameObject towerPrefab;
        public int cost;

        [Header("2단계 업그레이드 데이터 [★5-1 추가]")]
        public GameObject upgradePrefab; // 업그레이드될 머신건_2 프리팹
        public int upgradeCost;          // 업그레이드 가격 (150 Gold)
    }
}