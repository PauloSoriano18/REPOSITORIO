using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;


namespace MiLibreria
{
    public class Utilidades
    {

        public static DataSet Ejecutar(string cmd)
        {
            DataSet DS;
            MySqlDataAdapter DA;
            MySqlConnection conexion = new MySqlConnection("server=localhost; database=ejemplo1; Uid=root; pwd=159357; SslMode=none;");
            conexion.Open();

            DS = new DataSet();
            DA = new MySqlDataAdapter(cmd, conexion);

            DA.Fill(DS);

            conexion.Close();

            return DS;


        }
    }
}
