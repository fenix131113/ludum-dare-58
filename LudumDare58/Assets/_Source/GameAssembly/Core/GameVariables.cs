using System;
using System.Collections.Generic;

namespace Core
{
    public class GameVariables
    {
        public bool CanMove { get; private set; }
        public bool CanRotate { get; private set; }

        private readonly Dictionary<GameVariablesBlockerEnum, List<GameVariableBlocker>> _activeBlockers = new();

        public void RegisterBlocker(GameVariableBlocker blocker)
        {
            if (!_activeBlockers.ContainsKey(blocker.Value))
                _activeBlockers.Add(blocker.Value, new List<GameVariableBlocker>());

            if (_activeBlockers[blocker.Value].Contains(blocker))
                return;

            _activeBlockers[blocker.Value].Add(blocker);
            CheckBlockCondition(blocker.Value);
            blocker.OnReleased += OnBlockerReleased;
        }

        public void ForceClearBlockers(GameVariablesBlockerEnum blockerType)
        {
            if (!_activeBlockers.TryGetValue(blockerType, out var blocker))
                return;

            blocker.Clear();
            CheckBlockCondition(blockerType);
        }

        private void OnBlockerReleased(GameVariableBlocker blocker)
        {
            _activeBlockers[blocker.Value].Remove(blocker);
            CheckBlockCondition(blocker.Value);
        }

        private void CheckBlockCondition(GameVariablesBlockerEnum blockerType)
        {
            var flag = _activeBlockers[blockerType].Count == 0;

            switch (blockerType)
            {
                case GameVariablesBlockerEnum.PLAYER_MOVE:
                    CanMove = flag;
                    break;
                case GameVariablesBlockerEnum.PLAYER_ROTATE:
                    CanRotate = flag;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(blockerType), blockerType,
                        $"Can't check block condition. Flag: {blockerType.ToString()}");
            }
        }

        /// <summary>
        /// Need to save into var to dispose in instance's script. Otherwise, blocker will stick in list and need to do force clear
        /// </summary>
        public class GameVariableBlocker : IDisposable
        {
            public GameVariablesBlockerEnum Value { get; private set; }

            public event Action<GameVariableBlocker> OnReleased;

            public GameVariableBlocker(GameVariablesBlockerEnum value) => Value = value;

            public void Dispose()
            {
                OnReleased?.Invoke(this);
                OnReleased = null;
            }
        }
    }
}