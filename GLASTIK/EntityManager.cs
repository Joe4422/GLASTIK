using System.Collections.Generic;

namespace GLASTIK
{
    public class EntityManager
    {
        public Level Level { get; set; }

        public List<BaseEntity> Entities { get; } = new();

        public EntityManager(Level level)
        {
            Level = level;
        }

        public void Register(BaseEntity entity)
        {
            Entities.Add(entity);
            entity.Level = Level;
        }

        public void TickEntities()
        {
            foreach (var entity in Entities)
            {
                entity.Tick();
            }
        }
    }
}
