﻿// -----------------------------------------------------------------------
// <copyright file="ExtensionMethodTests.cs" company="Andrii Kurdiumov">
// Copyright (c) Andrii Kurdiumov. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace SqlMarshal.Tests;

using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class ExtensionMethodTests : CodeGenerationTestBase
{
    [TestMethod]
    public void ScalarResult()
    {
        string source = @"
namespace Foo
{
    static class C
    {
        [SqlMarshal(""sp_TestSP"")]
        public static partial Task<int> M(this DbConnection connection, int clientId, string? personId);
    }
}";
        string output = this.GetGeneratedOutput(source, NullableContextOptions.Disable);

        Assert.IsNotNull(output);

        var expectedOutput = @"// <auto-generated>
// Code generated by Stored Procedures Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>
#nullable enable
#pragma warning disable 1591

namespace Foo
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Linq;

    static partial class C
    {
        public static partial async Task<int> M(this DbConnection connection, int clientId, string? personId)
        {
            
            using var command = connection.CreateCommand();

            var clientIdParameter = command.CreateParameter();
            clientIdParameter.ParameterName = ""@client_id"";
            clientIdParameter.Value = clientId;

            var personIdParameter = command.CreateParameter();
            personIdParameter.ParameterName = ""@person_id"";
            personIdParameter.Value = personId == null ? (object)DBNull.Value : personId;

            var parameters = new DbParameter[]
            {
                clientIdParameter,
                personIdParameter,
            };

            var sqlQuery = @""sp_TestSP @client_id, @person_id"";
            command.CommandText = sqlQuery;
            command.Parameters.AddRange(parameters);
            var result = await command.ExecuteScalarAsync().ConfigureAwait(false);
            return (int)result;
        }
    }
}";
        Assert.AreEqual(expectedOutput, output);
    }

    [TestMethod]
    public void ScalarResultWithoutThis()
    {
        string source = @"
namespace Foo
{
    static class C
    {
        [SqlMarshal(""sp_TestSP"")]
        public static partial Task<int> M(DbConnection connection, int clientId, string? personId);
    }
}";
        string output = this.GetGeneratedOutput(source, NullableContextOptions.Disable);

        Assert.IsNotNull(output);

        var expectedOutput = @"// <auto-generated>
// Code generated by Stored Procedures Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>
#nullable enable
#pragma warning disable 1591

namespace Foo
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Linq;

    static partial class C
    {
        public static partial async Task<int> M(DbConnection connection, int clientId, string? personId)
        {
            
            using var command = connection.CreateCommand();

            var clientIdParameter = command.CreateParameter();
            clientIdParameter.ParameterName = ""@client_id"";
            clientIdParameter.Value = clientId;

            var personIdParameter = command.CreateParameter();
            personIdParameter.ParameterName = ""@person_id"";
            personIdParameter.Value = personId == null ? (object)DBNull.Value : personId;

            var parameters = new DbParameter[]
            {
                clientIdParameter,
                personIdParameter,
            };

            var sqlQuery = @""sp_TestSP @client_id, @person_id"";
            command.CommandText = sqlQuery;
            command.Parameters.AddRange(parameters);
            var result = await command.ExecuteScalarAsync().ConfigureAwait(false);
            return (int)result;
        }
    }
}";
        Assert.AreEqual(expectedOutput, output);
    }

    [TestMethod]
    public void ConnectionCanHandDifferentName()
    {
        string source = @"
namespace Foo
{
    static class C
    {
        [SqlMarshal(""sp_TestSP"")]
        public static partial Task<int> M(DbConnection conn, int clientId, string? personId);
    }
}";
        string output = this.GetGeneratedOutput(source, NullableContextOptions.Disable);

        Assert.IsNotNull(output);

        var expectedOutput = @"// <auto-generated>
// Code generated by Stored Procedures Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>
#nullable enable
#pragma warning disable 1591

namespace Foo
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Linq;

    static partial class C
    {
        public static partial async Task<int> M(DbConnection conn, int clientId, string? personId)
        {
            var connection = conn;
            using var command = connection.CreateCommand();

            var clientIdParameter = command.CreateParameter();
            clientIdParameter.ParameterName = ""@client_id"";
            clientIdParameter.Value = clientId;

            var personIdParameter = command.CreateParameter();
            personIdParameter.ParameterName = ""@person_id"";
            personIdParameter.Value = personId == null ? (object)DBNull.Value : personId;

            var parameters = new DbParameter[]
            {
                clientIdParameter,
                personIdParameter,
            };

            var sqlQuery = @""sp_TestSP @client_id, @person_id"";
            command.CommandText = sqlQuery;
            command.Parameters.AddRange(parameters);
            var result = await command.ExecuteScalarAsync().ConfigureAwait(false);
            return (int)result;
        }
    }
}";
        Assert.AreEqual(expectedOutput, output);
    }

    [TestMethod]
    public void MapResultSetToProcedure()
    {
        string source = @"
namespace Foo
{
    public class Item
    {
        public string StringValue { get; set; }
        public int Int32Value { get; set; }
        public int? NullableInt32Value { get; set; }
    }

    class C
    {
        [SqlMarshal(""sp_TestSP"")]
        public partial Task<IList<Item>> M(DbConnection connection)
    }
}";
        string output = this.GetGeneratedOutput(source, NullableContextOptions.Disable);

        Assert.IsNotNull(output);

        var expectedOutput = @"// <auto-generated>
// Code generated by Stored Procedures Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>
#nullable enable
#pragma warning disable 1591

namespace Foo
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Linq;

    partial class C
    {
        public partial async Task<IList<Foo.Item>> M(DbConnection connection)
        {
            
            using var command = connection.CreateCommand();

            var sqlQuery = @""sp_TestSP"";
            command.CommandText = sqlQuery;
            using var reader = await command.ExecuteReaderAsync().ConfigureAwait(false);
            var result = new List<Item>();
            while (await reader.ReadAsync().ConfigureAwait(false))
            {
                var item = new Item();
                var value_0 = reader.GetValue(0);
                item.StringValue = value_0 == DBNull.Value ? (string?)null : (string)value_0;
                var value_1 = reader.GetValue(1);
                item.Int32Value = (int)value_1;
                var value_2 = reader.GetValue(2);
                item.NullableInt32Value = value_2 == DBNull.Value ? (int?)null : (int)value_2;
                result.Add(item);
            }

            await reader.CloseAsync().ConfigureAwait(false);
            return result;
        }
    }
}";
        Assert.AreEqual(expectedOutput, output);
    }

    [TestMethod]
    public void ScalarResultDbContext()
    {
        string source = @"
namespace Foo
{
    public partial class CustomDbContext : DbContext
    {
        public virtual DbSet<Item>? Items { get; set; } = null!;
        public virtual DbSet<PersonItem>? Persons { get; set; } = null!;
    }

    static class C
    {
        [SqlMarshal(""sp_TestSP"")]
        public static partial int M(this CustomDbContext context, int clientId, string? personId);
    }
}";
        string output = this.GetGeneratedOutput(source, NullableContextOptions.Disable);

        Assert.IsNotNull(output);

        var expectedOutput = @"// <auto-generated>
// Code generated by Stored Procedures Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>
#nullable enable
#pragma warning disable 1591

namespace Foo
{
    using System;
    using System.Data.Common;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage;

    static partial class C
    {
        public static partial int M(this Foo.CustomDbContext context, int clientId, string? personId)
        {
            var connection = context.Database.GetDbConnection();
            using var command = connection.CreateCommand();

            var clientIdParameter = command.CreateParameter();
            clientIdParameter.ParameterName = ""@client_id"";
            clientIdParameter.Value = clientId;

            var personIdParameter = command.CreateParameter();
            personIdParameter.ParameterName = ""@person_id"";
            personIdParameter.Value = personId == null ? (object)DBNull.Value : personId;

            var parameters = new DbParameter[]
            {
                clientIdParameter,
                personIdParameter,
            };

            var sqlQuery = @""sp_TestSP @client_id, @person_id"";
            command.CommandText = sqlQuery;
            command.Parameters.AddRange(parameters);
            command.Transaction = context.Database.CurrentTransaction?.GetDbTransaction();
            context.Database.OpenConnection();
            try
            {
                var result = command.ExecuteScalar();
                return (int)result;
            }
            finally
            {
                context.Database.CloseConnection();
            }
        }
    }
}";
        Assert.AreEqual(expectedOutput, output);
    }
}
