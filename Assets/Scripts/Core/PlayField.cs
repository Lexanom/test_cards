using System;
using System.Collections.Generic;
using System.Linq;
using TestApp.Cards;
using TestApp.Data;
using TestApp.Core.CardFactories;
using TestApp.Core.CardHolders;
using DG.Tweening;
using UnityEngine;

namespace TestApp.Core
{
    public class PlayField : MonoBehaviour
    {
        [SerializeField] Transform container;
        [SerializeField] CardsHolderArc cardsHolder;
        [SerializeField] Transform cardDispenser;
        [SerializeField] Transform selectTarget;

        public Transform CardsContainer => container;

        private GameData GameData;
        private AbstractCardsFactory CardsFactory;
        private CardViewBase SelectedCard;

        private List<CardViewBase> PlayerCards;

        private int GetCardIndex(CardViewBase cardView) => PlayerCards.IndexOf(cardView);

        public void Init(GameData gameData, AbstractCardsFactory cardsFactory)
        {
            GameData = gameData;
            CardsFactory = cardsFactory;

            PlayerCards = new List<CardViewBase>();
        }

        public void ShowCards()
        {
            var cards = GameData.GetCards();

            cardsHolder.Init(cards.Length);

            var seq = DOTween.Sequence();
            foreach (var cardData in cards)
            {
                var cardView = CardsFactory.CreateCardView(cardData);
                PlayerCards.Add(cardView);

                seq.Append(CardAppear(cardView));
            }
        }

        public void ChangeNextCardValue(int change)
        {
            var curId = SelectedCard ? GetCardIndex(SelectedCard) : -1;
            var nextId = curId + 1;
            if (nextId >= PlayerCards.Count)
                nextId = 0;

            var card = PlayerCards.ElementAtOrDefault(nextId) 
                ?? PlayerCards.FirstOrDefault();

            if (!card)
                return;

            var randomType = GetRandomTypeExceptEmpty();

            OnCardClick(card)?
                .AppendInterval(.5f)
                .OnComplete(ChangeValueTest);

            void ChangeValueTest()
            {
                ChangeValue(SelectedCard, randomType, change);
            }

            CardValueType GetRandomTypeExceptEmpty()
            {
                var types = Enum.GetValues(typeof(CardValueType))
                    .Cast<CardValueType>()
                    .ToArray();

                while (true)
                {
                    var result = types[UnityEngine.Random.Range(0, types.Length)];
                    if (card.Data.GetValue(result) > 0)
                        return result;
                }
            }
        }

        public void OnCardOver(CardViewBase cardView, bool hover)
        {
            if (hover && SelectedCard == cardView)
                return;

            cardView.SetHovered(hover);
        }

        public Sequence OnCardClick(CardViewBase cardView)
        {
            if (SelectedCard == cardView)
                return DOTween.Sequence();

            if (SelectedCard)
            {
                MoveCardToStack(SelectedCard);
                SelectedCard.transform.SetSiblingIndex(SelectedCard.Id);
            }

            SelectedCard = cardView;
            SelectedCard.SetHovered(false);
            SelectedCard.transform.SetAsLastSibling();

            return MoveOutOfStack(SelectedCard);
        }

        private void ChangeValue(CardViewBase cardView, CardValueType valueType, int change)
        {
            cardView.Data.ChangeValue(valueType, change);
            if (cardView.Data.HP <= 0)
                RemoveCard(cardView);
        }

        private void RemoveCard(CardViewBase cardView)
        {
            if (SelectedCard == cardView)
                SelectedCard = null;

            PlayerCards.Remove(cardView);
            DisappearCard(cardView)
                .OnComplete(() =>
                {
                    cardView.Destroy();
                    UpdateCardsInStack();
                });
        }

        private void UpdateCardsInStack()
        {
            cardsHolder.Init(PlayerCards.Count);
            PlayerCards.ForEach(x => MoveCardToStack(x)); 
        }

        private Sequence CardAppear(CardViewBase cardView)
        {
            const float appear = .4f;
            const float delay = .3f;

            return DOTween.Sequence()
                .SetLink(cardView.gameObject)
                .Join(cardView.transform.DOLocalMove(Vector3.zero, appear).ChangeStartValue(cardDispenser.localPosition))
                .Join(cardView.transform.DOScale(Vector3.one, appear).ChangeStartValue(Vector3.zero))
                .AppendInterval(delay)
                .Append(MoveCardToStack(cardView));
        }

        private Sequence MoveOutOfStack(CardViewBase cardView)
        {
            const float duration = .3f;

            var endPosition = selectTarget.transform.position.y;
            var endScale = Vector3.one;

            return DOTween.Sequence()
                .SetLink(cardView.gameObject)
                .Join(cardView.transform.DOMoveY(endPosition, duration))
                .Join(cardView.transform.DORotate(Vector3.zero, duration))
                .Join(cardView.transform.DOScale(endScale, duration));
        }

        private Sequence MoveCardToStack(CardViewBase cardView)
        {
            const float duration = .3f;

            var endPosition = cardsHolder.GetCardPosition(GetCardIndex(cardView));
            var endScale = Vector3.one * cardsHolder.Scale;

            return DOTween.Sequence()
                .SetLink(cardView.gameObject)
                .Join(cardView.transform.DOMove(endPosition.Position, duration))
                .Join(cardView.transform.DORotate(endPosition.Rotation, duration))
                .Join(cardView.transform.DOScale(endScale, duration));
        }

        private Sequence DisappearCard(CardViewBase cardView)
        {
            const float delay = .5f;
            const float duration = .4f;

            var endPosition = Vector2.zero;
            var endRotation = new Vector3(0, 0, 180);
            var endScale = Vector2.zero;

            return DOTween.Sequence()
                .SetLink(cardView.gameObject)
                .AppendInterval(delay)
                .Append(cardView.transform.DOLocalMove(endPosition, duration))
                .Join(cardView.transform.DORotate(endRotation, duration))
                .Join(cardView.transform.DOScale(endScale, duration));
        }
    }
}