using gRPC_Server.Logic;
using Xunit;

namespace Tests;

public sealed class CalculatorTests
{
    private readonly Calculator calculator = new();

    [Fact]
    public void Calculate_Addition_ReturnsCorrectResult()
    {
        var result = calculator.Calculate(2, 3, ArithmeticOperation.Addition);

        Assert.Equal(5, result);
    }

    [Fact]
    public void Calculate_Subtraction_ReturnsCorrectResult()
    {
        var result = calculator.Calculate(7, 3, ArithmeticOperation.Subtraction);

        Assert.Equal(4, result);
    }

    [Fact]
    public void Calculate_Multiplication_ReturnsCorrectResult()
    {
        var result = calculator.Calculate(4, 2.5, ArithmeticOperation.Multiplication);

        Assert.Equal(10, result);
    }

    [Fact]
    public void Calculate_Division_ReturnsCorrectResult()
    {
        var result = calculator.Calculate(7.5, 2.5, ArithmeticOperation.Division);

        Assert.Equal(3, result);
    }

    [Fact]
    public void Calculate_DivisionByZero_ThrowsValidationException()
    {
        var exception = Assert.Throws<CalculationValidationException>(() => calculator.Calculate(4, 0, ArithmeticOperation.Division));

        Assert.Equal("Division durch null ist nicht erlaubt.", exception.Message);
    }

    [Fact]
    public void Calculate_NegativeNumbers_ReturnsCorrectResult()
    {
        var result = calculator.Calculate(-5, 2, ArithmeticOperation.Addition);

        Assert.Equal(-3, result);
    }

    [Fact]
    public void Calculate_DecimalNumbers_ReturnsCorrectResult()
    {
        var result = calculator.Calculate(1.25, 2.5, ArithmeticOperation.Addition);

        Assert.Equal(3.75, result);
    }
}
