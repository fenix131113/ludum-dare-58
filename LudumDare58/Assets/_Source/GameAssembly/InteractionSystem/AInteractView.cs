namespace InteractionSystem
{
    public abstract class AInteractView
    {
        public virtual void OnInteract(){}
        public virtual void OnInteractEnabled(){}
        public virtual void OnInteractDisabled(){}
    }
}