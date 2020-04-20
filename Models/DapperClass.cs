using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Eksa_Api.Models
{
    public class DapperClass
    {
        public IEnumerable<Models.User> SearchUser()
        {
            IEnumerable<Models.User> model;
            using (var sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["EksaContext"].ToString()))
            {
                model = sqlConnection.Query<Models.User>("SELECT * FROM USERS");
                sqlConnection.Close();
            }
            return model;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public string AddUser([FromUri] User User)
        {
            using (var sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["EksaContext"].ToString()))
            {
                try
                {
                    var variables = new DynamicParameters();
                    variables.Add("@userIdentityNumber", User.userIdentityNumber);
                    variables.Add("@userPersonalName", User.userPersonalName);
                    variables.Add("@userPersonalSurname", User.userPersonalSurname);
                    variables.Add("@userPhoneNumber", User.userPhoneNumber);
                    variables.Add("@userEmail", User.userEmail);
                    variables.Add("@userBirthDate", User.userBirthDate);
                    variables.Add("@userPassword", User.userPassword);
                    variables.Add("@userImageSource", User.userImageSource);
                    variables.Add("@userRol", User.userRol);

                    sqlConnection.Execute("spAddUser", variables, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();
                    return "Success";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }
    }
}