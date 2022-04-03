using System;
using TestApp.Cards;

namespace TestApp.Data
{
    public class CardData
    {
        public readonly int Id;
        public string Name;
        public string Desc;
        public int Attack;
        public int HP;
        public int Mana;

        public event Action Updated;

        public CardData(int id)
        {
            Id = id;
        }

        public void ChangeValue(CardValueType type, int value)
        {
            switch (type)
            {
                case CardValueType.Attack:
                    Attack += value;
                    break;

                case CardValueType.HP:
                    HP += value;
                    break;

                case CardValueType.Mana:
                    Mana += value;
                    break;
            }

            Updated?.Invoke();
        }

        internal int GetValue(CardValueType type) => type switch
        {
            CardValueType.Attack => Attack,
            CardValueType.HP => HP,
            CardValueType.Mana => Mana,
            _ => throw new ArgumentException(type.ToString(), "CardValueType")
        };
    }
}