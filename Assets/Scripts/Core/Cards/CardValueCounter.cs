using DG.Tweening;
using TMPro;
using UnityEngine;

namespace TestApp.Cards
{
    public class CardValueCounter : MonoBehaviour
    {
        private const float FADE_DURATION = 2f;
        private const float COUNTER_DURATION = .5f;
        [SerializeField] TextMeshProUGUI text;

        private int Value;

        public void SetValue(int value, bool animate = true)
        {
            if (value < 0)
                value = 0;

            if (animate && Value != value)
                AnimateProcess(value);
            else
                SetText(value);

            Value = value;
        }

        private void SetText(int value)
        {
            text.text = Mathf.Clamp(value, 0, int.MaxValue).ToString();
        }

        private void AnimateProcess(int newValue)
        {
            var wasValue = Value;
            var diff = newValue - wasValue;

            var dropText = Instantiate(text, transform);
            dropText.text = $"{(diff > 0 ? "+" : null)}{diff}";
            dropText.transform.SetAsLastSibling();

            DOTween.Sequence()
                .SetLink(gameObject)
                .Join(dropText.DOFade(0, FADE_DURATION))
                .Join(dropText.transform.DOLocalMoveY(50, FADE_DURATION))
                .OnComplete(() => Destroy(dropText));

            DOTween.To(() => wasValue, SetText, newValue, COUNTER_DURATION)
                .SetLink(gameObject)
                .OnComplete(() => SetText(newValue));
        }
    }
}