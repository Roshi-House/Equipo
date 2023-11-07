using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Equipo.Conexion
{
    class Conexion
    {
        MySqlConnection Conex;
        string cadenaconex;

        public MySqlConnection Conectar()
        {
            cadenaconex = "server=127.0.0.1; Database=equipos; uid=root; pwd=; pooling = false; convert zero datetime = true";
            Conex = new MySqlConnection(cadenaconex);
            return Conex;
        }

    }
}
