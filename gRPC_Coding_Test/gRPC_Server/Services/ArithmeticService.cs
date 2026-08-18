using Grpc.Core;
using GrpcCodingTest.Contracts;
using gRPC_Server.Logic;

namespace gRPC_Server.Services;

/// <summary>
/// Implements the calculator gRPC endpoint and translates domain errors to gRPC statuses.
/// </summary>
public sealed class ArithmeticService : CalculatorGrpc.CalculatorGrpcBase
{
    private readonly Calculator calculatorLogic;
    private readonly ILogger<ArithmeticService> serviceLogger;
        
    public ArithmeticService(Calculator _calculator, ILogger<ArithmeticService> _logger)
    {
        calculatorLogic = _calculator;
        serviceLogger = _logger;
    }

    /// <summary>
    /// Calculates the requested result or returns a suitable gRPC error for invalid or unexpected failures.
    /// </summary>
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

    /// <summary>
    /// Converts the protobuf operation value to the internal calculation operation.
    /// </summary>
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
