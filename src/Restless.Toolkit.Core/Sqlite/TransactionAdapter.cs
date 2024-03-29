﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;

namespace Restless.Toolkit.Core.Database.SQLite
{
    /// <summary>
    /// Provides transaction services that include fallback for affected <see cref="DataRow"/> objects
    /// </summary>
    public class TransactionAdapter
    {
        #region Private
        private readonly DatabaseControllerBase controller;
        #endregion

        /************************************************************************/

        #region Constructor
        internal TransactionAdapter(DatabaseControllerBase controller)
        {
            this.controller = controller ?? throw new ArgumentNullException("TransactionAdapter.Controller");
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Executes a series of statements within a transaction.
        /// </summary>
        /// <param name="sqlList">The list of sql statements</param>
        /// <remarks>
        /// This method is used to execute a series of sql statements within a transaction.
        /// It works without regard to DataTable objects or DataRow objects.
        /// This (or a variant) can be found on various internet sites.
        /// </remarks>
        public void ExecuteTransaction(IEnumerable<string> sqlList)
        {
            sqlList = sqlList ?? throw new ArgumentNullException("ExecuteTransaction.SqlList");

            // See comments on TableBase.SavePrivate() for use of lock.
            lock (controller.TransactionLockObject)
            {
                using (var transaction = controller.Connection.BeginTransaction())
                {
                    try
                    {
                        foreach (var sql in sqlList)
                        {
                            using (var cmd = new SQLiteCommand(sql, controller.Connection, transaction))
                            {
                                cmd.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Executes a series of statements within a transaction
        /// </summary>
        /// <param name="updateCallback">A callback action to perform updates within the transaction</param>
        /// <param name="tables">The tables that will participate in the transaction.</param>
        /// <remarks>
        /// <para>
        /// This method is used to execute a series of database operations within a transaction.
        /// Unlike <see cref="ExecuteTransaction(IEnumerable{string})"/>, this method handles the rollback
        /// of <see cref="DataRow"/> objects within the specified tables if needed.
        /// </para>
        /// <para>
        /// Rollback of the data rows within the tables requires that the tables are saved (all changes accepted)
        /// before the transaction begins. This is handled automatically by this method. Each table passed
        /// in the <paramref name="tables"/> parameter is saved before the transaction begins.
        /// </para>
        /// <para>
        /// In the <paramref name="updateCallback"/> method, you should perform whatever updates are needed. You must call each particpiating
        /// table's <see cref="TableBase.Save(IDbTransaction)"/> method once (and only once) during the callback, passing the transaction given in the callback.
        /// </para>
        /// </remarks>
        public void ExecuteTransaction(Action<SQLiteTransaction> updateCallback, params TableBase[] tables)
        {
            updateCallback = updateCallback ?? throw new ArgumentNullException("ExecuteTransaction.UpdateCallback");

            if (tables.Count() == 0)
            {
                throw new InvalidOperationException("ExecuteTransaction (no tables)");
            }

            if (tables.Where((t) => t == null).Count() > 0)
            {
                throw new ArgumentNullException("ExecuteTransaction.Tables");
            }

            foreach (var table in tables)
            {
                table.Save();
            }

            // See comments on TableBase.SavePrivate() for use of lock.
            lock (controller.TransactionLockObject)
            {
                using (var transaction = controller.Connection.BeginTransaction())
                {
                    try
                    {
                        updateCallback(transaction);
                        transaction.Commit();
                        foreach (var table in tables)
                        {
                            table.AcceptChanges();
                        }
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        /*
                         * Catch-22 here when a table has a self-relation.
                         * Self-relation.AcceptRejectRule == None (the default)
                         *      RejectChanges() throws with: cannot make this change because constraints are enforced
                         *      
                         * Self-relation.AcceptRejectRule == Cascade     
                         *      RejectChanges() throws with: row has been removed, etc.
                         *      
                         * One thing that works is to have self-relation.AcceptRejectRule == None (the default)
                         * and disable EnforceConstraints on the data set.
                         * 
                         * TODO: Investigate. Currently using .NET 4.5.2 - Possibly behavior is different in later version.
                         */
                        foreach (var table in tables)
                        {
                            bool enforce = table.DataSet.EnforceConstraints;
                            table.DataSet.EnforceConstraints = false;
                            table.RejectChanges();
                            table.DataSet.EnforceConstraints = enforce;
                        }
                        throw;
                    }
                    finally
                    {
                    }
                }
            }
        }
        #endregion

        /************************************************************************/

        #region Private methods
        #endregion
    }
}
