
namespace _Scripts
{
    public interface IDamageable
    {
        void TakeDamage(int damage, ElementType elementType);
        void Death();
    }
}