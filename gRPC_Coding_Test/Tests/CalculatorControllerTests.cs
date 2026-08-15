using Grpc.Core;
using GrpcCodingTest.Contracts;
using gRPC_Client.Controllers;
using gRPC_Client.Models;
using gRPC_Client.Services;
using Xunit;

namespace Tests;

public sealed class CalculatorControllerTests
{
    [Theory]
    [InlineData("", "2")]
    [InlineData("1", "invalid")]
    public async Task CalculateAsync_InvalidNumber_ReturnsValidationMessage(string _leftOperand, string _rightOperand)
    {
        var controller = new CalculatorController(new unavailableCalculatorGrpcClient());

        var result = await controller.CalculateAsync(_leftOperand, _rightOperand, CalculationOperation.Addition);

        Assert.Equal("Bitte geben Sie zwei gueltige Zahlen ein.", result.Message);
    }

    [Fact]
    public async Task CalculateAsync_MissingOperation_ReturnsValidationMessage()
    {
        var controller = new CalculatorController(new unavailableCalculatorGrpcClient());

        var result = await controller.CalculateAsync("1", "2", null);

        Assert.Equal("Bitte waehlen Sie eine Rechenoperation aus.", result.Message);
    }

    [Fact]
    public async Task CalculateAsync_ServerUnavailable_ReturnsConnectionMessage()
    {
        var controller = new CalculatorController(new unavailableCalculatorGrpcClient());

        var result = await controller.CalculateAsync("1", "2", CalculationOperation.Addition);

        Assert.Equal("Der Server ist nicht erreichbar. Starten Sie den Server und versuchen Sie es erneut.", result.Message);
    }

    private sealed class unavailableCalculatorGrpcClient : ICalculatorGrpcClient
    {
        public Task<double> CalculateAsync(CalculationInput _input)
        {
            return Task.FromException<double>(new RpcException(new Status(StatusCode.Unavailable, "Server nicht erreichbar.")));
        }
    }
}
