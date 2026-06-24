// --------------------------------------------------------------------------
// <copyright file="OdbcFirebirdPrimaryKeysTool.cs" company="Devart">
//
// Copyright (c) Devart. ALL RIGHTS RESERVED
// Use of the source code is permitted under the license.
// </copyright>
// --------------------------------------------------------------------------

using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Devart.AI.McpServer.Extensions;
using Devart.AI.McpServer.Interfaces;
using Devart.AI.McpServer.Tools;

namespace Devart.AI.McpServer.Odbc.Firebird.Tools
{
  internal sealed class OdbcFirebirdPrimaryKeysTool(McpConfiguration serverConfiguration) : PrimaryKeysTool(serverConfiguration)
  {
    protected override async Task<DataTable> GetMetadataTable(
      DbConnection connection,
      string schema,
      string tableName,
      IServiceProvider services,
      CancellationToken cancellationToken)
    {
      const string sql =
"""
SELECT
  CAST(TRIM(R.RDB$CONSTRAINT_NAME) AS VARCHAR(4000)) AS "PK_NAME",
  CAST(TRIM(RF.RDB$FIELD_NAME) AS VARCHAR(4000)) AS "COLUMN_NAME"
FROM RDB$RELATION_CONSTRAINTS R 
  INNER JOIN RDB$INDEX_SEGMENTS I ON I.RDB$INDEX_NAME = R.RDB$INDEX_NAME 
  INNER JOIN RDB$RELATION_FIELDS RF ON RF.RDB$FIELD_NAME = I.RDB$FIELD_NAME AND 
        RF.RDB$RELATION_NAME = R.RDB$RELATION_NAME 
  INNER JOIN RDB$FIELDS F ON F.RDB$FIELD_NAME = RF.RDB$FIELD_SOURCE 
WHERE R.RDB$CONSTRAINT_TYPE = 'PRIMARY KEY' 
      AND R.RDB$RELATION_NAME = ? 
ORDER BY 1, 2
""";
      var database = services.GetRequiredService<IDatabase>();
      var commandHelper = services.GetRequiredService<ICommandHelper>();

      await using var reader = await database.ExecuteReaderAsync(
        connection,
        sql,
        cmd =>
        {
          commandHelper.AddParameter(cmd, tableName);
        },
        cancellationToken
      ).ConfigureAwait(false);

      return await reader.ToDataTableAsync(OdbcConstants.PrimaryKeysCollectionName, cancellationToken).ConfigureAwait(false);
    }
  }
}
