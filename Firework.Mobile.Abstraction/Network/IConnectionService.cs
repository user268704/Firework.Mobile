using Firework.Mobile.Models.Models.Instructions;
using Firework.Mobile.Models.Models.Results;

namespace Firework.Mobile.Abstraction.Network;

public interface IConnectionService : IDisposable
{
    public bool IsConnected();
    public Task<List<InstructionResult>> SendCommandAsync(List<InstructionInfo> commands);
    public List<InstructionResult> SendCommand(List<InstructionInfo> commands);
    public bool TryStartConnection();
    public void RestartConnection();
    public void ClearConnection();
}