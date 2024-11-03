# HeartbeatApp

HeartbeatApp is a .NET 8 application that implements a heartbeat (ticker) system using Quartz.NET and NATS.io. The application publishes heartbeat messages at regular intervals to a NATS.io message broker.

## Overview

HeartbeatApp is designed to function as the heartbeat (ticker) system for a game server infrastructure. It utilizes:
- Quartz.NET for scheduling tasks at precise intervals.
- NATS.io as a lightweight, high-performance message broker for publish-subscribe messaging.

By publishing heartbeat messages, the application enables other services (subscribers) to respond to these events independently, promoting a scalable and modular architecture.

## Architecture
- HeartbeatApp (Publisher):
  - Schedules and publishes heartbeat messages at regular intervals (e.g., every 15 seconds).
  - Connects to the NATS.io server to publish messages.

- NATS.io Message Broker:
  - Receives messages from publishers.
  - Distributes messages to all subscribers interested in specific subjects (topics).

- Subscriber Applications:
  - Subscribe to heartbeat messages from NATS.io.
  - Execute game logic such as NPC actions or random events upon receiving messages.

## Prerequisites

- .NET 8 SDK installed on your development machine.
- NATS.io Server running locally or accessible from the application.

## Getting Started
### Clone the Repository
    git clone https://github.com/yourusername/HeartbeatApp.git
    cd HeartbeatApp

### Setup the NATS.io Server in a Docker Container

You can run the NATS server using Docker:

- Pull the NATS Docker Image
  
      docker pull nats

- Run the NATS Docker Container

      docker run -d --name nats-server -p 4222:4222 nats

  - -d runs the container in detached mode.
  - --name nats-server names the container for easy reference.
  - -p 4222:4222 maps port 4222 inside the container to port 4222 on your host machine.

- Verify the NATS Server is Running

      docker ps

You should see the nats-server container listed and running.

### Configure the Application

- Update appsettings.json

Ensure that the NATS server URL is correctly set in appsettings.json:

    {
      "Nats": {
        "Url": "nats://localhost:4222"
      }
    }

If your NATS server is running on a different host or port, update the URL accordingly.

- Ensure appsettings.json is Included in the Build

Make sure that appsettings.json is copied to the output directory by updating your .csproj file:

    <ItemGroup>
      <None Update="appsettings.json">
        <CopyToOutputDirectory>Always</CopyToOutputDirectory>
      </None>
    </ItemGroup>

## Build and Run HeartbeatApp

- Restore Dependencies:

      dotnet restore

- Build the Application:

      dotnet build

- Run the Application:

      dotnet run

You should see output indicating that heartbeat messages are being published:

    HeartbeatApp is running. Press Ctrl+C to shut down.
    Published heartbeat message: Heartbeat at 2023-11-03T14:00:00.0000000Z

## Usage

HeartbeatApp runs continuously, publishing heartbeat messages at specified intervals. Subscriber applications can listen for these messages and execute appropriate logic in response.

### Implementing a Subscriber

Here's an example of a subscriber application in .NET 8:
      
      using NATS.Client.Core;
      using System;
      using System.Text;
      using System.Threading.Tasks;
      
      namespace SubscriberApp
      {
          class Program
          {
              private static readonly string _natsUrl = "nats://localhost:4222";
              private static readonly string _subject = "game.heartbeat";
      
              static async Task Main(string[] args)
              {
                  var options = NatsOpts.Default with { Url = _natsUrl };
      
                  await using var connection = new NatsConnection(options);
                  await connection.ConnectAsync();
      
                  Console.WriteLine("Subscriber is listening for heartbeat messages...");
      
                  await foreach (var msg in connection.SubscribeAsync(_subject))
                  {
                      var message = Encoding.UTF8.GetString(msg.Data.Span);
                      Console.WriteLine($"Received heartbeat message: {message}");
                      await HandleHeartbeatEventAsync();
                  }
              }
      
              static async Task HandleHeartbeatEventAsync()
              {
                  Console.WriteLine("Executing NPC actions and triggering random events...");
                  await Task.Delay(1000);
              }
          }
      }

Steps to Run the Subscriber:

- Create a New .NET Project:

      dotnet new console -n SubscriberApp
      cd SubscriberApp

- Add NATS Client Package:

      dotnet add package NATS.Client.Core

- Replace the Generated Program.cs with the Code Above.

- Run the Subscriber:

      dotnet run

You should see output similar to:

    Subscriber is listening for heartbeat messages...
    Received heartbeat message: Heartbeat at 2023-11-03T14:00:00.0000000Z
    Executing NPC actions and triggering random events...

## Best Practices

- Configuration Management:
    - Use appsettings.json or environment variables to manage configuration settings.
    - Avoid hardcoding values in the codebase.

- Connection Management:
    - Maintain persistent connections to the NATS.io server to reduce overhead.

- Asynchronous Operations:
    - Use async/await to prevent blocking threads and improve scalability.

- Error Handling:
    - Implement robust exception handling and logging.

- Security:
    - Configure authentication and TLS encryption for NATS.io communications if required.

- Resource Optimization:
    - Ensure efficient use of CPU and memory, especially on resource-constrained environments.

- Monitoring:
    - Utilize monitoring tools to track performance and detect issues promptly.
