using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Domain.Entities;
public abstract class Entity
{
    protected Entity()
    { }

    protected Entity(int id)
    {
        Id = id;
    }

    public int Id { get; private set; }
}