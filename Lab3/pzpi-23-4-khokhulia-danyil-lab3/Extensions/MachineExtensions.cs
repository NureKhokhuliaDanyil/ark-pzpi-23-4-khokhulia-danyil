using Washing.Entities;

namespace Washing.Extensions;

public static class MachineExtensions
{
    public static bool IsAvailable(this WashingMachine machine)
    {
        return machine.Status == MachineStatus.Idle;
    }
}
