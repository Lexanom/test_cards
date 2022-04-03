using System;
using System.Linq;
using TestApp.Cards;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TestApp.UI
{
    public class HUD : MonoBehaviour
    {
        [SerializeField] Button interact;
        [SerializeField] TMP_InputField from;
        [SerializeField] TMP_InputField to;

        private void Start()
        {
            interact.onClick.RemoveAllListeners();
            interact.onClick.AddListener(OnInteractClick);
        }

        private void OnInteractClick()
        {
            if (int.TryParse(from.text, out var startValue) && int.TryParse(to.text, out var endValue))
            {
                var change = GetRandomExceptZero(startValue, endValue);
                Game.Instance.PlayField.ChangeNextCardValue(change);
            }

            int GetRandomExceptZero(int start, int end)
            {
                if (start == 0 && end == 0)
                    return 0;

                while (true)
                {
                    var result = UnityEngine.Random.Range(start, end);
                    if (result != 0)
                        return result;
                }
            }
        }
    }
}