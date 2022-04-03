using System;
using TestApp.Core;
using TestApp.Data;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TestApp.Cards
{
    public class CardViewBase : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Values")]
        [SerializeField] CardValueCounter damage;
        [SerializeField] CardValueCounter live;
        [SerializeField] CardValueCounter mana;

        public CardData Data { get; private set; }
        protected PlayField PlayField;

        public int Id => Data.Id;

        public virtual void Init(PlayField playField, CardData data)
        {
            PlayField = playField;
            Data = data;

            name = $"Card view #{Id}";

            UpdateValues(false);
            Data.Updated += DataUpdated;
        }

        private void DataUpdated()
        {
            UpdateValues();
        }

        private void UpdateValues(bool needAnim = true)
        {
            damage.SetValue(Data.Attack, needAnim);
            live.SetValue(Data.HP, needAnim);
            mana.SetValue(Data.Mana, needAnim);
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            PlayField.OnCardClick(this);
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            PlayField.OnCardOver(this, true);
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            PlayField.OnCardOver(this, false);
        }

        public virtual void SetHovered(bool value)
        {
        }

        public void Destroy()
        {
            Data.Updated -= DataUpdated;
            Destroy(gameObject);
        }
    }
}