using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Weighplatation.Model;
using Weighplatation.Interface;
using System.Configuration;
using Npgsql;
using System.Data;
using System.Globalization;

namespace Weighplatation.Repository
{
    public class PortRepo : IPort
    {
        private readonly string stringCon = ConfigurationManager.ConnectionStrings["dbcon"].ToString();

   
        public PortModel GetPort(string WBCode)
        {
            PortModel _PortModel = new PortModel();

            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection())
                {

                    con.ConnectionString = stringCon;
                    con.Open();

                    string strSql = @"select * from public.""WBCONFIG"" where ""WBSOURCE""='" + WBCode + "'";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(strSql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            reader.Read();
                            _PortModel.WBSOURCE = (string)reader["WBSOURCE"];
                            _PortModel.Description = (string)reader["Description"];
                            _PortModel.Bautrate = double.Parse(reader["Bautrate"].ToString());
                            _PortModel.ComPort = (string)reader["ComPort"].ToString();
                            _PortModel.DataBits = double.Parse(reader["DataBits"].ToString());
                            _PortModel.StopBits = double.Parse(reader["StopBits"].ToString());
                            _PortModel.Parity = (string)reader["Parity"];

                        }
                        cmd.Dispose();
                    }

                    con.Close();

                    return _PortModel;
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                throw;
            }
        }
    }
}