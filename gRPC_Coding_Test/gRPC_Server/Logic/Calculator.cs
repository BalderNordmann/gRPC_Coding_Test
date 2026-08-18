namespace gRPC_Server.Logic;

/// <summary>
/// Performs basic arithmetic operations on two valid, finite numbers.
/// </summary>
public sealed class Calculator
{
    /// <summary>
    /// Validates two finite operands and applies the requested arithmetic operation.
    /// </summary>
    public double Calculate(double _leftOperand, double _rightOperand, ArithmeticOperation _operation)
    {
        ValidateOperand(_leftOperand, nameof(_leftOperand));
        ValidateOperand(_rightOperand, nameof(_rightOperand));

        return _operation switch
        {
            ArithmeticOperation.Addition => _leftOperand + _rightOperand,
            ArithmeticOperation.Subtraction => _leftOperand - _rightOperand,
            ArithmeticOperation.Multiplication => _leftOperand * _rightOperand,
            ArithmeticOperation.Division when _rightOperand == 0 => throw new CalculationValidationException("Division durch null ist nicht erlaubt."),
            ArithmeticOperation.Division => _leftOperand / _rightOperand,
            _ => throw new CalculationValidationException("Die Rechenoperation wird nicht unterstuetzt.")
        };
    }

    /// <summary>
    /// Rejects operands that are not finite numeric values.
    /// </summary>
    private static void ValidateOperand(double _operand, string _parameterName)
    {
        if (double.IsNaN(_operand) || double.IsInfinity(_operand))
        {
            throw new CalculationValidationException($"{_parameterName} muss eine endliche Zahl sein.");
        }
    }
}
