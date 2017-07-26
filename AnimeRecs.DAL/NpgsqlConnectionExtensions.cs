using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Npgsql;

namespace AnimeRecs.DAL
{
    public static class NpgsqlConnectionExtensions
    {
        /// <summary>
        /// Wrapper around Dapper's <c>QueryAsync<typeparamref name="T"/></c> that uses cancellation tokens to implement a timeout.
        /// Npgsql currently only implements backend timeouts for async operations other than connecting and only with cancellation tokens.
        /// The command timeout specified in the connection string or in the command is not used for async operations.
        /// Npgsql does honor cancellation requests via cancellation token by sending a cancel command to the PostgreSQL backend, which cancels
        /// the running query. If connectivity has been lost to the database in a way that does not result in connection resets, async
        /// operations in Npgsql can wait forever, beware! Animerecs is small enough that the app and DB can run on the same server
        /// no problem, so the best solution for now is to do that and run them on the same server.
        /// This function also takes the exception Npgsql throws when doing a backend cancellation as a result of cancellation token
        /// cancellation and wraps it a more appropriate exception that communicates that a timeout occurs. This function
        /// distinguishes between timeout (caused by the timeout time elapsing) and cancellation (caused by the passed-in cancellation token
        /// being cancelled). Timeout is treated as an error condition, with a regular exception type being thrown. Cancellation
        /// is treated as, well, a cancellation, with an OperationCanceledException being thrown.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="transaction"></param>
        /// <param name="timeout"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<IList<T>> QueryAsyncWithCancellation<T>(this NpgsqlConnection conn, string sql, TimeSpan timeout, CancellationToken cancellationToken, NpgsqlTransaction transaction = null)
        {
            using (CancellationTokenSource timeoutSource = new CancellationTokenSource(timeout))
            using (CancellationTokenSource queryCancel = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutSource.Token))
            {
                CommandDefinition command = new CommandDefinition(sql, transaction: transaction, cancellationToken: queryCancel.Token);
                Task<IEnumerable<T>> queryTask = null;
                IEnumerable<T> results;
                try
                {
                    queryTask = conn.QueryAsync<T>(command);
                    results = await queryTask.ConfigureAwait(false);
                }
                catch (PostgresException ex) when (ex.SqlState == "57014")
                {
                    // If PostgresException with SqlState = "57014", a backend cancellation via the cancellationtoken occured.
                    // If it was due to timeout, throw an exception that makes that clear.
                    // Otherwise if it was due to cancellation being requested, throw an OperationCanceledException

                    if (cancellationToken.IsCancellationRequested)
                    {
                        throw new OperationCanceledException("Cancellation of query was requested.", ex, cancellationToken);
                    }
                    else
                    {
                        throw new NpgsqlException("Query timed out.", ex);
                    }
                }

                return results.AsList();
            }
        }
    }
}

// Copyright (C) 2017 Greg Najda
//
// This file is part of AnimeRecs.DAL.
//
// AnimeRecs.DAL is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.DAL is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.DAL.  If not, see <http://www.gnu.org/licenses/>.