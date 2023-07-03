using System.Collections.Generic;
using Features.TimeSystems.Interfaces.Handlers;

namespace Features.TimeSystems.Interfaces
{
    public interface ITimeCollector
    {
        List<IUpdateHandler> UpdateHandlers { get; }
        List<IFixedUpdateHandler> FixedUpdateHandlers { get; }
        List<ILateUpdateHandler> LateUpdateHandlers { get; }
        
        string Category { get; }
    }
}