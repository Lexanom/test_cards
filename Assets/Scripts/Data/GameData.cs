using System;
using System.Linq;
using UnityEngine;

namespace TestApp.Data
{
    public class GameData
    {
        protected Sprite[] cardSprites;
        protected CardData[] cards;

        public GameData(Action<float> onProgress, Action onSuccess, Action<Exception> onError)
        {
        }

        public virtual CardData[] GetCards()
        {
            return cards;
        }

        public Sprite GetCardArt(int id)
        {
            return cardSprites.ElementAtOrDefault(id);
        }
    }
}