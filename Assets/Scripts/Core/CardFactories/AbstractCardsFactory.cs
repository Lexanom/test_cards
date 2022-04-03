using TestApp.Cards;
using TestApp.Data;

namespace TestApp.Core.CardFactories
{
    public abstract class AbstractCardsFactory
    {
        protected PlayField playField;
        protected GameData data;

        public AbstractCardsFactory(PlayField playField, GameData data)
        {
            this.playField = playField;
            this.data = data;
        }

        public abstract CardViewBase CreateCardView(CardData cardData);
    }
}