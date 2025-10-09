using TMPro;
using UnityEngine;
using VContainer;

namespace PlayerSystem
{
    public class CoinsView : MonoBehaviour
    {
        [SerializeField] private TMP_Text coinsLabel;

        [Inject] private PlayerResources _playerResources;

        private void Awake() => Bind();

        private void OnDestroy() => Expose();

        private void Start() => Redraw();

        private void Redraw() => coinsLabel.text = _playerResources.Coins.ToString();

        private void Bind() => _playerResources.OnCoinsChanged += Redraw;

        private void Expose() => _playerResources.OnCoinsChanged -= Redraw;
    }
}