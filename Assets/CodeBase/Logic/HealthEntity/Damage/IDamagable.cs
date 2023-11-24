namespace CodeBase.Logic.HealthEntity.Damage
{
    public interface IDamagable
    {
        void Damage(int damagePoints, DamageType damageType);
    }
}