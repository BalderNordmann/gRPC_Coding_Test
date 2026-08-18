using gRPC_Server.Logic;
using Xunit;

namespace Tests;

/// <summary>
/// Verifies the server-side calculator's arithmetic and input validation behavior.
/// </summary>
public sealed class CalculatorTests
{
    private readonly Calculator calculator = new();

    /// <summary>
    /// Verifies that addition returns the expected result.
    /// </summary>
    [Fact]
    public void Calculate_Addition_ReturnsCorrectResult()
    {
        var result = calculator.Calculate(2, 3, ArithmeticOperation.Addition);

        Assert.Equal(5, result);
    }

    /// <summary>
    /// Verifies that subtraction returns the expected result.
    /// </summary>
    [Fact]
    public void Calculate_Subtraction_ReturnsCorrectResult()
    {
        var result = calculator.Calculate(7, 3, ArithmeticOperation.Subtraction);

        Assert.Equal(4, result);
    }

    /// <summary>
    /// Verifies that multiplication returns the expected result.
    /// </summary>
    [Fact]
    public void Calculate_Multiplication_ReturnsCorrectResult()
    {
        var result = calculator.Calculate(4, 2.5, ArithmeticOperation.Multiplication);

        Assert.Equal(10, result);
    }

    /// <summary>
    /// Verifies that division returns the expected result.
    /// </summary>
    [Fact]
    public void Calculate_Division_ReturnsCorrectResult()
    {
        var result = calculator.Calculate(7.5, 2.5, ArithmeticOperation.Division);

        Assert.Equal(3, result);
    }

    /// <summary>
    /// Verifies that division by zero is rejected as invalid input.
    /// </summary>
    [Fact]
    public void Calculate_DivisionByZero_ThrowsValidationException()
    {
        var exception = Assert.Throws<CalculationValidationException>(() => calculator.Calculate(4, 0, ArithmeticOperation.Division));

        Assert.Equal("Division durch null ist nicht erlaubt.", exception.Message);
    }

    /// <summary>
    /// Verifies that negative operands are calculated correctly.
    /// </summary>
    [Fact]
    public void Calculate_NegativeNumbers_ReturnsCorrectResult()
    {
        var result = calculator.Calculate(-5, 2, ArithmeticOperation.Addition);

        Assert.Equal(-3, result);
    }

    /// <summary>
    /// Verifies that decimal operands are calculated correctly.
    /// </summary>
    [Fact]
    public void Calculate_DecimalNumbers_ReturnsCorrectResult()
    {
        var result = calculator.Calculate(1.25, 2.5, ArithmeticOperation.Addition);

        Assert.Equal(3.75, result);
    }

    /// <summary>
    /// Verifies floating-point addition within an appropriate precision tolerance.
    /// </summary>
    [Fact]
    public void Calculate_RepeatingDecimal_ReturnsResultWithinTolerance()
    {
        var result = calculator.Calculate(0.1, 0.2, ArithmeticOperation.Addition);

        Assert.InRange(result, 0.299999999999, 0.300000000001);
    }

    /// <summary>
    /// Verifies that NaN is rejected as an invalid operand.
    /// </summary>
    [Fact]
    public void Calculate_NotANumber_ThrowsValidationException()
    {
        Assert.Throws<CalculationValidationException>(() => calculator.Calculate(double.NaN, 1, ArithmeticOperation.Addition));
    }

    /// <summary>
    /// Verifies that infinity is rejected as an invalid operand.
    /// </summary>
    [Fact]
    public void Calculate_Infinity_ThrowsValidationException()
    {
        Assert.Throws<CalculationValidationException>(() => calculator.Calculate(double.PositiveInfinity, 1, ArithmeticOperation.Addition));
    }
}
