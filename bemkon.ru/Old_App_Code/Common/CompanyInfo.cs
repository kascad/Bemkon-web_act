using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using ProfessorTesting.Old_App_Code.Connection;

namespace ProfessorTesting.Old_App_Code.Common
{

    /// <summary>
    /// Представляет информацию о текущей компании.
    /// </summary>
    /// 
    public class CompanyInfo
    {
        private DataRow info;


        private int _id;
        public int Id
        {
            get { return _id; }
        }
        private string _companyname;
        public string CompanyName
        {
            get { return _companyname; }
            set { _companyname = value; }
        }
        
        public CompanyInfo(DataRow info)
        {
            this._id = Core.Converting.ConvertToInt(info[CompanyDB.Company.ID]);
            this._companyname = info[CompanyDB.Company.CompanyName].ToString();

        }
    }
}