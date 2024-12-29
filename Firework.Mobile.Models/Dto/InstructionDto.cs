using Firework.Mobile.Models.Models.Instructions;

namespace Firework.Mobile.Models.Dto;

public class InstructionDto
{
    public string Service { get; set; }
    public string Action { get; set; }
    public List<ActionParameterInfo> Parameters { get; set; }
}