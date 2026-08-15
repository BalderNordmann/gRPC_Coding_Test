using Grpc.Net.Client;
using GrpcCodingTest.Contracts;
using gRPC_Client.Models;

namespace gRPC_Client.Services;

public sealed class GrpcCalculatorClient
{
    private const string serverAddress = "https://localhost:7042";

    public async Task<double> CalculateAsync(CalculationInput _input)
    {
        using var channel = GrpcChannel.ForAddress(serverAddress);
        var client = new CalculatorGrpc.CalculatorGrpcClient(channel);
        var response = await client.CalculateAsync(new CalculationRequest
        {
            LeftOperand = _input.LeftOperand,
            RightOperand = _input.RightOperand,
            Operation = _input.Operation
        });

        return response.Result;
    }
}