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
