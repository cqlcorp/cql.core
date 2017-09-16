// ***********************************************************************
// Assembly         : Cql.Core.TransactionScopes
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-14-2017
// ***********************************************************************
// <copyright file="XactScope.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Cql.Core.TransactionScopes
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Transactions;

    using JetBrains.Annotations;

    /// <summary>
    /// A utility class for wrapping async data calls inside a <see cref="TransactionScope" />.
    /// </summary>
    public static class XactScope
    {
        /// <summary>
        /// Execute as an asynchronous operation wrapped in a <see cref="TransactionScope" />.
        /// </summary>
        /// <param name="taskToRunInsideTransactionScope">The task to run inside transaction scope.</param>
        /// <param name="transactionScopeTimeout">The transaction scope timeout (TIP - Use <see cref="SetMaximumTimeout"/> to increase maximum default timeout of 10 minutes).</param>
        /// <returns>A task result.</returns>
        public static async Task ExecuteAsync([NotNull] Func<Task> taskToRunInsideTransactionScope, [CanBeNull] TimeSpan? transactionScopeTimeout = default(TimeSpan?))
        {
            using (var xact = CreateAsyncTransactionScope(transactionScopeTimeout))
            {
                await taskToRunInsideTransactionScope();

                xact.Complete();
            }
        }

        /// <summary>
        /// Execute as an asynchronous operation wrapped in a <see cref="TransactionScope" />.
        /// </summary>
        /// <typeparam name="T">The strongly typed result.</typeparam>
        /// <param name="taskToRunInsideTransactionScope">The task to run inside transaction scope.</param>
        /// <param name="transactionScopeTimeout">The transaction scope timeout (TIP - Use <see cref="SetMaximumTimeout"/> to increase maximum default timeout of 10 minutes).</param>
        /// <returns>A strongly typed task result.</returns>
        public static async Task<T> ExecuteAsync<T>([NotNull] Func<Task<T>> taskToRunInsideTransactionScope, [CanBeNull] TimeSpan? transactionScopeTimeout = default(TimeSpan?))
        {
            if (taskToRunInsideTransactionScope == null)
            {
                throw new ArgumentNullException(nameof(taskToRunInsideTransactionScope));
            }

            using (var xact = CreateAsyncTransactionScope(transactionScopeTimeout))
            {
                var ret = await taskToRunInsideTransactionScope();

                xact.Complete();

                return ret;
            }
        }

        /// <summary>
        /// Sets the maximum timeout.
        /// </summary>
        /// <param name="timeout">The timeout.</param>
        public static void SetMaximumTimeout(TimeSpan timeout)
        {
            SetTransactionManagerField("_cachedMaxTimeout", true);
            SetTransactionManagerField("_maximumTimeout", timeout);
        }

        /// <summary>
        /// Creates the asynchronous transaction scope.
        /// </summary>
        /// <param name="transactionScopeTimeout">The transaction scope timeout.</param>
        /// <returns>A <see cref="TransactionScope" /> that supports Async flow.</returns>
        [NotNull]
        private static TransactionScope CreateAsyncTransactionScope([CanBeNull] TimeSpan? transactionScopeTimeout = default(TimeSpan?))
        {
            const TransactionScopeOption TransactionScopeOption = TransactionScopeOption.Required;
            const TransactionScopeAsyncFlowOption ScopeAsyncFlowOption = TransactionScopeAsyncFlowOption.Enabled;

            return transactionScopeTimeout.HasValue
                       ? new TransactionScope(TransactionScopeOption, transactionScopeTimeout.Value, ScopeAsyncFlowOption)
                       : new TransactionScope(TransactionScopeOption, ScopeAsyncFlowOption);
        }

        /// <summary>
        /// Sets the transaction manager field.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="value">The value.</param>
        private static void SetTransactionManagerField([NotNull] string fieldName, [NotNull] object value)
        {
            typeof(TransactionManager).GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static)?.SetValue(null, value);
        }
    }
}
