using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace PlayerSystem.View
{
    public class HandsView : MonoBehaviour
    {
        [SerializeField] private List<RotationAngleGroup> rotations;
        [SerializeField] private Transform rotatePivot;

        [Inject] private PlayerAim _playerAim;

        private void Update()
        {
            rotatePivot.rotation = Quaternion.Euler(0, 0, _playerAim.RotateAngle);

            var fullCircleDegrees = _playerAim.RotateAngle < 0 ? 360 + _playerAim.RotateAngle : _playerAim.RotateAngle;

            rotatePivot.localScale =
                IsInRange(fullCircleDegrees, 270, 90) ? new Vector3(1, 1, 1) : new Vector3(1, -1, 1);

            var correct = rotations.Find(rot => IsInRange(fullCircleDegrees, rot.MinEdge, rot.MaxEdge));

            if (correct != null)
            {
                //temRenderer.sprite = correct.ItemRotation;
                return;
            }

#if UNITY_EDITOR
            Debug.LogWarning($"Can't rotate player with {_playerAim.RotateAngle} degrees!");
#endif
        }

        private static bool IsInRange(float angle, float min, float max)
        {
            if (min <= max)
                return angle >= min && angle <= max;

            return angle >= min || angle <= max;
        }

        [Serializable]
        private class RotationAngleGroup
        {
            [field: SerializeField] public float MaxEdge { get; private set; }
            [field: SerializeField] public float MinEdge { get; private set; }
            [field: SerializeField] public Sprite ItemRotation { get; private set; } //TODO: Move to items system
        }
    }
}