using UnityEngine;

namespace PlayerSystem
{
    public abstract class APlayerHandItem : MonoBehaviour
    {
        protected bool _exposed = true;
        
        protected virtual void Start(){}

        protected virtual void OnDestroy() => Expose();

        public virtual void Activate() => gameObject.SetActive(true);

        public virtual void Deactivate() => gameObject.SetActive(false);

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