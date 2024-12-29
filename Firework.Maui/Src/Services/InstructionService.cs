using System.Text;
using System.Text.RegularExpressions;
using Firework.Maui.Exceptions;
using Firework.Mobile.Abstraction.Instructions;
using Firework.Mobile.Models.Dto;
using Firework.Mobile.Models.Models.Instructions;

namespace Firework.Maui.Services;

public class InstructionService : IInstructionService
{
    public InstructionInfo CreateInstruction(string instruction)
    {
        try
        {
            var instructionDto = ParseInstruction(instruction);

            var parameters = GetParameters(instruction);
            
            var instructionResult = new InstructionInfo
            {
                ServiceName = instructionDto.Service,
                Parameters = parameters,
                ActionInfo = GetActionInfo(instructionDto.Action, parameters)
            };

            return instructionResult;
        }
        catch (ArgumentOutOfRangeException)
        {
            throw new ParseInstructionException("Не удалось спарсить инструкцию");
        }
        catch (ParseInstructionException ex)
        {
            throw new ParseInstructionException(ex.Message);
        }
    }

    public InstructionInfo CreateInstruction(string service, string action)
    {
        try
        {
            return new InstructionInfo
            {
                ServiceName = service,
                ActionInfo = GetActionInfo(action, new List<ActionParameterInfo>()),
                Parameters = new List<ActionParameterInfo>()
            };
        }
        catch (ArgumentOutOfRangeException)
        {
            throw new ParseInstructionException("Не удалось спарсить инструкцию");
        }
        catch (ParseInstructionException ex)
        {
            throw new ParseInstructionException(ex.Message);
        }
    }

    public InstructionInfo CreateInstruction(string service, string action, List<ActionParameterInfo> parameters)
    {
        try
        {
            parameters ??= new List<ActionParameterInfo>();

            return new InstructionInfo
            {
                ServiceName = service,
                ActionInfo = GetActionInfo(action, parameters),
                Parameters = parameters
            };
        }
        catch (ArgumentOutOfRangeException)
        {
            throw new ParseInstructionException("Не удалось спарсить инструкцию");
        }
        catch (ParseInstructionException ex)
        {
            throw new ParseInstructionException(ex.Message);
        }
    }

    private ActionInfo GetActionInfo(string actionName, List<ActionParameterInfo> parameters)
    {
        return new ActionInfo
        {
            Name = actionName,
            Parameters = parameters
        };
    }

    private InstructionDto ParseInstruction(string instruction)
    {
        if (!IsValid(instruction))
            throw new ParseInstructionException("Инструкция не прошла валидацию");
        
        var result = new InstructionDto
        {
            Service = GetServiceName(instruction),
            Action = GetActionName(instruction),
            Parameters = GetParameters(instruction)
        };

        return result;
    }

    private string GetActionName(string instruction)
    {
        var action = instruction.Split('>')[1];
        
        return action.Contains('(') || action.Contains(')')
            ? action.Substring(0, action.IndexOf('('))
            : action;
    }
    
    private string GetServiceName(string instruction) => instruction.Split('>')[0];

    private bool IsValid(string instruction)
    {

        List<string> regexList = new()
        {
            // Один или несколько параметров но без именования
            // task>run(cmd.exe, vivaldi.exe)
            // task>run(cmd.exe)
            // mouse>click(left, 1)
            @"^\w+>\w+\(\w+(\.\w+)?(, \w+(\.\w+)?)*\)$",
            
            // Без параметров и без скобок
            // task>run
            @"^\w+>\w+$",
            
            // Без параметров но со скобками
            // task>run()
            @"^\w+>\w+\(\)$",
            
            // Именованные параметры
            // task>run(path: cmd.exe, arguments: 1)
            @"^\w+>\w+\(\w+: \w+(\.\w+)?, \w+: \w+(\.\w+)?\)$",
            
            // Именованные параметры с неограниченным количеством параметров
            // task>run(path: cmd.exe, arguments: 1, test: 1)
            @"^\w+>\w+\((\w+: \w+(\.\w+)?|\w+: \d+)(, \w+: \w+(\.\w+)?|\w+: \d+)*\)$"
        };
        
        List<bool> validationResults = new();
        
        foreach (string regex in regexList) 
            validationResults.Add(Regex.IsMatch(instruction, regex));
        
        return validationResults.Any(x =>  x);
    }

    private List<ActionParameterInfo> GetParameters(string instruction)
    {
        if (!instruction.Contains('('))
            return new();
        
        List<ActionParameterInfo> result = new();
        instruction = instruction.Remove(0, instruction.IndexOf('(') + 1);
        instruction = instruction[..instruction.LastIndexOf(')')];

        var parameters = instruction.Split(',', StringSplitOptions.TrimEntries).ToList();
        foreach (var parameter in parameters)
        {
            var item = parameter.Split(':');
            result.Add(new ActionParameterInfo(item.Last() == item.First() ? null : item.First().Trim(), item.Last().Trim()));
        }

        return result;
    }
    
    public string ToString(InstructionInfo instruction)
    {
        var resultInstructionString = new StringBuilder();

        resultInstructionString.Append($"{instruction.ServiceName}>");
        resultInstructionString.Append($"{instruction.ActionInfo.Name}");

        resultInstructionString.Append("(");
        if (instruction.Parameters.Count > 0)
            resultInstructionString.Append(string.Join(", ", instruction.Parameters.Select(x =>
                string.IsNullOrEmpty(x.Name)
                    ? $"{x.Value}"
                    : $"{x.Name}: {x.Value}")));


        resultInstructionString.Append(")");

        return resultInstructionString.ToString();
    }
}