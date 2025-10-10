using System;
using System.Collections.Generic;
using System.Linq;
using BaseSystem;
using InteractionSystem;
using LevelsSystem;
using PlayerSystem;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace TrophySystem.View
{
    public class TrophyShelfView : MonoBehaviour
    {
        [SerializeField] private List<TrophyGroup> trophies;
        [SerializeField] private Button closeButton;
        [SerializeField] private GameObject canvas;
        [SerializeField] private BaseInteractiveObject interactiveObject;

        [Inject] private InputSystem_Actions _input;

        private InterLevelData _interLevelData;

        private void Start()
        {
            _interLevelData = FindFirstObjectByType<InterLevelData>();

            if (_interLevelData)
                SetActiveTrophies(_interLevelData.CompletedLevels.ToList());

            Bind();
        }

        private void OnDestroy() => Expose();

        private void OpenShelf()
        {
            _input.Player.Disable();
            canvas.SetActive(true);
        }

        private void CloseShelf()
        {
            _input.Player.Enable();
            canvas.SetActive(false);
        }

        private void Bind()
        {
            interactiveObject.OnInteracted += OpenShelf;
            closeButton.onClick.AddListener(CloseShelf);
        }

        private void Expose()
        {
            interactiveObject.OnInteracted -= OpenShelf;
            closeButton.onClick.RemoveAllListeners();
        }

        public void SetActiveTrophies(List<int> collected)
        {
            collected.ForEach(level =>
            {
                var find = trophies.FirstOrDefault(x => x.LevelIndex == level);

                if (find == null)
                    return;
                
                foreach (var o in find.TrophiesObjects)
                    o.SetActive(true);
            });
        }

        [Serializable]
        public class TrophyGroup
        {
            [field: SerializeField] public int LevelIndex { get; private set; }
            [field: SerializeField] public GameObject[] TrophiesObjects { get; private set; }
        }
    }
}