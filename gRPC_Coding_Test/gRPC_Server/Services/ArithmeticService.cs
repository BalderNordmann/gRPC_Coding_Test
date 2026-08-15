using Grpc.Core;
using GrpcCodingTest.Contracts;
using gRPC_Server.Logic;

namespace gRPC_Server.Services;

public sealed class ArithmeticService : CalculatorGrpc.CalculatorGrpcBase
{
    private readonly Calculator calculatorLogic;
    private readonly ILogger<ArithmeticService> serviceLogger;

    public ArithmeticService(Calculator _calculator, ILogger<ArithmeticService> _logger)
    {
        calculatorLogic = _calculator;
        serviceLogger = _logger;
    }

    public override Task<CalculationResponse> Calculate(CalculationRequest _request, ServerCallContext _context)
    {
        try
        {
            var operation = MapOperation(_request.Operation);
            var result = calculatorLogic.Calculate(_request.LeftOperand, _request.RightOperand, operation);
            return Task.FromResult(new CalculationResponse { Result = result });
        }
        catch (CalculationValidationException _exception)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, _exception.Message));
        }
        catch (Exception _exception)
        {
            serviceLogger.LogError(_exception, "Die Berechnung konnte nicht ausgeführt werden.");
            throw new RpcException(new Status(StatusCode.Internal, "Beim Server ist ein unerwarteter Fehler aufgetreten."));
        }
    }

    private static ArithmeticOperation MapOperation(CalculationOperation _operation)
    {
        return _operation switch
        {
            CalculationOperation.Addition => ArithmeticOperation.Addition,
            CalculationOperation.Subtraction => ArithmeticOperation.Subtraction,
            CalculationOperation.Multiplication => ArithmeticOperation.Multiplication,
            CalculationOperation.Division => ArithmeticOperation.Division,
            _ => throw new CalculationValidationException("Die Rechenoperation wird nicht unterstuetzt.")
        };
    }
}
