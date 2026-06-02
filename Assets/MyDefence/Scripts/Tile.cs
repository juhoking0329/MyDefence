using MyDefence;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("타일 상태별 머티리얼 설정")]
    [SerializeField] private Material normalMaterial; // 기본 머티리얼 (흰색 계열)
    [SerializeField] private Material hoverMaterial;  // 마우스 올렸을 때 (파란색 계열)

    private Renderer rend;
    private bool isTurretBuilt = false;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend == null) return;

        // 게임이 시작되면 인스펙터에 넣은 기본 머티리얼(흰색)로 옷을 입힙니다.
        if (normalMaterial != null)
        {
            rend.material = normalMaterial;
        }
    }

    // 1. 마우스 커서를 타일 위에 올렸을 때 (IPointerEnter)
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 이미 터렛이 설치되어 있다면 마우스를 올려도 아무 반응 없도록 리턴!
        if (isTurretBuilt) return;

        // 마우스를 올리면 파란색 머티리얼로 갈아 끼우기
        if (hoverMaterial != null)
        {
            rend.material = hoverMaterial;
        }
    }

    // 2. 마우스 커서가 타일 밖으로 나갔을 때 (IPointerExit)
    public void OnPointerExit(PointerEventData eventData)
    {
        // 이미 터렛이 설치되어 있다면 색이 변하지 않도록 리턴!
        if (isTurretBuilt) return;

        // 마우스를 치우면 다시 원래 기본 머티리얼(흰색)로 원상복구
        if (normalMaterial != null)
        {
            rend.material = normalMaterial;
        }
    }

    // 3. 타일을 마우스로 클릭했을 때 (IPointerClick)
    public void OnPointerClick(PointerEventData eventData)
    {
        // 이미 터렛이 지어진 곳이라면 중복 설치 방지
        if (isTurretBuilt)
        {
            Debug.Log("여기에는 이미 터렛이 설치되어 있습니다!");
            return;
        }

        Debug.Log("마우스 클릭 - 여기에 터렛 설치");
        BuildManager.instance.BuildTurretOn(transform);

        // ★ [핵심 수정] 터렛을 지었으므로 상태를 true로 변경
        isTurretBuilt = true;

        // ★ [핵심 추가] 클릭해서 타워가 지어지는 순간, 마우스가 위에 있더라도 
        // 하이라이트(파란색)를 강제로 끄고 원래 기본(흰색) 머티리얼로 즉시 되돌립니다!
        if (normalMaterial != null)
        {
            rend.material = normalMaterial;
        }
    }
}