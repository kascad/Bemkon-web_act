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
/// Представляет привилегии пользователя.
/// </summary>
public enum UserPrivGroup
{
    None,
    Administrator,
    User,
    ProUser,
    TestUser,
    InterpetUser,
    Operator
}
