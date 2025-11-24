using ItemsSystem.Data;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerSystem
{
    public abstract class APlayerHandItem : MonoBehaviour
    {
        [field: SerializeField] public ItemDataSO ItemData { get; protected set; }

        [SerializeField] protected GameObject reloadObject;
        [SerializeField] protected Image reloadFiller;

        protected bool _exposed = true;

        protected virtual void OnDestroy() => Expose();

        public virtual void Activate()
        {
            gameObject.SetActive(true);
            reloadObject.SetActive(true);
            
        }

        public virtual void Deactivate()
        {
            gameObject.SetActive(false);
            reloadObject.SetActive(false);
        }

        protected virtual void Bind()
        {
            if(!_exposed)
                return;
            
            _exposed = false;
        }

        protected virtual void Expose()
        {
            if (_exposed)
                return;
            
            _exposed = true;
        }
    }
}