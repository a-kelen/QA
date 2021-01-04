using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface iJwtGenerator
    {
        string CreateToken(User user);
    }
}
