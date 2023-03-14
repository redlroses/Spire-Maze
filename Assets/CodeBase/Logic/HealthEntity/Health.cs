using UnityEngine;

namespace CodeBase.Logic.HealthEntity
{
    public class Health : MonoBehaviour, IDamagable, IHealable
    {
        public void Damage(int points)
        {
            throw new System.NotImplementedException();
        }

        public void Heal(int points)
        {
            throw new System.NotImplementedException();
        }
    }
}