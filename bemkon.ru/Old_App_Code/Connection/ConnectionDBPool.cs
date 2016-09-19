using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

/// <summary>
/// Представляет собой соединение с базой данных.
/// </summary>
internal class ConnectionDBPool
    :IDisposable
{
    #region Переменные и свойства

    private ErrorHandler error;
    public ErrorHandler Error
    {
        get { return error; }
    }
    /// <summary>
    /// Соединение с базой.
    /// </summary>
    private SqlConnection connection;
    public bool IsOpenFree
    {
        get
        {
            return connection != null
                && connection.State == ConnectionState.Open
                && connection.State != ConnectionState.Executing
                && connection.State != ConnectionState.Fetching;
        }
    }

    #endregion

    #region Конструкторы

    public ConnectionDBPool()
    {
        this.error = new ErrorHandler();
        Connect();
    }

    #endregion

    #region Методы

    public bool Connect()
    {
        Disconnect();

        error.ResetError();

        try
        {
            connection = new SqlConnection();
            connection.ConnectionString = Core.Site.connectionString;
            connection.Open();
        }
        catch (Exception ex)
        {
            if (ex.Message.IndexOf("#28000") >= 0)
                error.SetError("Access denied for user");
            else
                error.SetError(ex.Message);
            return false;
        }

        return true;
    }
    public object ExecuteScalar(string nameProc, List<SqlParam> par)
    {
        error.ResetError();

        SqlCommand cmd = new SqlCommand(nameProc);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Connection = connection;
        if (par.Count > 0)
        {
            foreach (SqlParam pp in par)
            {
                cmd.Parameters.Add(pp.name, pp.ConvertType());
                cmd.Parameters[pp.name].Value = pp.value;
                cmd.Parameters[pp.name].Direction = pp.ConvertDirection();
            }
        }
        try { return cmd.ExecuteScalar(); }
        catch (Exception err)
        {
            error.SetError(err.Message);
            return null;
        }
    }
    public void ExecuteNoResult(string nameProc, List<SqlParam> par)
    {
        error.ResetError();

        SqlCommand cmd = new SqlCommand(nameProc);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Connection = connection;
        if (par != null && par.Count > 0)
        {
            foreach (SqlParam pp in par)
            {
                cmd.Parameters.Add(pp.name, pp.ConvertType());
                cmd.Parameters[pp.name].Value = pp.value;
                cmd.Parameters[pp.name].Direction = pp.ConvertDirection();
            }
        }

        try { cmd.ExecuteNonQuery(); }
        catch (Exception err)
        {
            error.SetError(err.Message);
        }
    }

    public DataTable ExecuteReader(string nameProc, List<SqlParam> par)
    {
        error.ResetError();

        SqlCommand cmd = new SqlCommand(nameProc);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Connection = connection;
        if (par != null && par.Count > 0)
        {
            foreach (SqlParam pp in par)
            {
                cmd.Parameters.Add(pp.name, pp.ConvertType());
                cmd.Parameters[pp.name].Value = pp.value;
                cmd.Parameters[pp.name].Direction = pp.ConvertDirection();
            }
        }

        DataTable table = null;
        try
        {
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            table = new DataTable();
            adapter.Fill(table);
        }
        catch (Exception err)
        {
            error.SetError(err.Message);
            return table;
        }
        return table;
    }
    public void ExecuteNoResult(string query)
    {
        error.ResetError();

        SqlCommand cmd = InitCommand(query);
        if (cmd == null)
        {
            if (!error.IsError)
                error.SetError(ErrorHandler.unknownError);
            return;
        }

        try { cmd.ExecuteNonQuery(); }
        catch (Exception err) { error.SetError(err.Message); }
    }
    public object ExecuteScalar(string query)
    {
        error.ResetError();

        SqlCommand cmd = InitCommand(query);
        if (cmd == null)
        {
            if (!error.IsError)
                error.SetError(ErrorHandler.unknownError);
            return null;
        }
        try { return cmd.ExecuteScalar(); }
        catch (Exception err)
        {
            error.SetError(err.Message);
            return null;
        }
    }
    public DataTable ExecuteReader(string query)
    {
        error.ResetError();
        SqlCommand cmd = InitCommand(query);
        if (cmd == null)
        {
            if (!error.IsError)
                error.SetError(ErrorHandler.unknownError);
            return null;
        }
        DataTable table = null;
        try
        {
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            table = new DataTable();
            adapter.Fill(table);
        }
        catch (Exception err)
        {
            error.SetError(err.Message);
            return table;
        }
        return table;
    }
    private void CloseReader(SqlDataReader reader)
    {
        if (reader != null)
            reader.Close();
    }
    private SqlCommand InitCommand(string query)
    {
        if (!ValidConnection())
            return null;

        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = query;
        cmd.Connection = connection;

        return cmd;
    }
    private bool ValidConnection()
    {
        if (connection == null || connection.State == ConnectionState.Closed)
            Connect();

        if (error.IsError)
            return false;
        return true;
    }
    public void Disconnect()
    {
        error.ResetError();

        if (connection == null)
            return;

        try { connection.Close(); }
        catch (Exception err) { error.SetError(err.Message); }

        connection = null;
    }

    public SqlCommand CreateCommand()
    {
        return connection.CreateCommand();
    }

    #endregion

    #region IDisposable Members

    public void Dispose()
    {
        Disconnect();
    }

    #endregion
}