using System;
using TestApp.Data;
using TestApp.Core.CardFactories;
using TestApp.UI;
using UnityEngine;
using TestApp.Core;

namespace TestApp
{
    public class Game : MonoBehaviour
    {
        public static Game Instance { get; private set; }

        [SerializeField] LoadWindow loadWindow;
        [SerializeField] PlayField playField;
        [SerializeField] BasePrefabs basePrefabs;

        public BasePrefabs BasePrefabs => basePrefabs;
        public PlayField PlayField => playField;

        private GameData GameData;
        private AbstractCardsFactory CardsFactory;

        private void Awake()
        {
            if (Instance)
            {
                Destroy(this);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            GameData = new GameDataTest(OnProgress, OnLoadComplete, OnLoadError);
            CardsFactory = new CardsFactoryTest(playField, GameData);

            playField.Init(GameData, CardsFactory);
            loadWindow.Show();

            void OnProgress(float value)
            {
                loadWindow.OnProgress(value);
            }

            void OnLoadComplete()
            {
                loadWindow.Hide();
                playField.ShowCards();
            }

            void OnLoadError(Exception e)
            {
                loadWindow.OnError(e);
            }
        }
    }
}