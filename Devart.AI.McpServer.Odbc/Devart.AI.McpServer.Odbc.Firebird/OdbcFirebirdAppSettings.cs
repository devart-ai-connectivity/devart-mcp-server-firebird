// --------------------------------------------------------------------------
// <copyright file="OdbcFirebirdAppSettings.cs" company="Devart">
//
// Copyright (c) Devart. ALL RIGHTS RESERVED
// Use of the source code is permitted under the license.
// </copyright>
// --------------------------------------------------------------------------

namespace Devart.AI.McpServer.Odbc.Firebird
{
  internal sealed class OdbcFirebirdAppSettings : McpAppSettings
  {
    public override string ServerName => $"Devart {Properties.ProductInfo.ProductFullName}";

    public override string SourceName => "Firebird";

    public override bool OnPremise => true;
  }
}
