using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Confluent.Kafka;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using OrderApi.WebApi;
using Testcontainers.Kafka;
using Testcontainers.PostgreSql;

namespace OrderApi.IntegrationTests.EndToEndTests;

public class CreateOrderKafkaTests : IAsyncLifetime
{
    private readonly KafkaContainer _kafka = new KafkaBuilder()
        .WithImage("confluentinc/cp-kafka:7.4.3")
        .Build();

    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder()
        .WithImage("postgres:15")
        .WithDatabase("orderapidb")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    private HttpClient? _client;

    public async Task InitializeAsync()
    {
        await _postgres.StartAsync();
        await _kafka.StartAsync();

        var factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((_, cfg) =>
                {
                    var dict = new Dictionary<string, string?>
                    {
                        ["ConnectionStrings:OrderDbContext"] = _postgres.GetConnectionString(),
                        ["Kafka:BootstrapServers"] = _kafka.GetBootstrapAddress()
                    };

                    cfg.AddInMemoryCollection(dict);
                });
            });

        _client = factory.CreateClient();
    }

    public async Task DisposeAsync()
    {
        await _postgres.DisposeAsync();
        await _kafka.DisposeAsync();
    }

    [Fact]
    public async Task CreateOrder_ShouldPublishKafkaEvent()
    {
        // Arrange
        var request = new
        {
            customerId = Guid.NewGuid(),
            items = new[]
            {
                new { productId = Guid.NewGuid(), quantity = 2, unitPrice = 100m }
            }
        };

        // Act
        var response = await _client!.PostAsJsonAsync("/api/Orders", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var content = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonSerializer.Deserialize<ApiResponse>(content);
        apiResponse.Should().NotBeNull();
        apiResponse.Id.Should().NotBeEmpty();

        var config = new ConsumerConfig
        {
            BootstrapServers = _kafka.GetBootstrapAddress(),
            GroupId = "test-consumer-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using var consumer = new ConsumerBuilder<string, string>(config).Build();
        consumer.Subscribe("order-created-event");

        var result = consumer.Consume(TimeSpan.FromSeconds(10));
        result.Should().NotBeNull();
        var consumeResult = JsonSerializer.Deserialize<ConsumeResult>(result.Message.Value);
        consumeResult.Should().NotBeNull();
        consumeResult.OrderId.Should().Be(apiResponse.Id);
        consumeResult.TotalAmount.Should().Be(request.items.Sum(x => x.quantity * x.unitPrice));

        consumer.Close();
    }

    public class ApiResponse
    {
        [JsonPropertyName("id")]
        public string? Id { get; init; }
    }

    public class ConsumeResult
    {
        public string? OrderId { get; init; }
        public decimal TotalAmount { get; init; }
    }
}