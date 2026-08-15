using System.Globalization;
using Grpc.Core;
using GrpcCodingTest.Contracts;
using gRPC_Client.Models;
using gRPC_Client.Services;

namespace gRPC_Client.Controllers;

public sealed class CalculatorController
{
    private readonly GrpcCalculatorClient calculatorClient;

    public CalculatorController(GrpcCalculatorClient _calculatorClient)
    {
        calculatorClient = _calculatorClient;
    }

    public async Task<CalculationResult> CalculateAsync(string _leftOperandText, string _rightOperandText, CalculationOperation? _operation)
    {
        if (!TryParseNumber(_leftOperandText, out var leftOperand) || !TryParseNumber(_rightOperandText, out var rightOperand))
        {
            return new CalculationResult("Bitte geben Sie zwei gueltige Zahlen ein.");
        }

        if (_operation is null)
        {
            return new CalculationResult("Bitte waehlen Sie eine Rechenoperation aus.");
        }

        try
        {
            var input = new CalculationInput(leftOperand, rightOperand, _operation.Value);
            var result = await calculatorClient.CalculateAsync(input);
            return new CalculationResult(result.ToString(CultureInfo.CurrentCulture));
        }
        catch (RpcException _exception) when (_exception.StatusCode == StatusCode.Unavailable)
        {
            return new CalculationResult("Der Server ist nicht erreichbar. Starten Sie den Server und versuchen Sie es erneut.");
        }
        catch (RpcException _exception)
        {
            return new CalculationResult($"Serverfehler: {_exception.Status.Detail}");
        }
        catch (Exception)
        {
            return new CalculationResult("Der Server ist nicht erreichbar. Starten Sie den Server und versuchen Sie es erneut.");
        }
    }

    private static bool TryParseNumber(string _input, out double _number)
    {
        var wasParsed = double.TryParse(_input, NumberStyles.Float, CultureInfo.CurrentCulture, out _number)
                     || double.TryParse(_input, NumberStyles.Float, CultureInfo.InvariantCulture, out _number);

        return wasParsed && !double.IsNaN(_number) && !double.IsInfinity(_number);
    }
}
