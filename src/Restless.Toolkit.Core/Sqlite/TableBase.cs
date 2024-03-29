﻿using Restless.Toolkit.Core.Resources;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace Restless.Toolkit.Core.Database.SQLite
{
    /// <summary>
    /// Represents the base class for a data table
    /// </summary>
    public abstract class TableBase : DataTable
    {
        #region Private


        // This provides a way to avoid updating the db uneccesarily due to calculated columns.
        // When an eligible column is changed, it's added to this list. Later, during update, we check
        // to see if there's any need to update.
        private readonly Dictionary<DataRow,List<DataColumn>> changedEligibleColumns;
        #endregion

        /************************************************************************/

        #region Public fields / properties
        /// <summary>
        /// Gets a value that indicates whether or not this table is read only. When True, no update operations are allowed
        /// </summary>
        public bool IsReadOnly
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets a value that indicates if delete operations are allowed on this table.
        /// </summary>
        public bool IsDeleteRestricted
        {
            get;
            protected set;
        }

        /// <summary>
        /// When implemented in a derived class, gets the column name of the primary key, 
        /// or null if there is no single column primary key
        /// </summary>
        public abstract string PrimaryKeyName
        {
            get;
        }

        /// <summary>
        /// Gets the schema version for the table. Override if needed.
        /// </summary>
        public virtual long SchemaVersion => Controller.DefaultSchemaVersion;
        #endregion

        /************************************************************************/

        #region Protected properties
        /// <summary>
        /// Column name that SQLite automatically provides which uniquely identifies the row
        /// </summary>
        protected const string RowId = "_rowid_";

        /// <summary>
        /// Alias of <see cref="RowId"/>
        /// </summary>
        protected const string RowIdAlias = "SYSROWID";

        /// <summary>
        /// Gets the database controller assigned to this table
        /// </summary>
        protected DatabaseControllerBase Controller
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the adapter
        /// </summary>
        protected SQLiteDataAdapter Adapter
        {
            get;
        }
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="TableBase"/> class.
        /// </summary>
        /// <param name="controller">The database controller object.</param>
        /// <param name="schemaName">The name of the schema this table belongs to.</param>
        /// <param name="tableName">The name of the table.</param>
        protected TableBase(DatabaseControllerBase controller, string schemaName, string tableName)
        {
            if (string.IsNullOrEmpty(schemaName)) throw new ArgumentNullException(nameof(schemaName));
            if (string.IsNullOrEmpty(tableName)) throw new ArgumentNullException(nameof(tableName));
            Controller = controller ?? throw new ArgumentNullException(nameof(controller));

            Namespace = schemaName;
            TableName = tableName;
            IsReadOnly = false;
            IsDeleteRestricted = false;
            Adapter = new SQLiteDataAdapter
            {
                SelectCommand = new SQLiteCommand(controller.Connection),
                InsertCommand = new SQLiteCommand(controller.Connection),
                UpdateCommand = new SQLiteCommand(controller.Connection),
                DeleteCommand = new SQLiteCommand(controller.Connection)
            };
            changedEligibleColumns = new Dictionary<DataRow, List<DataColumn>>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableBase"/> class.
        /// </summary>
        /// <param name="controller">The database controller object.</param>
        /// <param name="tableName">The name of the table.</param>
        protected TableBase(DatabaseControllerBase controller, string tableName)
            : this(controller, DatabaseControllerBase.MainSchemaName, tableName)
        {
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Gets a boolean value that indicates if this table exists within the database.
        /// </summary>
        /// <returns>true if the table exists within the database; otherwise, false.</returns>
        public bool Exists()
        {
            string sql = $"SELECT name FROM {Namespace}.sqlite_master WHERE type='table' AND name='{TableName}'";
            var obj = Controller.Execution.Scalar(sql);
            return (obj != null);
        }

        /// <summary>
        /// Gets a boolean value that indicates if this table has any rows within the database.
        /// </summary>
        /// <returns>true if the table has any rows within the database; otherwise, false.</returns>
        public bool HasRows()
        {
            string sql = $"SELECT COUNT(*) AS C FROM {Namespace}.{TableName}";
            long count = (long)Controller.Execution.Scalar(sql);
            return count > 0;
        }
        
        /// <summary>
        /// When implemented in a derived class, loads the data.
        /// </summary>
        public abstract void Load();

        /// <summary>
        /// Saves the data that has changed.
        /// </summary>
        /// <param name="transaction">The transaction associated with this save operation.</param>
        /// <remarks>
        /// The <paramref name="transaction"/> parameter is designed to be used with the
        /// <see cref="TransactionAdapter.ExecuteTransaction(Action{SQLiteTransaction}, TableBase[])"/> method.
        /// </remarks>
        public void Save(IDbTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            SavePrivate(transaction);
        }

        /// <summary>
        /// Saves the data that has changed.
        /// </summary>
        public void Save()
        {
            SavePrivate(null);
        }

        /// <summary>
        /// Adds a new row to the table with default values, and calls the Save() method.
        /// </summary>
        /// <returns>The newly created data row</returns>
        public DataRow AddDefaultRow()
        {
            if (IsReadOnly)
            {
                throw new InvalidOperationException(Strings.InvalidOperationTableIsReadOnly);
            }
            DataRow row = NewRow();
            PopulateDefaultRow(row);
            Rows.Add(row);
            Save();
            return row;
        }

        /// <summary>
        /// Gets a value that indicates if this table has unsaved changes.
        /// </summary>
        /// <returns>true if any row needs inserting, updating, or deleting; otherwise, false.</returns>
        public bool IsDirty()
        {
            if (!IsReadOnly)
            {
                UpdateStatus status = new UpdateStatus(this);
                return status.HaveAny;
            }
            return false;
        }
        #endregion

        /************************************************************************/

        #region Protected methods
        /// <summary>
        /// When implemented in a derived class, gets the DDL needed to create this table.
        /// </summary>
        /// <returns>A string that has the DDL needed to create the table</returns>
        protected abstract string GetDdl();

        /// <summary>
        /// Override in a derived class to get the SQL needed to populate this table
        /// with default data, such as lookup data, configuration values, etc.
        /// This method is only called when the table has no rows.
        /// </summary>
        /// <returns>A sql string needed to populate the table with any required one-time, default data</returns>
        protected virtual string GetPopulateSql()
        {
            return null;
        }

        /// <summary>
        /// Loads the data
        /// </summary>
        /// <param name="where">A SQL where clause (without the WHERE keyword), or null for all records.</param>
        /// <param name="orderBy">A SQL order clause (without the ORDER BY keyword), or null for no specific order.</param>
        /// <param name="fields">A list if database field names to load</param>
        protected void Load(string where, string orderBy, params string[] fields)
        {
            StringBuilder sql = new StringBuilder($"SELECT {RowId} AS {RowIdAlias},", 512);

            if (fields.Length == 0) sql.Append("*,");
            foreach (string field in fields)
            {
                sql.Append($"{field},");
            }
            // get rid of the last comma
            sql.Remove(sql.Length - 1, 1);
            sql.Append($" FROM {Namespace}.{TableName}");
            if (!string.IsNullOrEmpty(where))
            {
                sql.Append($" WHERE {where}");
            }
            if (!string.IsNullOrEmpty(orderBy))
            {
                sql.Append($" ORDER BY {orderBy}");
            }

            Columns.Clear();
            Clear();
            Adapter.SelectCommand.CommandText = sql.ToString();
            Adapter.Fill(this);
            SetColumnProperties();
        }

        /// <summary>
        /// Gets the DDL used to create this table by querying the master metadata table
        /// </summary>
        /// <returns>The DDL used to create this table.</returns>
        protected string GetDdlByQuery()
        {
            string sql = $"SELECT sql FROM {Namespace}.sqlite_master WHERE name='{TableName}'";
            return Controller.Execution.Scalar(sql).ToString();
        }

        /// <summary>
        /// Creates a relation to a child table
        /// </summary>
        /// <typeparam name="T">The child type</typeparam>
        /// <param name="name">The name of the relation</param>
        /// <param name="parentColumnName">The parent column name, i.e. name of column in this table</param>
        /// <param name="childColumnName">The child column name, i.e. name of column in child table T</param>
        /// <param name="acceptRejectRule">The accept/reject rule to apply to child rows</param>
        /// <param name="deleteRule">The delete rule to apply to child rows</param>
        /// <param name="childColumnDefaultValue">Default value for child column. Needed when <paramref name="deleteRule"/> is Rule.SetDefault</param>
        protected void CreateParentChildRelation<T>
            (
                string name,
                string parentColumnName,
                string childColumnName,
                AcceptRejectRule acceptRejectRule = AcceptRejectRule.None,
                Rule deleteRule = Rule.Cascade,
                object childColumnDefaultValue = null
            ) where T: TableBase
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrEmpty(parentColumnName)) throw new ArgumentNullException(nameof(parentColumnName));
            if (string.IsNullOrEmpty(childColumnName)) throw new ArgumentNullException(nameof(childColumnName));

            var child = Controller.GetTable<T>();
            DataRelation r = new DataRelation(name, Columns[parentColumnName], child.Columns[childColumnName]);
            ChildRelations.Add(r);
            r.ChildKeyConstraint.AcceptRejectRule = acceptRejectRule;
            r.ChildKeyConstraint.DeleteRule = deleteRule;
            child.Columns[childColumnName].DefaultValue = childColumnDefaultValue;
        }

        /// <summary>
        /// Creates an expression column to get data via a previously defined relation.
        /// </summary>
        /// <typeparam name="T">The type of the data column</typeparam>
        /// <param name="colName">The name of the new column to add to this table, the child table</param>
        /// <param name="relationName">The relation name, created by parent</param>
        /// <param name="parentColName">The name of the parent column that will populate this column</param>
        protected void CreateChildToParentColumn<T>(string colName, string relationName, string parentColName)
        {
            if (string.IsNullOrEmpty(colName)) throw new ArgumentNullException(nameof(colName));
            if (string.IsNullOrEmpty(relationName)) throw new ArgumentNullException(nameof(relationName));
            if (string.IsNullOrEmpty(parentColName)) throw new ArgumentNullException(nameof(parentColName));

            // This is related to DatabaseControllerBase.TableRegistrationComplete() in that a table
            // that failed to create its columns will get a chance to try again. However, we only 
            // want the table to create a column if it was unsuccessful the first time. It may have
            // successfully created columns A and B, but failed on column C
           
            if (!Columns.Contains(colName))
            {
                DataColumn col = new DataColumn(colName)
                {
                    DataType = typeof(T),
                    Expression = string.Format("Parent({0}).{1}", relationName, parentColName)
                };
                Columns.Add(col);
            }
        }

        /// <summary>
        /// Creates an expression column to get data via a previously defined relation. Uses default type of string.
        /// </summary>
        /// <param name="colName">The name of the new column to add to this table, the child table</param>
        /// <param name="relationName">The relation name, created by parent</param>
        /// <param name="parentColName">The name of the parent column that will populate this column</param>
        protected void CreateChildToParentColumn(string colName, string relationName, string parentColName)
        {
            CreateChildToParentColumn<string>(colName, relationName, parentColName);
        }

        /// <summary>
        /// Creates an expression column using a caller defined expression.
        /// </summary>
        /// <typeparam name="T">The type of the data column</typeparam>
        /// <param name="colName">The name of the new column to add to this table</param>
        /// <param name="expression">The expression as created by the caller</param>
        protected void CreateExpressionColumn<T>(string colName, string expression)
        {
            if (string.IsNullOrEmpty(colName)) throw new ArgumentNullException(nameof(colName));
            if (string.IsNullOrEmpty(expression)) throw new ArgumentNullException(nameof(expression));

            // This is related to DatabaseControllerBase.TableRegistrationComplete() in that a table
            // that failed to create its columns will get a chance to try again. However, we only 
            // want the table to create a column if it was unsuccessful the first time. It may have
            // successfully created columns A and B, but failed on column C
            
            if (!Columns.Contains(colName))
            {
                DataColumn col = new DataColumn(colName)
                {
                    DataType = typeof(T),
                    Expression = expression
                };
                Columns.Add(col);
            }
        }

        /// <summary>
        /// Creates a column that gets its value from a callback method.
        /// </summary>
        /// <typeparam name="T">The type for the created column.</typeparam>
        /// <param name="colName">The name of the column.</param>
        /// <param name="dependentTable">The dependent table</param>
        /// <param name="updateAction">The action to perform when requesting a value for this column</param>
        /// <param name="dependentColumns">The names of the columns in the dependent table that when changed trigger the update.</param>
        protected void CreateActionExpressionColumn<T>(string colName, TableBase dependentTable, Action<ActionDataColumn, DataRowChangeEventArgs> updateAction, params string[] dependentColumns)
        {
            if (string.IsNullOrEmpty(colName)) throw new ArgumentNullException(nameof(colName));
            if (updateAction == null) throw new ArgumentNullException(nameof(updateAction));

            if (!Columns.Contains(colName))
            {
                var col = new ActionDataColumn(colName, typeof(T), updateAction);
                col.SetDependentTable(dependentTable, dependentColumns);
                Columns.Add(col);
            }
        }

        /// <summary>
        /// Provides a generic IEnumerable for the data rows of this table.
        /// </summary>
        /// <returns>An IEnumerable of <see cref="DataRow"/>.</returns>
        protected IEnumerable<DataRow> EnumerateRows()
        {
            foreach (DataRow row in Rows)
            {
                yield return row;
            }
        }

        /// <summary>
        /// Provides a generic IEnumerable for the data rows of this table.
        /// </summary>
        /// <param name="filter">An expression that determines which rows will be selected.</param>
        /// <returns>An IEnumerable of <see cref="DataRow"/>.</returns>
        protected IEnumerable<DataRow> EnumerateRows(string filter)
        {
            foreach (DataRow row in Select(filter))
            {
                yield return row;
            }
        }

        /// <summary>
        /// Provides a generic IEnumerable for the data rows of this table.
        /// </summary>
        /// <param name="filter">An expression that determines which rows will be selected.</param>
        /// <param name="orderBy">An expression that determines how the rows will be ordered.</param>
        /// <returns>An IEnumerable of <see cref="DataRow"/>.</returns>
        protected IEnumerable<DataRow> EnumerateRows(string filter, string orderBy)
        {
            foreach (DataRow row in Select(filter, orderBy))
            {
                yield return row;
            }
        }

        /// <summary>
        /// Sets custom properties on a column.
        /// </summary>
        /// <param name="col">The column.</param>
        /// <param name="keys">Value from the <see cref="DataColumnPropertyKey"/> enumeration.</param>
        /// <remarks>
        /// This method enables a derived class to specify how a column should be treated
        /// during insert and update operations. You can specify that a column is not used
        /// during insert/update operations and/or that a column should receive a copy of 
        /// the freshly inserted row id during an insert operation. The latter should be used
        /// on columns that have been declared INTEGER PRIMARY KEY, as this is an alias 
        /// for the _rowid_ column that SQLite creates.
        /// </remarks>
        protected void SetColumnProperty(DataColumn col, params DataColumnPropertyKey[] keys)
        {
            if (col == null) throw new ArgumentNullException(nameof(col));
            
            foreach (DataColumnPropertyKey key in keys)
            {
                if (!col.ExtendedProperties.ContainsKey(key))
                {
                    col.ExtendedProperties.Add(key, 1);
                }
            }
        }

        /// <summary>
        /// Adds a column to the table within the database if it doesn't exist.
        /// </summary>
        /// <param name="colName">The column name</param>
        /// <param name="colDefinition">The column defintion, ex: TEXT NOT NULL</param>
        /// <remarks>
        /// This method is used to alter the database table by adding another column.
        /// It may be used for example during a version update when another column should be added.
        /// </remarks>
        protected void AddColumn(string colName, string colDefinition)
        {
            if (string.IsNullOrEmpty(colName)) throw new ArgumentNullException(nameof(colName));
            if (string.IsNullOrEmpty(colDefinition)) throw new ArgumentNullException(nameof(colDefinition));

            if (!ColumnExists(colName))
            {
                string sql = $"ALTER TABLE {Namespace}.{TableName} ADD COLUMN `{colName}` {colDefinition}";
                Controller.Execution.NonQuery(sql);
            }
        }

        /// <summary>
        /// Enables a derived class to establish column settings.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Column defintions on the table are not available until the data adapter has had an opportunity to fill the table.
        /// During the adapter's fill operation, it creates the column definitions.
        /// </para>
        /// <para>
        /// This method is called after the table is filled and enables a derived class to call the <see cref="SetColumnProperty"/> method
        /// to set extended properties on the column.
        /// </para>
        /// </remarks>
        protected abstract void SetColumnProperties();

        /// <summary>
        /// Enables a derived class to populate a new row with default (starter) values.
        /// </summary>
        /// <param name="row">The freshly created DataRow to poulate</param>
        /// <remarks>
        /// Override this method in a derived class to provide default (starter) values for the row.
        /// If the child class does not support this operation, do not override. The base implementation
        /// throws a NotImplementedException.
        /// </remarks>
        protected virtual void PopulateDefaultRow(DataRow row)
        {
            throw new NotImplementedException(Strings.MethodNotImplemented);
        }

        /// <summary>
        /// Override in a derived class to perform post registration operations such as setting table relations. The controller
        /// calls this method after all tables have been registered. The base method does nothing.
        /// </summary>
        protected internal virtual void SetDataRelations()
        {
        }

        /// <summary>
        /// Override in a derived class to create calculated columns based on established relations. The controller
        /// calls this method after all tables have been registered and all tables have had an opportunity
        /// to set data relations via the <see cref="SetDataRelations"/> method. The base method does nothing.
        /// </summary>
        protected internal virtual void UseDataRelations()
        {
        }

        /// <summary>
        /// Override in a derived class to perform special operations when initialization has started.
        /// The controller calls this method during registration, after the table has been created (if needed),
        /// but before any data is loaded. The base method does nothing.
        /// </summary>
        protected internal virtual void OnInitializationStarted()
        {
        }

        /// <summary>
        /// Override in a derived class to perform special operations after load and relation operations.
        /// The controller calls this method after all tables have had an opportunity to set data relations
        /// via the  <see cref="SetDataRelations"/> method, and use data relations via the <see cref="UseDataRelations"/> method.
        /// The base method does nothing.
        /// </summary>
        protected internal virtual void OnInitializationComplete()
        {
        }

        /// <summary>
        /// Override in a derived class to perform any cleanup operations prior to shutting down the connection.
        /// The controller calls this method for each table from its Shutdown() method.
        /// </summary>
        /// <param name="saveOnShutdown">The value that was passed to the controller's Shutdown method. 
        /// If false, the table needs to call its Save() method if it makes changes to data.
        /// </param>
        protected internal virtual void OnShuttingDown(bool saveOnShutdown)
        {
        }

        /// <summary>
        /// Called when a colum has changed.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnColumnChanged(DataColumnChangeEventArgs e)
        {
            base.OnColumnChanged(e);
            if (IsColumnEligible(e.Column, DataColumnPropertyKey.ExcludeFromUpdate))
            {
                if (!changedEligibleColumns.ContainsKey(e.Row))
                {
                    changedEligibleColumns.Add(e.Row, new List<DataColumn>());
                }

                if (!changedEligibleColumns[e.Row].Contains(e.Column))
                {
                    changedEligibleColumns[e.Row].Add(e.Column);
                }
            }
        }

        /// <summary>
        /// Gets the corresponding database type from the specified .net type
        /// </summary>
        /// <param name="type">The .net type</param>
        /// <returns>The corresponding database type</returns>
        protected DbType NetTypeToDbType(Type type)
        {
            if (type == typeof(string)) return DbType.String;
            if (type == typeof(int)) return DbType.Int32;
            if (type == typeof(long)) return DbType.Int64;
            if (type == typeof(DateTime)) return DbType.DateTime;
            if (type == typeof(bool)) return DbType.Boolean;
            return DbType.String;
        }

        /// <summary>
        /// Gets a boolean value that determines if the specified column
        /// is eligible for an operation according to its exclusion key
        /// </summary>
        /// <param name="col">The column</param>
        /// <param name="exclusionKey">The exclusion key</param>
        /// <returns>true if column is eligible; otherwise, false.</returns>
        /// <remarks>
        /// This method returns true if the column in not null, not read only,
        /// is not an expression column, is not the <see cref="RowIdAlias"/> column,
        /// and does contain the specified <paramref name="exclusionKey"/>.
        /// </remarks>
        protected bool IsColumnEligible(DataColumn col, DataColumnPropertyKey exclusionKey)
        {
            return
                col != null &&
                !col.ReadOnly &&
                string.IsNullOrEmpty(col.Expression) &&
                col.ColumnName != RowIdAlias &&
                !col.ExtendedProperties.ContainsKey(exclusionKey);
        }
        #endregion

        /************************************************************************/

        #region Internal methods
        /// <summary>
        /// Creates the table in the database from the DDL text.
        /// </summary>
        internal void CreateFromDdl()
        {
            string sql = GetDdl().Trim();
            if (string.IsNullOrEmpty(sql)) throw new InvalidOperationException(Strings.InvalidOperationEmptyDdl);
            sql = SubstituteSchemaAndName(sql);
            Controller.Execution.NonQuery(sql);
        }

        /// <summary>
        /// Populate the table with default data
        /// </summary>
        /// <remarks>
        /// This method is called by DatabaseControllerBase at startup if the table has no rows and its behavior flags include
        /// DatabaseControllerBehavior.AutoPopulate
        /// </remarks>
        internal void Populate()
        {
            string sql = GetPopulateSql();
            if (!string.IsNullOrEmpty(sql))
            {
                sql = SubstituteSchemaAndName(sql);
                Controller.Execution.NonQuery(sql);
            }
        }
        #endregion

        /************************************************************************/

        #region Private methods
        /// <summary>
        /// Performs substitution on <paramref name="input"/> by replacing
        /// "{NS}" with the namespace of this table and "{NAME}" with the name of this table.
        /// </summary>
        /// <param name="input">The input string, typically a DDL statement</param>
        /// <returns>The substituted string.</returns>
        private string SubstituteSchemaAndName(string input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            return input.Replace("{NS}", Namespace).Replace("{NAME}", TableName);
        }

        private void SavePrivate(IDbTransaction transaction)
        {
            if (IsReadOnly) return;
            UpdateStatus status = new UpdateStatus(this);
            if (!status.HaveAny) return;

            if (transaction == null)
            {
                /*
                 * According to Sqlite docs, it is thread safe, even when using the same connection
                 * on multiple threads. It's possible that that doesn't apply to the .NET adapter.
                 * 
                 * Without the lock, multiple threads enter here and one of them can crash when
                 * it tries transaction.Commit() with "no transaction is active on the connection."
                 * 
                 * Problem starts with BeginTransaction() which states that it "Creates a new 
                 * System.Data.SQLite.SQLiteTransaction if one isn't already active on the connection.
                 * 
                 * Since it doesn't always create a new transaction (but instead returns the existing one),
                 * the first finished thread can call transaction.Commit() and the next finished thread
                 * fails with the above message. Therefore the lock.
                 */
                lock (Controller.TransactionLockObject)
                {
                    using (transaction = Controller.Connection.BeginTransaction())
                    {
                        try
                        {
                            Insert(status, transaction);
                            Update(status, transaction);
                            Delete(status, transaction);
                            transaction.Commit();
                            AcceptChanges();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            RejectChanges();
                            throw;
                        }
                        finally
                        {
                        }
                    }
                }
            }
            // already a transaction. AcceptChanges or RejectChanges will be called by TransactionAdapter.
            else
            {
                Insert(status, transaction);
                Update(status, transaction);
                Delete(status, transaction);
            }
            changedEligibleColumns.Clear();
        }

        private void Insert(UpdateStatus status, IDbTransaction transaction)
        {
            if (!status.HaveInsert) return;
            Adapter.InsertCommand.Transaction = transaction as SQLiteTransaction;
            StringBuilder colList = new StringBuilder(512);
            StringBuilder sql = new StringBuilder(512);

            foreach (DataColumn col in Columns)
            {
                if (IsColumnEligible(col, DataColumnPropertyKey.ExcludeFromInsert))
                {
                    colList.Append($"{col.ColumnName},");
                }
            }
            // get rid of the last comma in the column list
            colList.Remove(colList.Length - 1, 1);       
            
            foreach (DataRow row in status.Insert)
            {
                Adapter.InsertCommand.Parameters.Clear();
                sql.Clear();
                sql.Append($"INSERT INTO {Namespace}.{TableName} ({colList}) VALUES(");

                foreach (DataColumn col in Columns)
                {
                    if (IsColumnEligible(col, DataColumnPropertyKey.ExcludeFromInsert))
                    {
                        sql.Append($":{col.ColumnName},");
                        Adapter.InsertCommand.Parameters.Add(col.ColumnName, NetTypeToDbType(col.DataType)).Value = row[col];
                    }
                }

                if (PrimaryKey.Length == 0)
                {
                }
                // get rid of the last comma in the values list
                sql.Remove(sql.Length - 1, 1);
                sql.Append(")");
                Adapter.InsertCommand.CommandText = sql.ToString();
                Adapter.InsertCommand.ExecuteNonQuery();
                InsertLastInsertId(row);
            }
        }

        private void Update(UpdateStatus status, IDbTransaction transaction)
        {
            if (!status.HaveUpdate) return;
            Adapter.UpdateCommand.Transaction = transaction as SQLiteTransaction;

            StringBuilder sql = new StringBuilder(512);
            foreach (DataRow row in status.Update)
            {
                Adapter.UpdateCommand.Parameters.Clear();
                sql.Clear();
                sql.Append($"UPDATE {Namespace}.{TableName} SET ");
                foreach (DataColumn col in Columns)
                {
                    if (IsColumnEligible(col, DataColumnPropertyKey.ExcludeFromUpdate))
                    {
                        sql.Append(string.Format("{0}=:{0},", col.ColumnName));
                        Adapter.UpdateCommand.Parameters.Add(col.ColumnName, NetTypeToDbType(col.DataType)).Value = row[col];
                    }
                }
                // get rid of the last comma
                sql.Remove(sql.Length - 1, 1);
                sql.Append($" WHERE {RowId}={row[RowIdAlias]}");
                Adapter.UpdateCommand.CommandText = sql.ToString();
                Adapter.UpdateCommand.ExecuteNonQuery();
            }
        }


        private bool HaveChangedEligibleColumns(DataRow row)
        {
            if (changedEligibleColumns.ContainsKey(row))
            {
                foreach (DataColumn col in Columns)
                {
                    if (changedEligibleColumns[row].Contains(col))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void Delete(UpdateStatus status, IDbTransaction transaction)
        {
            if (IsDeleteRestricted || !status.HaveDelete) return;
            Adapter.DeleteCommand.Transaction = transaction as SQLiteTransaction;
            StringBuilder sql = new StringBuilder(512);
            foreach (DataRow row in status.Delete)
            {
                Adapter.DeleteCommand.Parameters.Clear();
                sql.Clear();
                sql.Append($"DELETE FROM {Namespace}.{TableName} ");
                sql.Append($"WHERE {RowId}={row[RowIdAlias, DataRowVersion.Original]}");
                Adapter.DeleteCommand.CommandText = sql.ToString();
                Adapter.DeleteCommand.ExecuteNonQuery();
            }
        }

        private void InsertLastInsertId(DataRow row)
        {
            Adapter.InsertCommand.CommandText = "SELECT last_insert_rowid()";
            object id = Adapter.InsertCommand.ExecuteScalar();
            row[RowIdAlias] = id; 
            foreach (DataColumn col in Columns)
            {
                if (col.ExtendedProperties.ContainsKey(DataColumnPropertyKey.ReceiveInsertedId))
                {
                    row[col] = id;
                }
            }
        }

        /// <summary>
        /// Gets a boolean value that indicates if the specified column exists in this table.
        /// </summary>
        /// <param name="colName">The column name</param>
        /// <returns>true if exists; otherwise, false.</returns>
        private bool ColumnExists(string colName)
        {
            string sql = $"PRAGMA {Namespace}.table_info({TableName})";

            using (var reader = Controller.Execution.Query(sql))
            {
                int nameIndex = reader.GetOrdinal("name");
                while (reader.Read())
                {
                    if (reader.GetString(nameIndex).Equals(colName))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion

        /************************************************************************/

        #region Update Status (private class)
        /// <summary>
        /// Creates and exposes information about rows that need to be updated.
        /// </summary>
        private class UpdateStatus
        {
            /// <summary>
            /// Gets a list of DataRow objects to be inserted
            /// </summary>
            public List<DataRow> Insert { get; }

            /// <summary>
            /// Gets a list of DataRow objects that have been updated.
            /// </summary>
            public List<DataRow> Update { get; }

            /// <summary>
            /// Gets a list of DataRow objects to be deleted.
            /// </summary>
            public List<DataRow> Delete { get; }

            /// <summary>
            /// Gets a boolean value that indicates if there are records that should be inserted.
            /// </summary>
            public bool HaveInsert => Insert.Count > 0;

            /// <summary>
            /// Gets a boolean value that indicates iof there are records that should be updated.
            /// </summary>
            public bool HaveUpdate => Update.Count > 0;

            /// <summary>
            /// Gets a bollean value that indicates if there are records that should be deleted
            /// </summary>
            public bool HaveDelete => Delete.Count > 0;

            /// <summary>
            /// Gets a boolean value that indicates if any row needs updating for any reason (insert, update, or delete); otherwise, false.
            /// </summary>
            public bool HaveAny => HaveInsert || HaveUpdate || HaveDelete;

            public UpdateStatus(TableBase table)
            {
                Insert = table.Select(null, null, DataViewRowState.Added).ToList();
                Delete = table.Select(null, null, DataViewRowState.Deleted).ToList();
                Update = new List<DataRow>();
                DataRow[] update = table.Select(null, null, DataViewRowState.ModifiedOriginal);
                foreach (DataRow row in update)
                {
                    if (table.HaveChangedEligibleColumns(row))
                    {
                        Update.Add(row);
                    }
                }
            }
        }
        #endregion
    }
}
