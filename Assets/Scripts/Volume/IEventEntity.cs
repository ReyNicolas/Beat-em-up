using System;

public interface IEventEntity
{
     event Action onTakeDamage;
     event Action onDead;
     event Action onAttack;
}
