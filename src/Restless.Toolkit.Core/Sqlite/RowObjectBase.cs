using Restless.Toolkit.Core.Resources;
using System;
using System.Data;

namespace Restless.Toolkit.Core.Database.SQLite
{
    /// <summary>
    /// Represents an object that encapsulate a single row. This class must be inherited.
    /// </summary>
    /// <typeparam name="T">The table type to which the row belongs</typeparam>
    public abstract class RowObjectBase<T>  where T : TableBase
    {
        #region Public properties
        /// <summary>
        /// Gets the data row that is the underlying basis for this object.
        /// </summary>
        public DataRow Row
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the table that is the underlying basis for this object.
        /// </summary>
        public T Table
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets a value that indicates if this row object is selected.
        /// </summary>
        /// <remarks>
        /// This property is not used by the base class. It is provided as a convienance
        /// property for use in data binding situations.
        /// </remarks>
        public bool IsSelected
        {
            get;
            set;
        }
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="RowObjectBase{T}"/> class.
        /// </summary>
        /// <param name="row">The data row</param>
        protected RowObjectBase(DataRow row)
        {
            _ = row ?? throw new ArgumentNullException(nameof(row));

            if (row.Table.GetType() != typeof(T))
            {
                throw new InvalidOperationException(Strings.InvalidOperationDataRowTableMismatch);
            }
            Table = (T)row.Table;
            Row = row;
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Saves the table that is associated with this row object.
        /// </summary>
        /// <remarks>
        /// Override this method if you need to perform other processing before the save.
        /// Either call Table.Save() from the overridden method after custom processing, or
        /// call the base method.
        /// </remarks>
        public virtual void Save()
        {
            Table.Save();
        }

        /// <summary>
        /// Accepts changes to the underlying row.
        /// Override if you need to perform others operations.
        /// Always call the base method,
        /// </summary>
        public virtual void AcceptChanges()
        {
            Row.AcceptChanges();
        }

        /// <summary>
        /// Rejects changes to the underlying row.
        /// Override if you need to perform others operations.
        /// Always call the base method,
        /// </summary>
        public virtual void RejectChanges()
        {
            Row.RejectChanges();
        }
        #endregion

        /************************************************************************/

        #region Protected methods
        /// <summary>
        /// Gets an Int64 value from the specified column.
        /// </summary>
        /// <param name="colName">The column name.</param>
        /// <returns>The Int64 value.</returns>
        protected long GetInt64(string colName)
        {
            return Row[colName] != DBNull.Value ? (long)Row[colName] : 0;
        }

        /// <summary>
        /// Gets a nullable Int64 value from the specified column.
        /// </summary>
        /// <param name="colName">The column name.</param>
        /// <returns>The Int64 value, or null.</returns>
        protected long? GetNullableInt64(string colName)
        {
            return Row[colName] is long value ? value : (long?)null;
        }

        /// <summary>
        /// Gets a Decimal value from the specified column.
        /// </summary>
        /// <param name="colName">The column name.</param>
        /// <returns>The Decimal value.</returns>
        protected decimal GetDecimal(string colName)
        {
            return Row[colName] != DBNull.Value ? (decimal)Row[colName] : 0;
        }

        /// <summary>
        /// Gets a string value from the specified column.
        /// </summary>
        /// <param name="colName">The column name.</param>
        /// <returns>The string value.</returns>
        protected string GetString(string colName)
        {
            return Row[colName].ToString();
        }

        /// <summary>
        /// Gets a DateTime value from the specified column.
        /// </summary>
        /// <param name="colName">The column name.</param>
        /// <returns>The DateTime value.</returns>
        protected DateTime GetDateTime(string colName)
        {
            return Row[colName] != DBNull.Value ? (DateTime)Row[colName] : DateTime.MinValue;
        }

        /// <summary>
        /// Gets a nullable DateTime value from the specified column.
        /// </summary>
        /// <param name="colName">The column name.</param>
        /// <returns>The DateTime value, or null.</returns>
        protected DateTime? GetNullableDateTime(string colName)
        {
            return Row[colName] is DateTime time ? (DateTime?)time : null;
        }

        /// <summary>
        /// Gets a Boolean value from the specified column.
        /// </summary>
        /// <param name="colName">The column name.</param>
        /// <returns>The Boolean value.</returns>
        protected bool GetBoolean(string colName)
        {
            return Row[colName] != DBNull.Value && (bool)Row[colName];
        }

        /// <summary>
        /// Gets a byte array from the specified column
        /// </summary>
        /// <param name="colName">The column name</param>
        /// <returns></returns>
        protected byte[] GetByteArray(string colName)
        {
            return (byte[])Row[colName];
        }

        /// <summary>
        /// Sets an Int64 value on the specified column.
        /// </summary>
        /// <param name="colName">The column name.</param>
        /// <param name="value">The value.</param>
        /// <returns>true if the column value was set; false if the column value was already the same as <paramref name="value"/>.</returns>
        protected bool SetValue(string colName, long value)
        {
            if (!Row[colName].Equals(value))
            {
                Row[colName] = value;
                OnSetValue(colName, value);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Sets an nullable Int64 value on the specified column.
        /// </summary>
        /// <param name="colName">The column name.</param>
        /// <param name="value">The value.</param>
        /// <returns>true if the column value was set; false if the column value was already the same as <paramref name="value"/>.</returns>
        protected bool SetValue(string colName, long? value)
        {
            object rowValue = value;
            if (value == null)
            {
                rowValue = DBNull.Value;
            }

            if (!Row[colName].Equals(rowValue))
            {
                Row[colName] = rowValue;
                OnSetValue(colName, value);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Sets a Decimal value on the specified column.
        /// </summary>
        /// <param name="colName">The column name.</param>
        /// <param name="value">The value.</param>
        /// <returns>true if the column value was set; false if the column value was already the same as <paramref name="value"/>.</returns>
        protected bool SetValue(string colName, decimal value)
        {
            if (!Row[colName].Equals(value))
            {
                Row[colName] = value;
                OnSetValue(colName, value);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Sets a nullable Decimal value on the specified column.
        /// </summary>
        /// <param name="colName">The column name.</param>
        /// <param name="value">The value.</param>
        /// <returns>true if the column value was set; false if the column value was already the same as <paramref name="value"/>.</returns>
        protected bool SetValue(string colName, decimal? value)
        {
            object rowValue = value;
            if (value == null)
            {
                rowValue = DBNull.Value;
            }

            if (!Row[colName].Equals(rowValue))
            {
                Row[colName] = rowValue;
                OnSetValue(colName, value);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Sets a string value on the specified column.
        /// </summary>
        /// <param name="colName">The column name.</param>
        /// <param name="value">The value.</param>
        /// <returns>true if the column value was set; false if the column value was already the same as <paramref name="value"/>.</returns>
        protected bool SetValue(string colName, string value)
        {
            object rowValue = value;
            if (string.IsNullOrWhiteSpace(value))
            {
                rowValue = DBNull.Value;
            }

            if (!Row[colName].Equals(rowValue))
            {
                Row[colName] = value;
                OnSetValue(colName, value);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Sets a DateTime value on the specified column.
        /// </summary>
        /// <param name="colName">The column name.</param>
        /// <param name="value">The value.</param>
        /// <returns>true if the column value was set; false if the column value was already the same as <paramref name="value"/>.</returns>
        protected bool SetValue(string colName, DateTime value)
        {
            if (!Row[colName].Equals(value))
            {
                Row[colName] = value;
                OnSetValue(colName, value);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Sets a nullable DateTime value on the specified column.
        /// </summary>
        /// <param name="colName">The column name.</param>
        /// <param name="value">The value.</param>
        /// <returns>true if the column value was set; false if the column value was already the same as <paramref name="value"/>.</returns>
        protected bool SetValue(string colName, DateTime? value)
        {
            object rowValue = value;
            if (value == null)
            {
                rowValue = DBNull.Value;
            }

            if (!Row[colName].Equals(rowValue))
            {
                Row[colName] = rowValue;
                OnSetValue(colName, value);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Sets a Boolean value on the specified column.
        /// </summary>
        /// <param name="colName">The column name.</param>
        /// <param name="value">The value.</param>
        /// <returns>true if the column value was set; false if the column value was already the same as <paramref name="value"/>.</returns>
        protected bool SetValue(string colName, bool value)
        {
            if (!Row[colName].Equals(value))
            {
                Row[colName] = value;
                OnSetValue(colName, value);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Called when any of the SetValue() overloads successfully set a value, i.e. change the value from what it was.
        /// Override in a derived class if needed. The base implementation does nothing.
        /// </summary>
        /// <param name="columnName">The column name.</param>
        /// <param name="value">The value</param>
        protected virtual void OnSetValue(string columnName, object value)
        {
        }
        #endregion
    }
}
