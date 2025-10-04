using UnityEngine;
namespace EntitySystem
{
    public interface IMoveEntity
    {
        void Move(Vector2 movement);
    }
}