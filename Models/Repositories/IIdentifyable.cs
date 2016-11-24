using System;

namespace AKK.Models.Repositories
{
    public interface IIdentifyable
    {
        Guid Id { get; }
    }
}
