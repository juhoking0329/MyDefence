using UnityEngine;

public class BuildManager : MonoBehaviour
{
    // [핵심] 외부에서 드래그 없이 바로 접근할 수 있도록 공유 주소(Static instance)를 만듭니다.
    public static BuildManager instance;

    [Header("설치할 터렛 프리팹")]
    [SerializeField] private GameObject machineGunTurretPrefab; // 머신건 타워 프리팹

    void Awake()
    {
        // 싱글톤 패턴의 핵심 안전장치: 이 세상에 나 하나만 존재하도록 고정합니다.
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // 혹시나 복사본이 생기면 파괴
        }
    }

    // 타일이 "여기 터렛 좀 지어줘!" 하고 호출할 함수입니다.
    public void BuildTurretOn(Transform tileTransform)
    {
        if (machineGunTurretPrefab == null) return;

        // 타일의 위치(position)에 터렛의 원래 회전값(rotation)으로 터렛을 소환합니다.
        // 타일 바로 위에 예쁘게 얹어지도록 Y축 높이를 살짝 올려서 소환해도 좋습니다.
        Vector3 spawnPosition = tileTransform.position + new Vector3(0, 0.5f, 0);
        Instantiate(machineGunTurretPrefab, spawnPosition, machineGunTurretPrefab.transform.rotation);

        Debug.Log("마우스 클릭 - 여기에 터렛 설치 완료!");
    }
}