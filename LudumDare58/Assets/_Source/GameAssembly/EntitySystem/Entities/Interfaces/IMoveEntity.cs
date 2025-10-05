using UnityEngine;

namespace EntitySystem.Entities.Interfaces
{
    public interface IMoveEntity : IEntityContains
    {
        void Move(Vector2 movement);
    }
}