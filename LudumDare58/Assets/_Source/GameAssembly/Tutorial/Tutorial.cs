using LevelsSystem;
using PlayerSystem;
using UnityEngine;
using VContainer;

namespace Tutorial
{
    public class Tutorial : MonoBehaviour //TODO: Disable tutorial show every level load
    {
        [SerializeField] private GameObject tutorialObject;

        [Inject] private InputSystem_Actions _input;

        private void Start()
        {
            if (FindFirstObjectByType<InterLevelData>().CompletedLevels.Count == 0)
                ActivateTutorial();
        }

        private void ActivateTutorial()
        {
            _input.Player.Disable();
            tutorialObject.gameObject.SetActive(true);
        }

        public void DeactivateTutorial()
        {
            _input.Player.Enable();
            tutorialObject.gameObject.SetActive(false);
        }
    }
}