# .NET MSMQ (Microsoft Message Queuing) Sample

A practical implementation of the **Producer-Consumer** pattern in C# using Windows built-in **MSMQ (Microsoft Message Queuing)**. This repository demonstrates how to send and receive persistent, asynchronous messages between two decoupled .NET Console applications using Private Queues.

## 📌 Project Structure

* **`MsmqSender` (Producer):** Connects to a local private queue (creates it if it doesn't exist), serializes a `PaymentMessage` object, and sends it to the queue with disk persistence (`Recoverable = true`).
* **`MsmqReceiver` (Consumer):** Listens to the private queue continuously, receives incoming messages in real-time, and deserializes the XML payload back into a strongly-typed C# object.

## ⚙️ Prerequisites

1. **Windows OS** (MSMQ is a Windows-specific feature).
2. **.NET SDK** (.NET 6.0 / 8.0 or later).
3. **MSMQ Server Feature Enabled:**
   Before running the applications, ensure that Microsoft Message Queue (MSMQ) is enabled on your Windows machine. 

   Run the following command in **PowerShell (as Administrator)**:
   ```powershell
   Enable-WindowsOptionalFeature -Online -FeatureName MSMQ-Server -All
   ```
   *Alternatively, press `Win + R`, type `OptionalFeatures.exe`, and check **Microsoft Message Queue (MSMQ) Server Core**.*

## 🚀 Getting Started

### 1. Clone the repository
```bash
git clone [https://github.com/YOUR-USERNAME/MsmqDemo.git](https://github.com/YOUR-USERNAME/MsmqDemo.git)
cd MsmqDemo
```

### 2. Run the Sender (Producer)
Open a terminal and start the sender application:
```bash
dotnet run --project MsmqSender
```
Type any description in the console and press `Enter` to dispatch a payment message to `.\private$\MyCompanyQueue`.

### 3. Inspect the Queue in Windows (Optional)
Before running the receiver, you can view the queued messages stored in Windows:
1. Press `Win + R` and run `compmgmt.msc` (Computer Management).
2. Navigate to **Services and Applications** -> **Message Queuing** -> **Private Queues** -> **mycompanyqueue** -> **Queue Messages**.

### 4. Run the Receiver (Consumer)
Open a second terminal window and start the receiver application:
```bash
dotnet run --project MsmqReceiver
```
The receiver will immediately process all pending messages in the queue and continue listening for new ones.

## 🛠️ Built With

* **C# / .NET**
* **[Experimental.System.Messaging](https://www.nuget.org/packages/Experimental.System.Messaging)** - .NET Core / .NET 5+ compatible wrapper for Windows MSMQ.

## 💡 Key Concepts Covered

* Creating and checking the existence of **Private Queues** (`.\private$`).
* Ensuring message durability across server restarts using `Recoverable = true`.
* Serializing and deserializing custom C# POCO models using `XmlMessageFormatter`.
* Synchronous blocking queue consumption via `queue.Receive()`.
