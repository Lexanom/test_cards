using TestApp.Cards;
using TestApp.Data;
using UnityEngine;

namespace TestApp.Core.CardFactories
{
    public class CardsFactoryTest : AbstractCardsFactory
    {
        public CardsFactoryTest(PlayField playField, GameData data) : base(playField, data)
        {
        }

        public override CardViewBase CreateCardView(CardData cardData)
        {
            var prefab = Game.Instance.BasePrefabs.CardView;

            var view = Object.Instantiate(prefab, playField.CardsContainer);
            var art = data.GetCardArt(cardData.Id);

            view.Init(playField, cardData);
            view.SetArt(art);
            return view;
        }
    }
}