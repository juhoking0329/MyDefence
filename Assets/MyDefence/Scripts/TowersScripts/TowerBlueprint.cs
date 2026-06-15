using UnityEngine;

namespace MyDefence
{
    [System.Serializable]
    public class TowerBlueprint
    {
        [Header("1단계 기본 데이터")]
        public GameObject towerPrefab;
        public int cost;

        [Header("2단계 업그레이드 데이터")]
        public GameObject upgradePrefab;
        public int upgradeCost;

        [Header("3단계 업그레이드 데이터")]
        public GameObject upgradePrefab2;  // ← 추가
        public int upgradeCost2;           // ← 추가
    }
}