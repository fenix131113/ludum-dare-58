using UnityEngine;

namespace PlayerSystem
{
    public abstract class APlayerHandItem : MonoBehaviour
    {
        public virtual void Activate() => gameObject.SetActive(true);

        public virtual void Deactivate() => gameObject.SetActive(false);
    }
}