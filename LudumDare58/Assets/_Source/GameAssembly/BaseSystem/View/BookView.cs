using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LevelsSystem;
using PlayerSystem;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using VContainer;
using Random = UnityEngine.Random;

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
        [SerializeField] private GameObject dropCoinPrefab;
        [SerializeField] private GameObject exitZone;
        [SerializeField] private float dropCoinsInterval;
        [SerializeField] private List<BookLevelData> levelsData;

        [Inject] private InputSystem_Actions _input;
        [Inject] private PlayerResources _playerResources;

        private InterLevelData _interLevelData;
        private Button _startButton;
        private int _currentPageIndex;

        private void Start()
        {
            ObjectInjector.InjectObject(this);
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

        private void OnStartGameButtonClicked() => exitZone.SetActive(true);

        private void OpenBook()
        {
            var temp = _interLevelData.CompletedLevels.Count > 0 ? _interLevelData.CompletedLevels[^1] : 0;

            if (!_startButton && temp <= _interLevelData.CompletedLevels.Count)
            {
                _startButton = levelsData.First(x => x.LevelIndex == temp + 1).StartButton;
                _startButton.gameObject.SetActive(true);
                _startButton.onClick.AddListener(OnStartGameButtonClicked);
            }

            _input.Player.Disable();
            bookCanvas.gameObject.SetActive(true);

            if (_interLevelData.MoneyToGet > 0)
            {
                StartCoroutine(DropCoinsCoroutine(_interLevelData.MoneyToGet));
                _interLevelData.ClearGetMoney();
            }
        }

        private void CloseBook()
        {
            _input.Player.Enable();
            bookCanvas.gameObject.SetActive(false);
        }

        private void DropCoin()
        {
            _playerResources.TryChangeCoins(1);
            var screenXOffset = Screen.width / 5; // TODO: Move to settings
            var screenYOffset = Screen.height / 5; // Move to settings
            var spawnPos = new Vector3(Random.Range(screenXOffset, Screen.width - screenXOffset), Random.Range(screenYOffset, Screen.height - screenYOffset), 10);
            Instantiate(dropCoinPrefab,
                Camera.main!.ScreenToWorldPoint(spawnPos),
                Quaternion.identity);
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
            _startButton?.onClick.RemoveAllListeners();
        }

        private IEnumerator DropCoinsCoroutine(int count)
        {
            var temp = count;
            yield return new WaitForSeconds(dropCoinsInterval);

            DropCoin();
            temp--;

            if (temp > 0)
                StartCoroutine(DropCoinsCoroutine(temp));
        }

        [Serializable]
        public class BookLevelData
        {
            [field: SerializeField] public int LevelIndex { get; private set; }
            [field: SerializeField] public Image IconImage { get; private set; }
            [field: SerializeField] public Sprite ChangeIcon { get; private set; }
            [field: SerializeField] public Button StartButton { get; private set; }
            [field: SerializeField] public GameObject[] ObjectsToActivate { get; private set; }
            [field: SerializeField] public GameObject[] ObjectsToDeactivate { get; private set; }
        }
    }
}