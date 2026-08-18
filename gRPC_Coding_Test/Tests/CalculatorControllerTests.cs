using Grpc.Core;
using GrpcCodingTest.Contracts;
using gRPC_Client.Controllers;
using gRPC_Client.Models;
using gRPC_Client.Services;
using Xunit;

namespace Tests;

/// <summary>
/// Verifies client-side validation and service-failure handling in the calculator controller.
/// </summary>
public sealed class CalculatorControllerTests
{
    /// <summary>
    /// Verifies that invalid operand text returns a validation message without contacting the service.
    /// </summary>
    [Theory]
    [InlineData("", "2")]
    [InlineData("1", "invalid")]
    public async Task CalculateAsync_InvalidNumber_ReturnsValidationMessage(string _leftOperand, string _rightOperand)
    {
        var controller = new CalculatorController(new unavailableCalculatorGrpcClient());

        var result = await controller.CalculateAsync(_leftOperand, _rightOperand, CalculationOperation.Addition);

        Assert.Equal("Bitte geben Sie zwei gueltige Zahlen ein.", result.Message);
    }

    /// <summary>
    /// Verifies that a missing operation returns a validation message.
    /// </summary>
    [Fact]
    public async Task CalculateAsync_MissingOperation_ReturnsValidationMessage()
    {
        var controller = new CalculatorController(new unavailableCalculatorGrpcClient());

        var result = await controller.CalculateAsync("1", "2", null);

        Assert.Equal("Bitte waehlen Sie eine Rechenoperation aus.", result.Message);
    }

    /// <summary>
    /// Verifies that an unavailable service returns a connection message.
    /// </summary>
    [Fact]
    public async Task CalculateAsync_ServerUnavailable_ReturnsConnectionMessage()
    {
        var controller = new CalculatorController(new unavailableCalculatorGrpcClient());

        var result = await controller.CalculateAsync("1", "2", CalculationOperation.Addition);

        Assert.Equal("Der Server ist nicht erreichbar. Starten Sie den Server und versuchen Sie es erneut.", result.Message);
    }

    /// <summary>
    /// Test double that always simulates an unavailable calculator service.
    /// </summary>
    private sealed class unavailableCalculatorGrpcClient : ICalculatorGrpcClient
    {
        /// <summary>
        /// Returns a failed task containing a gRPC Unavailable error.
        /// </summary>
        public Task<double> CalculateAsync(CalculationInput _input)
        {
            return Task.FromException<double>(new RpcException(new Status(StatusCode.Unavailable, "Server nicht erreichbar.")));
        }
    }
}
