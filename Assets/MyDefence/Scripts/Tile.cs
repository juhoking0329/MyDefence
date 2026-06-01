using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("타일 색상 설정")]
    [SerializeField] private Color hoverColor = Color.green;

    private Renderer rend;
    private Color startColor;
    private bool isTurretBuilt = false;

    void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isTurretBuilt) return;
        rend.material.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rend.material.color = startColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isTurretBuilt)
        {
            Debug.Log("여기에는 이미 터렛이 설치되어 있습니다!");
            return;
        }

        Debug.Log("마우스 클릭 - 여기에 터렛 설치");
        BuildManager.instance.BuildTurretOn(transform);
        isTurretBuilt = true;
        rend.material.color = startColor;
    }
}