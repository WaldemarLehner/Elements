using ComputergrafikSpiel.Model.EntitySettings.Interfaces;

namespace ComputergrafikSpiel.Model.Triggers.Interfaces
{
    public interface ITrigger : IEntity
    {
        void TriggerCollisionFunction();
    }
}
