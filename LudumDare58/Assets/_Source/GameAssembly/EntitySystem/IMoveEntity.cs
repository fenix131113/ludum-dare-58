using UnityEngine;
namespace EntitySystem
{
    public interface IMoveEntity : IEntityContains
    {
        void Move(Vector2 movement);
    }
}