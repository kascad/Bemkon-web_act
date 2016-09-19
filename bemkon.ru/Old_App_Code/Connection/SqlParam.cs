using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

/// <summary>
/// Представляет параметр хранимой процедуры.
/// </summary>
public class SqlParam
{
    public string name;
    public Type type;
    public object value;
    public int dir;

	public SqlParam(string name, Type type, object value)
        : this(name, type, value, 1)
	{
	}

	public SqlParam(string name, Type type, object value, int dir)
	{
        this.name = name;
        this.type = type;
        this.value = value;
        this.dir = dir;
	}
    public SqlDbType ConvertType()
    {
        if (type == typeof(string))
            return SqlDbType.VarChar;
        if (type == typeof(int))
            return SqlDbType.Int;
        return SqlDbType.Variant;
    }
    public ParameterDirection ConvertDirection()
    {
        if (Enum.IsDefined(typeof(ParameterDirection), dir))
            return (ParameterDirection)dir;
        return ParameterDirection.Input;
    }
}
