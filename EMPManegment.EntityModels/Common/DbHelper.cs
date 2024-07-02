using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.EntityModels.Common
{
    public class DbHelper
    {
        public static DataTable GetDataTable(string cmdText, CommandType cmdType, SqlParameter[] parameters, string ConnectionString)
        {
            try
            {
                string conString = ConnectionString;
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(cmdText, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = cmdType;
                        if (parameters != null)
                        {
                            foreach (SqlParameter parameter in parameters)
                            {
                                if (null != parameter) cmd.Parameters.Add(parameter);
                            }
                        }

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            con.Close();
                            con.Dispose();
                            return dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        public static DataSet GetDataSet(string cmdText, CommandType cmdType, SqlParameter[] parameters, string ConnectionString)
        {
            try
            {
                //Set Connection string
                string conString = ConnectionString;
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(cmdText, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = cmdType;
                        if (parameters != null)
                        {
                            foreach (SqlParameter parameter in parameters)
                            {
                                if (null != parameter) cmd.Parameters.Add(parameter);
                            }
                        }


                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            da.Fill(ds);
                            con.Close();
                            return ds;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
