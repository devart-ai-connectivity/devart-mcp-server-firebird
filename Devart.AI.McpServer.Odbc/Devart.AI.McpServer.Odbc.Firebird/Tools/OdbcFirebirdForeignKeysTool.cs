// --------------------------------------------------------------------------
// <copyright file="OdbcFirebirdForeignKeysTool.cs" company="Devart">
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
using Devart.AI.McpServer.Tools;
using Devart.AI.McpServer.Interfaces;

namespace Devart.AI.McpServer.Odbc.Firebird.Tools
{
  internal sealed class OdbcFirebirdForeignKeysTool(McpConfiguration serverConfiguration) : ForeignKeysTool(serverConfiguration)
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
  CAST(TRIM(rc.RDB$CONSTRAINT_NAME) AS VARCHAR(4000)) AS "FK_NAME",
  CAST(TRIM(s.RDB$FIELD_NAME) AS VARCHAR(4000)) AS "FKCOLUMN_NAME",
  NULL AS "PKTABLE_SCHEM",
  CAST(TRIM(i2.RDB$RELATION_NAME) AS VARCHAR(4000)) AS "PKTABLE_NAME",
  CAST(TRIM(s2.RDB$FIELD_NAME) AS VARCHAR(4000)) AS "PKCOLUMN_NAME",
  NULL AS "UPDATE_RULE",
  NULL AS "DELETE_RULE"
FROM
  RDB$INDEX_SEGMENTS s
  LEFT JOIN RDB$INDICES i ON i.RDB$INDEX_NAME = s.RDB$INDEX_NAME
  LEFT JOIN RDB$RELATION_CONSTRAINTS rc ON rc.RDB$INDEX_NAME = s.RDB$INDEX_NAME
  LEFT JOIN RDB$REF_CONSTRAINTS refc ON rc.RDB$CONSTRAINT_NAME = refc.RDB$CONSTRAINT_NAME
  LEFT JOIN RDB$RELATION_CONSTRAINTS rc2 ON rc2.RDB$CONSTRAINT_NAME = refc.RDB$CONST_NAME_UQ
  LEFT JOIN RDB$INDICES i2 ON i2.RDB$INDEX_NAME = rc2.RDB$INDEX_NAME
  LEFT JOIN RDB$INDEX_SEGMENTS s2 ON i2.RDB$INDEX_NAME = s2.RDB$INDEX_NAME
WHERE
  rc.RDB$CONSTRAINT_TYPE = 'FOREIGN KEY'
  AND TRIM(i.RDB$RELATION_NAME) = ?
ORDER BY
  rc.RDB$CONSTRAINT_NAME, s.RDB$FIELD_POSITION
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

      return await reader.ToDataTableAsync(OdbcConstants.ForeignKeysCollectionName, cancellationToken).ConfigureAwait(false);
    }
  }
}
