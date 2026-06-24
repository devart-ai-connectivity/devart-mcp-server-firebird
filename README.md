[![Devart MCP Server for Firebird](https://github.com/devart-ai-connectivity/.github/blob/main/assets/cover-banner-mcp-server-for-firebird.webp?raw=true)](https://www.devart.com/mcp/)

# Devart MCP Server for Firebird

Devart MCP Server for Firebird enables AI clients to interact with your data through a secure server running in your environment. It turns a regular AI chat into a practical way to work with real-world business data — and it is faster than conventional export or manual querying.

## Key benefits

Devart MCP Server for Firebird allows you to:

* Work with data intuitively through natural language.
* Retrieve the required data for analysis within minutes.
* Generate reports faster with AI-powered assistance.
* Minimize manual data handling and integration maintenance.

## How it works

Devart MCP Server for Firebird helps AI clients communicate directly with Firebird databases using natural-language prompts. It translates AI requests into structured queries, executes them through Devart connectivity drivers, and returns clean, structured results for seamless AI-powered data access.

![Devart MCP Server architecture](https://github.com/devart-ai-connectivity/.github/blob/main/assets/how_mcp_works_single.webp?raw=true)

## Quick start

To get started with Devart MCP Server for Firebird:

1\. [Download](https://www.devart.com/odbc/firebird/download.html) and [install](https://docs.devart.com/odbc/firebird/installation-dbms.htm) Devart ODBC Driver for Firebird.

2\. [Download](https://www.devart.com/mcp/download.html) and [install](https://docs.devart.com/mcp/installation.html) Devart MCP Server for Firebird.

3\. In Devart MCP Server for Firebird, [configure your data connection and integration settings](https://docs.devart.com//mcp/connection-configuration.html).

![Devart MCP Server configuration](https://github.com/devart-ai-connectivity/.github/blob/main/assets/mcp-servers-gui.webp?raw=true)

4\. Run your first natural-language query.

[![Need an MCP Server for multiple data sources?](https://github.com/devart-ai-connectivity/.github/blob/main/assets/need-mcp-server-universal.webp?raw=true)](https://www.devart.com/mcp/universal/)

## Manual installation and configuration 

**Prerequisites** 

Before building and running Devart MCP Server for Firebird, ensure the following components are installed:

* [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* **ODBC connection** — **Devart.AI.McpServer.Odbc.Firebird.csproj** [Devart ODBC Driver for Firebird](https://www.devart.com/odbc/firebird/download.html) (requires manual download and installation)

**Step 1: Clone the repository**

Clone the project repository and navigate to the project directory:

1\. Open **Command Prompt**.

2\. Enter the following command:

```cmd
git clone https://github.com/devart-ai-connectivity/devart-mcp-server-firebird.git
cd devart-mcp-server-firebird
```

**Step 2: Build the MCP Server from source**

You can build Devart MCP Server for Firebird from source using ODBC.

To build the MCP server with ODBC, select the command based on the bitness of your data source.

* For 64-bit data source, run the following command:

```cmd
dotnet publish Devart.AI.McpServer.Odbc/Devart.AI.McpServer.Odbc.Firebird/Devart.AI.McpServer.Odbc.Firebird.csproj -c ReleaseFirebird -r "win-x64" /p:TargetFramework=net8.0
```

* For 32-bit data source, run the following command:

```cmd
dotnet publish Devart.AI.McpServer.Odbc/Devart.AI.McpServer.Odbc.Firebird/Devart.AI.McpServer.Odbc.Firebird.csproj -c ReleaseFirebird -r "win-x86" /p:TargetFramework=net8.0
```
>**Note**
>
>The target platform must match the bitness of your ODBC data source.

**Step 3: Configure the database connection for the MCP Server**

1\. Create an `mcpserver.json` configuration file in the directory containing the built MCP Server executable.

2\. In the file, configure the database connection:

```json
{
  "Connections": [
    {
      "Name": "my_firebird",
      "DsnName": "your_dsn_name",
      "ProtocolType": "stdio"
    }
  ]
}
```

Alternatively, you can configure the database connection using the connection string:

```json
{
  "Connections": [
    {
      "Name": "my_firebird",
      "ConnectionString": "Driver={Devart ODBC Driver for Firebird};Server=localhost;User ID=firebird;Password=your_password;Database=your_database;",
      "ProtocolType": "stdio"
    }
  ]
}
```
where:

* `Name` — The name of the ODBC connection.

* `DsnName` — The name of your data source.

* `ProtocolType` — A transport protocol. The possible options are: `stdio` or `http`.

* `HttpPort` (required if `ProtocolType` is set to `http`) — The port number for the `http` protocol. 

**Step 4: Run the MCP server**

After you configure the MCP Server, you can start it. 

>**Note**
>
>This step is required only when `ProtocolType` is configured as `http`. If you use the `stdio` transport protocol, your AI client starts the server automatically.

To start the server, run the following command:

```cmd
Devart.AI.McpServer.Odbc.Firebird.exe run my_firebird
```

where `my_firebird` is the name of the ODBC connection.

**Step 5: Integrate with Claude Desktop**

1\. Open `claude_desktop_config.json`, the Claude configuration file.

>**Tip**
>
>If you can't locate the configuration file, it may not exist yet. To create it, open Claude Desktop and navigate to **File** > **Settings** > **Developer**, then click **Edit Config**. The folder with the `claude_desktop_config.json` file opens.

2\. Add one of the following objects, depending on the transport protocol used by MCP Server:

* STDIO

```json
{
  "mcpServers": {
    "devart": {
      "command": "C:\\path\\to\\Devart.AI.McpServer.Odbc.Firebird.exe",
      "args": [
        "run",
        "my_firebird"
      ]
    }
  }
}
```

 where:

  * `devart` is the connector name that will appear in Claude Desktop.
  * `C:\\path\\to\\Devart.AI.McpServer.Odbc.Firebird.exe` is the path to the executable file.
  * `my_firebird` is the connection name specified in the `mcpserver.json` configuration file.

* **HTTP**

  ```json
    "mcpServers": {
      "devart": {
        "command": "npx",
        "args": [
          "-y",
          "mcp-remote",
          "http://localhost:5000/sse"
        ]
      }
    }
  ```

  where:

  * `devart` is the connector name that will appear in Claude Desktop.
  * `5000` is the MCP Server listening port.

3\. Save the file.

4\. Restart Claude Desktop.

Devart MCP Server for Firebird is now integrated with Claude, and **devart** appears in the Claude Desktop app in **Customize** > **Connectors**.

You can also [integrate](https://docs.devart.com/mcp/ai-integration/) Devart MCP Server for Firebird with other AI clients such as Cline, Codex, Cursor, Visual Studio Code, Windsurf, Zed.

## Supported clients

Devart MCP Server for Firebird supports integration with the following AI clients: 
 
* Claude Desktop
* Visual Studio Code
* Cursor
* Codex
* Windsurf
* Cline
* Zed
* ...and other MCP-compatible AI clients

## Typical use cases

Devart MCP Server for Firebird is a practical fit for teams working with Firebird as their primary data source.

* **ERP and accounting data analysis**  
  Query financial records, journal entries, supplier invoices, and customer accounts stored in Firebird-backed ERP or accounting systems without SQL expertise.

* **Legacy application data access**  
  Use AI to explore and extract insights from Firebird databases embedded in legacy desktop or custom business applications still in active use.

* **Inventory and logistics reporting**  
  Let warehouse and operations teams ask natural-language questions about stock levels, order fulfillment, and logistics data managed in Firebird.

* **Schema documentation and reverse engineering**  
  Help developers understand Firebird schemas, stored procedures, triggers, and business logic in systems that lack current documentation.

* **Ad-hoc business reporting**  
  Generate one-off reports and answer business questions from Firebird data without building dedicated reporting infrastructure.

* **Secure on-premises AI access to Firebird**  
  Keep Firebird data inside your own environment while enabling AI tools to work with it in a controlled, non-destructive way.

## Licensing and activation

Devart MCP Server for Firebird is distributed as a free single-source MCP server.

To connect to Firebird, the server requires the corresponding [Devart ODBC Driver for Firebird](https://www.devart.com/odbc/firebird/), which is a paid product.

A 30-day free trial is available for the Devart ODBC Driver for Firebird.

See the product page and documentation for the latest installation and activation details.

## Support

* [Documentation](https://docs.devart.com/mcp/)
* [Submit a request](https://www.devart.com/company/contactform.html)
* [Suggest a feature](https://devart.uservoice.com/)
* [Join our forum](https://support.devart.com/portal/en/community)

## Other Devart connectivity solutions

* [MCP Server Universal](https://github.com/devart-ai-connectivity/devart-mcp-server-universal)
* [ODBC Driver for Firebird](https://www.devart.com/odbc/firebird/)
