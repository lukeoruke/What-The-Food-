using Microsoft.Data.SqlClient;
using TeamNoStress.WhatTheFood.Domain;


namespace TeamNoStress.WhatTheFood.DataAccessLayer
{
    public class SQLDataAccessObject
    {
        public void GetAllUserHistory(string username)
        {
            var connString = "Server={LocalHost}; Port=3306; Database={your_database}; Uid={root}; Pwd={WhatTheFood}; SslMode=Preferred"; //only used when referencing a local machine. windows only 3:17:15
            using (var conn = new SqlConnection(connString))
            {
                using (var adapter = new SqlDataAdapter())
                {
                    var sql = "Select * FROM UserInfo";// this takes in an actual SQL result and updates it
                    using (var command = new SqlCommand(sql,conn))
                    {
                        //command.ExecuteReader(); //getting multiple data
                        //command.ExecuteNonQuery(); // not getting any data back
                        //command.ExecuteScalar(); // getting a single value
            
                    }
                }
            }
        }
    }
}//use ORM
