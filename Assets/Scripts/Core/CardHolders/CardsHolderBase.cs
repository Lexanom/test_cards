using System.Linq;
using TestApp.Cards;
using UnityEngine;

namespace TestApp.Core.CardHolders
{
    public abstract class CardsHolderBase : MonoBehaviour
    {
        [SerializeField] float cardScale = .7f;
        public float Scale => cardScale;

        protected CardPosition[] Positions;

        public void Init(int count)
        {
            Positions = new CardPosition[count];
            CalculateCardPositions();
        }

        public CardPosition GetCardPosition(int index) =>
            Positions.ElementAtOrDefault(index);

        protected abstract void CalculateCardPositions();
    }
}