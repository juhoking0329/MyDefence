using UnityEngine;
using UnityEngine.EventSystems;

namespace MyDefence
{
    public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private AnimationCurve scaleCurve; // 인스펙터에서 커브 직접 조절
        [SerializeField] private float animSpeed = 1f;

        private Vector3 originalScale;
        private bool isHovering = false;
        private float timer = 0f;

        private void Start()
        {
            originalScale = transform.localScale;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            isHovering = true;
            timer = 0f;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isHovering = false;
            transform.localScale = originalScale;
        }

        private void Update()
        {
            if (!isHovering) return;

            timer += Time.deltaTime * animSpeed;

            // 커브 끝에 도달하면 다시 처음부터 반복
            if (timer > scaleCurve.keys[scaleCurve.length - 1].time)
            {
                timer = 0f;
            }

            float scale = scaleCurve.Evaluate(timer);
            transform.localScale = originalScale * scale;
        }
    }
}