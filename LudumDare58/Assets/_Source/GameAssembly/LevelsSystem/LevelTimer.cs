using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LevelsSystem
{
    public class LevelTimer : MonoBehaviour //TODO: Separate timer view and logic, maybe add abstraction layer
    {
        [SerializeField] private TMP_Text timerText;
        [SerializeField] private int levelSecondsTime;

        public float TimeLeft { get; private set; }

        private void Awake() => TimeLeft = levelSecondsTime;

        private void Update()
        {
            TimeLeft -= Time.deltaTime;
            timerText.text = $"{(TimeLeft - TimeLeft % 60) / 60:00}:{TimeLeft % 60:00}";

            if (TimeLeft <= 0)
                OnLostTimer();
        }

        private void OnLostTimer() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}