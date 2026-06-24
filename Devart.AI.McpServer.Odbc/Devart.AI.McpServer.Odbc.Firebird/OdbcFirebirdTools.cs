// --------------------------------------------------------------------------
// <copyright file="OdbcFirebirdTools.cs" company="Devart">
//
// Copyright (c) Devart. ALL RIGHTS RESERVED
// Use of the source code is permitted under the license.
// </copyright>
// --------------------------------------------------------------------------

using System.Collections.Generic;
using ModelContextProtocol.Server;
using Devart.AI.McpServer.Odbc.Firebird.Tools;

namespace Devart.AI.McpServer.Odbc.Firebird
{
  internal static class OdbcFirebirdTools
  {
    public static List<McpServerTool> CreateTools(McpConfiguration configuration)
      => OdbcTools.CreateBuilder(configuration)
        .Add(new OdbcFirebirdPrimaryKeysTool(configuration))
        .Add(new OdbcFirebirdForeignKeysTool(configuration))
        .Build();
  }
}
