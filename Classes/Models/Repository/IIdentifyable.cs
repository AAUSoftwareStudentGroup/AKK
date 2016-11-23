using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKK.Classes.Models.Repository
{
    public interface IIdentifyable
    {
        Guid Id { get; set; }
    }
}
