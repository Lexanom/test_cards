using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TestApp.Data
{
    public class GameDataTest : GameData
    {
        public const string IMAGE_URL = "https://picsum.photos/170/150";
        public const int CARDS_MIN_COUNT = 4;
        public const int CARDS_MAX_COUNT = 6;

        public GameDataTest(Action<float> onProgress, Action onSuccess, Action<Exception> onError) : base(onProgress, onSuccess, onError)
        {
            LoadCardData();
            LoadCardArts(onProgress, onSuccess, onError);
        }

        public override CardData[] GetCards()
        {
            var count = Random.Range(CARDS_MIN_COUNT, CARDS_MAX_COUNT + 1);
            return cards
                .Take(count)
                .ToArray();
        }

        private void LoadCardData()
        {
            cards = new CardData[CARDS_MAX_COUNT];

            for (int i = 0; i < cards.Length; i++)
                cards[i] = new CardData(i)
                {
                    Name = $"Card title #{i}",
                    Desc = $"Card desc #{i}",
                    Attack = Random.Range(1, 6),
                    Mana = Random.Range(1, 6),
                    HP = Random.Range(1, 6)
                };
        }

        private void LoadCardArts(Action<float> onProgress, Action onSuccess, Action<Exception> onError)
        {
            Game.Instance.StartCoroutine(LoadImages(onProgress, onSuccess, onError));
        }

        private IEnumerator LoadImages(Action<float> onProgress, Action onSuccess, Action<Exception> onError)
        {
            var loader = new DataLoader();
            var loadHandlers = new List<LoadHandler<Sprite>>();

            for (int i = 0; i < CARDS_MAX_COUNT; i++)
            {
                var handler = loader.LoadSpriteAsync(IMAGE_URL);
                loadHandlers.Add(handler);
            }

            while (loadHandlers.Any(x => !x.IsCompleted))
            {
                var progress = loadHandlers
                    .Select(x => x.Progress)
                    .Average();
                onProgress?.Invoke(progress);
                yield return null;
            }

            cardSprites = loadHandlers
                .Select(x => x.Result)
                .ToArray();

            onSuccess?.Invoke();
        }
    }
}