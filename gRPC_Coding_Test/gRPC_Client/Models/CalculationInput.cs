using GrpcCodingTest.Contracts;

namespace gRPC_Client.Models;

public sealed class CalculationInput
{
    public CalculationInput(double _leftOperand, double _rightOperand, CalculationOperation _operation)
    {
        LeftOperand = _leftOperand;
        RightOperand = _rightOperand;
        Operation = _operation;
    }

    public double LeftOperand { get; }

    public double RightOperand { get; }

    public CalculationOperation Operation { get; }
}