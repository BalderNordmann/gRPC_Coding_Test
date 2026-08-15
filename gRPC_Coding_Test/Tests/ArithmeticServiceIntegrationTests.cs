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

public sealed class ArithmeticServiceIntegrationTests : IAsyncLifetime
{
    private WebApplication application = null!;
    private GrpcChannel channel = null!;
    private CalculatorGrpc.CalculatorGrpcClient client = null!;

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

    public async Task DisposeAsync()
    {
        channel.Dispose();
        await application.DisposeAsync();
    }

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
