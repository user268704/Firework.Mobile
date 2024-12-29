using Firework.Mobile.Models.Models.Instructions;

namespace Firework.Mobile.Abstraction.Instructions;

public interface IInstructionService
{
    public InstructionInfo CreateInstruction(string instruction);   
    public InstructionInfo CreateInstruction(string service, string action);   
    public InstructionInfo CreateInstruction(string service, string action, List<ActionParameterInfo> parameters);   
    public string ToString(InstructionInfo instruction);
}