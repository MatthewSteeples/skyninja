﻿using System;
using System.Data.Common;
using System.Data.SQLite;
using System.Threading.Tasks;

using NLog;

using SkyNinja.Core.Classes;
using SkyNinja.Core.Exceptions;

namespace SkyNinja.Core.Inputs.Skype
{
    /// <summary>
    /// Skype database input connector.
    /// </summary>
    internal class SkypeInput: Input
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private const string GetConversationsQuery = @"
            select id as id,
                   identity as identity,
                   displayname as displayName
            from conversations
        ";

        private readonly string databasePath;

        private SQLiteConnection connection;

        public SkypeInput(string databasePath)
        {
            this.databasePath = databasePath;
        }

        public override async Task Open()
        {
            string connectionString = String.Format("Data Source={0};Read Only=True", databasePath);
            Logger.Debug("Connection string: {0}", connectionString);
            connection = new SQLiteConnection(connectionString);
            Logger.Debug("Opening ...");
            try
            {
                await connection.OpenAsync();
            }
            catch (SQLiteException e)
            {
                throw new InternalException("Failed to open database.", e);
            }
            Logger.Info("Opened.");
            // Add trace handler.
            connection.Trace += ConnectionTrace;
        }

        public override async Task<ConversationEnumerator> GetConversationsAsync()
        {
            Logger.Debug("Getting conversations ...");
            using (SQLiteCommand command = new SQLiteCommand(GetConversationsQuery, connection))
            {
                DbDataReader reader = await command.ExecuteReaderAsync();
                return new SkypeConversationEnumerator(reader);
            }
        }

        public override void Close()
        {
            if (connection != null)
            {
                connection.Close();
            }
        }

        private void ConnectionTrace(object sender, TraceEventArgs e)
        {
            Logger.Trace("SQL: {0}", e.Statement);
        }
    }
}
