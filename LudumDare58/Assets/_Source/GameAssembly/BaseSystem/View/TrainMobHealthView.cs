using DG.Tweening;
using HealthSystem;
using UnityEngine;

namespace BaseSystem.View
{
    public class TrainMobHealthView : AHealthView
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private float animDuration;
        [SerializeField] private float shakeStrength;
        [SerializeField] private int shakeVibrato = 15;
        [SerializeField] private Color hitColor;

        private Color _startColor;
        private Tween _currentColorTween;

        protected override void Start()
        {
            base.Start();
            _startColor = spriteRenderer.color;
        }

        protected override void Draw(int oldValue, int newValue)
        {
            if(newValue >= oldValue)
                return;
            
            transform.DOShakePosition(animDuration, shakeStrength, shakeVibrato);

            _currentColorTween?.Kill();
            _currentColorTween = spriteRenderer.DOColor(hitColor, animDuration / 2);
            _currentColorTween.onComplete +=
                () => _currentColorTween = spriteRenderer.DOColor(_startColor, animDuration / 2);
        }
    }
}