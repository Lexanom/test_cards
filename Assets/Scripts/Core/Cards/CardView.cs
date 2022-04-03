using System;
using TestApp.Core;
using TestApp.Data;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TestApp.Cards
{
    public class CardView : CardViewBase
    {
        [Header("Art")]
        [SerializeField] Image icon;
        [SerializeField] TextMeshProUGUI title;
        [SerializeField] TextMeshProUGUI desc;
        [Header("Effects")]
        [SerializeField] Image glow;
        [SerializeField] Image shine;
        [SerializeField] float fadeDuration = .5f;

        private Tween tween;
        private bool isGlowed;
        private bool isShined;

        public override void Init(PlayField playField, CardData data)
        {
            base.Init(playField, data);

            shine.gameObject.SetActive(false);
            glow.gameObject.SetActive(false);

            title.text = Data.Name;
            desc.text = Data.Desc;
        }

        public void SetArt(Sprite art)
        {
            icon.sprite = art;
        }

        public override void SetHovered(bool value)
        {
            base.SetHovered(value);

            if (isGlowed == value)
                return;
            isGlowed = value;

            glow.gameObject.SetActive(value);
            tween?.Kill();

            if (value)
                tween = glow.DOFade(1, fadeDuration)
                    .ChangeStartValue(glow.color.Set(a: 0))
                    .SetLink(gameObject);
        }

        //public override void SetSelected(bool value)
        //{
        //    //if (isShined == value)
        //    //    return;
        //    //isShined = value;

        //    //shine.gameObject.SetActive(value);
        //    //tween?.Kill();

        //    //if (value)
        //    //    tween = shine.DOFade(1, fadeDuration)
        //    //        .ChangeStartValue(shine.color.Set(a: 0))
        //    //        .SetLoops(-1, LoopType.Yoyo)
        //    //        .SetLink(gameObject);
        //}
    }
}