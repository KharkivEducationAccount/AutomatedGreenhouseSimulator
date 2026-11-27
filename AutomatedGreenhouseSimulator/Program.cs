using AutomatedGreenhouseSimulator;
using AutomatedGreenhouseSimulator.Mqtt;
using AutomatedGreenhouseSimulator.Systems;
using Models;
using MQTTnet;
using System.Text.Json;

public class Program
{
    public static async Task Main()
    {
        SensorConfiguration[] sensors = CreateSensors();
        ActuatorConfiguration[] actuators = CreateActuators(sensors);

        var factory = new MqttClientFactory();
        using var mqttClient = factory.CreateMqttClient();

        var options = new MqttClientOptionsBuilder()
            .WithTcpServer("127.0.0.1", 1883)
            .Build();

        await mqttClient.ConnectAsync(options);
        Console.WriteLine("Connected to MQTT broker.");

        await RunSimulationAsync(mqttClient, sensors, actuators);

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

    private static ActuatorConfiguration[] CreateActuators(SensorConfiguration[] sensors)
    {
        Sensor insideTemperature = sensors[2].Sensor;
        Sensor insideHumidity = sensors[3].Sensor;

        return
            [
            new (
            "Ventilation",
            new VentilationSystem(),
            insideTemperature,
            "greenhouse/1/actuators/ventilation/state",
            onLevel: 30,
            offLevel: 22,
            onWhenAbove: true),
        new (
            "Irrigation",
            new IrrigationSystem(),
            insideHumidity,
            "greenhouse/1/actuators/irrigation/state",
            onLevel: 30,
            offLevel: 60,
            onWhenAbove: false),
        ];
    }


    private static async Task RunSimulationAsync(
        IMqttClient mqttClient,
        SensorConfiguration[] sensors,
        ActuatorConfiguration[] actuators)
    {
        for (int iteration = 1; ; iteration++)
        {
            Console.WriteLine($"Iteration {iteration}.");


            foreach (var sensor in sensors)
            {
                await PublichSensorReadingAsync(mqttClient, sensor);
            }

            foreach (var actuator in actuators)
            {
                await UpdateActuatorAsync(mqttClient, actuator);
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

    private static async Task UpdateActuatorAsync(
    IMqttClient mqttClient,
    ActuatorConfiguration configuration)
    {
        string? reason = configuration.Evaluate();
        bool isOn = configuration.Actuator.IsOn;

        // Публікуємо тільки зміну стану (і перший стан після запуску)
        if (reason is null && configuration.PublishedState == isOn)
        {
            return;
        }

        string payload = JsonSerializer.Serialize(new
        {
            isOn = isOn,
            reason = reason ?? "initial state",
            changedAtUtc = DateTimeOffset.UtcNow
        });

        // Retain: брокер зберігає останній стан,
        // і веб бачить його одразу після підключення
        var message = new MqttApplicationMessageBuilder()
            .WithTopic(configuration.Topic)
            .WithPayload(payload)
            .WithRetainFlag()
            .Build();

        await mqttClient.PublishAsync(message);
        configuration.PublishedState = isOn;

        Console.WriteLine($"Published {configuration.Topic} -> {payload} ");
    }
}