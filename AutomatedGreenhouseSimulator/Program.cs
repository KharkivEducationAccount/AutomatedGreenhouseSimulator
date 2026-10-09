using AutomatedGreenhouseSimulator;
using AutomatedGreenhouseSimulator.Mqtt;
using Models;
using MQTTnet;
using System.Text.Json;

public class Program
{
    public static async Task Main()
    {
        SensorConfiguration[] sensors = CreateSensors();

        var factory = new MqttClientFactory();
        using var mqttClient = factory.CreateMqttClient();

        var options = new MqttClientOptionsBuilder()
            .WithTcpServer("127.0.0.1", 1883)
            .Build();

        await mqttClient.ConnectAsync(options);
        Console.WriteLine("Connected to MQTT broker.");

        await RunSimulationAsync(mqttClient, sensors);

        await mqttClient.DisconnectAsync();
    }

    private static SensorConfiguration[] CreateSensors()
    {
        return
            [
            new (
                new TemperatureSensor("Outside temperature"),
                "greenhouse/1/sensors/outside/temperature",
                "C"),
            new (
                new HumiditySensor("Outside humidity"),
                "greenhouse/1/sensors/outside/humidity",
                "%"),
            new (
                new TemperatureSensor("Inside temperature"),
                "greenhouse/1/sensors/inside/temperature",
                "C"),
            new (
                new HumiditySensor("Inside humidity"),
                "greenhouse/1/sensors/inside/humidity",
                "%"),
            ];
    }

    private static async Task RunSimulationAsync(
        IMqttClient mqttClient,
        SensorConfiguration[] sensors)
    {
        for (int iteration = 1; ; iteration++)
        {
            Console.WriteLine($"Iteration {iteration}.");


            foreach (var sensor in sensors)
            {
                await PublichSensorReadingAsync(mqttClient, sensor);
            }

            Console.WriteLine();
            await Task.Delay(TimeSpan.FromSeconds(2));
        }
    }

    private static async Task PublichSensorReadingAsync(
        IMqttClient mqttClient,
        SensorConfiguration configuration)
    {
        configuration.Sensor.ReadValue();

        string payload = JsonSerializer.Serialize(new
        {
            value = Math.Round(configuration.Sensor.Value, 2),
            unit = configuration.Unit,
            measuredAtUtc = DateTimeOffset.UtcNow
        });

        var message = new MqttApplicationMessageBuilder()
            .WithTopic(configuration.Topic)
            .WithPayload(payload)
            .Build();

        await mqttClient.PublishAsync(message);

        Console.WriteLine($"Published { configuration.Topic} -> {payload} ");
    }
}