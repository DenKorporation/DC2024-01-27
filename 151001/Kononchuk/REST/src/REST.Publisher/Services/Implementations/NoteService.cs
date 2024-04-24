using System.Diagnostics;
using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Microsoft.Extensions.Options;
using REST.Publisher.Models.DTOs.Request;
using REST.Publisher.Models.DTOs.Response;
using REST.Publisher.Services.Interfaces;
using REST.Publisher.Utilities.Serializers;
using KafkaException = REST.Publisher.Utilities.Exceptions.KafkaException;

namespace REST.Publisher.Services.Implementations;

public class NoteService(
    IOptions<ProducerConfig> producerConfig,
    IOptions<ConsumerConfig> consumerConfig,
    IConfiguration configuration,
    ILogger<NoteService> logger) : INoteService
{
    private readonly string _producerTopic = configuration["Kafka:Producer:Topic"] ??
                                             throw new InvalidOperationException(
                                                 "configuration[\"Kafka:Producer:Topic\"] doesn't exist");

    private readonly string _consumerTopic = configuration["Kafka:Consumer:Topic"] ??
                                             throw new InvalidOperationException(
                                                 "configuration[\"Kafka:Consumer:Topic\"] doesn't exist");

    //Key is GUID
    private readonly IProducer<string, KafkaRequestDto> _producer =
        new ProducerBuilder<string, KafkaRequestDto>(producerConfig.Value)
            .SetValueSerializer(new JsonSerializer<KafkaRequestDto>()).Build();

    private IConsumer<string, KafkaResponseDto> GetNewConsumer()
    {
        return new ConsumerBuilder<string, KafkaResponseDto>(consumerConfig.Value)
            .SetValueDeserializer(new JsonDeserializer<KafkaResponseDto>().AsSyncOverAsync()).Build();
    }

    public async Task<NoteResponseDto> CreateAsync(NoteRequestDto dto)
    {
        var guid = Guid.NewGuid();
        await _producer.ProduceAsync(_producerTopic,
            new Message<string, KafkaRequestDto>
                { Key = guid.ToString(), Value = new KafkaRequestDto { Method = HttpMethod.Post, Request = dto } });

        CancellationToken cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(10)).Token;

        var response = Consume(guid, cancellationToken);

        if (response.Error is not null)
        {
            throw new KafkaException(response.Error.ErrorCode / 100, response.Error);
        }

        return response.Response!;
    }

    public async Task<NoteResponseDto> GetByIdAsync(long id)
    {
        var guid = Guid.NewGuid();
        await _producer.ProduceAsync(_producerTopic,
            new Message<string, KafkaRequestDto>
                { Key = guid.ToString(), Value = new KafkaRequestDto { Method = HttpMethod.Get, Id = id} });

        CancellationToken cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(10)).Token;

        var response = Consume(guid, cancellationToken);

        if (response.Error is not null)
        {
            throw new KafkaException(response.Error.ErrorCode / 100, response.Error);
        }

        return response.Response!;
    }

    public async Task<IEnumerable<NoteResponseDto>> GetAllAsync()
    {
        var guid = Guid.NewGuid();
        await _producer.ProduceAsync(_producerTopic,
            new Message<string, KafkaRequestDto>
                { Key = guid.ToString(), Value = new KafkaRequestDto { Method = HttpMethod.Get} });

        CancellationToken cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(10)).Token;

        var response = Consume(guid, cancellationToken);

        if (response.Error is not null)
        {
            throw new KafkaException(response.Error.ErrorCode / 100, response.Error);
        }

        return response.ResponseList!;
    }

    public async Task<NoteResponseDto> UpdateAsync(long id, NoteRequestDto dto)
    {
        var guid = Guid.NewGuid();
        await _producer.ProduceAsync(_producerTopic,
            new Message<string, KafkaRequestDto>
                { Key = guid.ToString(), Value = new KafkaRequestDto { Method = HttpMethod.Put, Id = id, Request = dto} });

        CancellationToken cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(10)).Token;

        var response = Consume(guid, cancellationToken);

        if (response.Error is not null)
        {
            throw new KafkaException(response.Error.ErrorCode / 100, response.Error);
        }

        return response.Response!;
    }

    public async Task DeleteAsync(long id)
    {
        var guid = Guid.NewGuid();
        await _producer.ProduceAsync(_producerTopic,
            new Message<string, KafkaRequestDto>
                { Key = guid.ToString(), Value = new KafkaRequestDto { Method = HttpMethod.Delete, Id = id} });

        CancellationToken cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(10)).Token;

        var response = Consume(guid, cancellationToken);

        if (response.Error is not null)
        {
            throw new KafkaException(response.Error.ErrorCode / 100, response.Error);
        }
    }

    private KafkaResponseDto Consume(Guid guid, CancellationToken cancellationToken)
    {
        IConsumer<string, KafkaResponseDto>? consumer = null;
        var stopwatch = new Stopwatch();
        try
        {
            consumer = GetNewConsumer();
            consumer.Subscribe(_consumerTopic);
            Message<string, KafkaResponseDto> message;
            
            do
            {
                stopwatch.Restart();
                message = consumer.Consume(cancellationToken).Message;
                stopwatch.Stop();
                logger.LogInformation($"Duration: {stopwatch.ElapsedMilliseconds} ms");
            } while (message.Key != guid.ToString());
            
            return message.Value;
        }
        finally
        {
            consumer?.Close();
        }
    }
}