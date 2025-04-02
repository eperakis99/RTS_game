using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IDamageable
{

    void takeDamage();

    void OnHealthChanged(int healthLeft);
}
