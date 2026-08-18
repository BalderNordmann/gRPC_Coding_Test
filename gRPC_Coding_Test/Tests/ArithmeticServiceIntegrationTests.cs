using Grpc.Core;
using Grpc.Net.Client;
using GrpcCodingTest.Contracts;
using gRPC_Server.Logic;
using gRPC_Server.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Tests;

/// <summary>
/// Verifies the calculator gRPC endpoint through an in-memory ASP.NET Core test server.
/// </summary>
public sealed class ArithmeticServiceIntegrationTests : IAsyncLifetime
{
    private WebApplication application = null!;
    private GrpcChannel channel = null!;
    private CalculatorGrpc.CalculatorGrpcClient client = null!;

    /// <summary>
    /// Starts the in memory gRPC host and creates a client for each test run.
    /// </summary>
    public async Task InitializeAsync()
    {
        var builder = WebApplication.CreateBuilder();
        builder.WebHost.UseTestServer();
        builder.Services.AddGrpc();
        builder.Services.AddSingleton<Calculator>();

        application = builder.Build();
        application.MapGrpcService<ArithmeticService>();
        await application.StartAsync();

        var handler = application.GetTestServer().CreateHandler();
        channel = GrpcChannel.ForAddress("http://localhost", new GrpcChannelOptions { HttpHandler = handler });
        client = new CalculatorGrpc.CalculatorGrpcClient(channel);
    }

    /// <summary>
    /// Releases the in memory gRPC channel and application host.
    /// </summary>
    public async Task DisposeAsync()
    {
        channel.Dispose();
        await application.DisposeAsync();
    }

    /// <summary>
    /// Verifies that a valid gRPC request returns the calculated result.
    /// </summary>
    [Fact]
    public async Task Calculate_ValidRequest_ReturnsResult()
    {
        var response = await client.CalculateAsync(new CalculationRequest
        {
            LeftOperand = 8,
            RightOperand = 2,
            Operation = CalculationOperation.Division
        }).ResponseAsync;

        Assert.Equal(4, response.Result);
    }

    /// <summary>
    /// Verifies that division by zero is returned as a gRPC InvalidArgument error.
    /// </summary>
    [Fact]
    public async Task Calculate_DivisionByZero_ReturnsInvalidArgument()
    {
        var exception = await Assert.ThrowsAsync<RpcException>(async () =>
            await client.CalculateAsync(new CalculationRequest
            {
                LeftOperand = 1,
                RightOperand = 0,
                Operation = CalculationOperation.Division
            }).ResponseAsync);

        Assert.Equal(StatusCode.InvalidArgument, exception.StatusCode);
    }

    /// <summary>
    /// Verifies that an undefined operation value is returned as a gRPC InvalidArgument error.
    /// </summary>
    [Fact]
    public async Task Calculate_UnknownOperation_ReturnsInvalidArgument()
    {
        var exception = await Assert.ThrowsAsync<RpcException>(async () =>
            await client.CalculateAsync(new CalculationRequest
            {
                LeftOperand = 1,
                RightOperand = 2,
                Operation = (CalculationOperation)999
            }).ResponseAsync);

        Assert.Equal(StatusCode.InvalidArgument, exception.StatusCode);
    }

    /// <summary>
    /// Verifies that the default unspecified operation is returned as a gRPC InvalidArgument error.
    /// </summary>
    [Fact]
    public async Task Calculate_UnspecifiedOperation_ReturnsInvalidArgument()
    {
        var exception = await Assert.ThrowsAsync<RpcException>(async () =>
            await client.CalculateAsync(new CalculationRequest
            {
                LeftOperand = 1,
                RightOperand = 2,
                Operation = CalculationOperation.Unspecified
            }).ResponseAsync);

        Assert.Equal(StatusCode.InvalidArgument, exception.StatusCode);
    }
}
