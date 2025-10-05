using EntitySystem.Data;

namespace EntitySystem.Entities.Interfaces
{
    public interface IEntity
    {
        void Configure(EntityConfigSO config);
    }
}