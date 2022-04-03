using System.Linq;
using TestApp.Cards;
using UnityEngine;

namespace TestApp.Core.CardHolders
{
    public class CardsHolderArc : CardsHolderBase
    {
        [SerializeField] AnimationCurve curve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(.5f, 1), new Keyframe(1, 0));
        [SerializeField] float startAngle = 10;
        [SerializeField] float endAngle = -10;

        protected override void CalculateCardPositions()
        {
            var rect = (RectTransform)transform;

            var bounds = rect.sizeDelta;

            var curveLenght = curve.keys.LastOrDefault().time;
            for (int i = 0; i < Positions.Length; i++)
            {
                var normalizedXPos = Positions.Length > 1
                    ? (float)1 / (Positions.Length - 1) * i
                    : 0;
                var time = curveLenght * normalizedXPos;

                var evaluteY = curve.Evaluate(time);
                var rot = Mathf.Lerp(startAngle, endAngle, normalizedXPos);

                Positions[i] = new CardPosition
                {
                    Position = transform.TransformPoint(new Vector2(bounds.x * normalizedXPos, bounds.y * evaluteY)),
                    Rotation = new Vector3(0, 0, rot)
                };
            }
        }
    }
}