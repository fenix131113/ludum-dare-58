using EntitySystem.Data;

namespace EntitySystem
{
    public interface IEntity
    {
        void Configure(EntityConfigSO config);
    }
}