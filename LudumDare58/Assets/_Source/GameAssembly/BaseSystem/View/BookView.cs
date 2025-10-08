using System;
using System.Collections.Generic;
using System.Linq;
using LevelsSystem;
using PlayerSystem;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace BaseSystem.View
{
    public class BookView : MonoBehaviour
    {
        [SerializeField] private GameObject bookCanvas;
        [SerializeField] private List<GameObject> pages = new();
        [SerializeField] private Button rightArrow;
        [SerializeField] private Button leftArrow;
        [SerializeField] private Button closeBookButton;
        [SerializeField] private Lectern lectern;
        [SerializeField] private List<BookLevelData> levelsData;
        
        [Inject] private InputSystem_Actions _input;

        private InterLevelData _interLevelData;
        private int _currentPageIndex;

        private void Start()
        {
            _interLevelData = FindFirstObjectByType<InterLevelData>();
            Bind();
            CheckArrowsActive();
            LoadCompletedLevels();
        }

        private void OnDestroy() => Expose();

        private void LoadCompletedLevels()
        {
            foreach (var t in levelsData.Where(t => _interLevelData.CompletedLevels.Contains(t.LevelIndex)))
            {
                foreach (var o in t.ObjectsToActivate)
                    o.SetActive(true);
                foreach (var o in t.ObjectsToDeactivate)
                    o.SetActive(false);

                if (t.IconImage)
                    t.IconImage.sprite = t.ChangeIcon;
            }
        }

        private void OpenBook()
        {
            _input.Player.Disable();
            bookCanvas.gameObject.SetActive(true);
        }

        private void CloseBook()
        {
            _input.Player.Enable();
            bookCanvas.gameObject.SetActive(false);
        }

        private void OpenCurrentPage(int old)
        {
            if (_currentPageIndex < 0 || _currentPageIndex >= pages.Count)
                return;

            pages[old].SetActive(false);
            pages[_currentPageIndex].SetActive(true);
        }

        private void NextPage()
        {
            if (_currentPageIndex + 1 >= pages.Count)
                return;

            var temp = _currentPageIndex;
            _currentPageIndex++;

            CheckArrowsActive();
            OpenCurrentPage(temp);
        }

        private void PreviousPage()
        {
            if (_currentPageIndex - 1 < 0)
                return;

            var temp = _currentPageIndex;
            _currentPageIndex--;

            CheckArrowsActive();
            OpenCurrentPage(temp);
        }

        private void CheckArrowsActive()
        {
            rightArrow.gameObject.SetActive(_currentPageIndex < pages.Count - 1);
            leftArrow.gameObject.SetActive(_currentPageIndex > 0);
        }

        private void Bind()
        {
            rightArrow.onClick.AddListener(NextPage);
            leftArrow.onClick.AddListener(PreviousPage);
            closeBookButton.onClick.AddListener(CloseBook);
            lectern.OnInteracted += OpenBook;
        }

        private void Expose()
        {
            rightArrow.onClick.RemoveAllListeners();
            leftArrow.onClick.RemoveAllListeners();
            closeBookButton.onClick.RemoveAllListeners();
            lectern.OnInteracted -= OpenBook;
        }

        [Serializable]
        public class BookLevelData
        {
            [field: SerializeField] public int LevelIndex { get; private set; }
            [field: SerializeField] public Image IconImage { get; private set; }
            [field: SerializeField] public Sprite ChangeIcon { get; private set; }
            [field: SerializeField] public GameObject[] ObjectsToActivate { get; private set; }
            [field: SerializeField] public GameObject[] ObjectsToDeactivate { get; private set; }
        }
    }
}