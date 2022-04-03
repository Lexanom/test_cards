using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TestApp.UI
{
    public class LoadWindow : MonoBehaviour
    {
        [SerializeField] Slider slider;
        [SerializeField] TextMeshProUGUI txtProgress;
        [SerializeField] TextMeshProUGUI txtMessage;

        private void Start()
        {
            slider.maxValue = 1;
            SetValue(0);
            txtMessage.gameObject.SetActive(false);

            transform.SetAsLastSibling();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void OnProgress(float progress)
        {
            SetValue(progress);
        }

        public void OnError(Exception ex)
        {
            txtMessage.text = ex.Message;
            txtMessage.gameObject.SetActive(true);
        }

        private void SetValue(float progress)
        {
            slider.value = progress;
            txtProgress.text = $"{progress * 100:0} %";
        }
    }
}