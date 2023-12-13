using System;

namespace CodeBase.Logic.StaminaEntity
{
    public interface IStamina : IPoints
    {
        event Action AttemptToEmptyUsed;
    }
}