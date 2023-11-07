// Type: ConexionMySQL

using MySql.Data.MySqlClient;
using System;
using System.Diagnostics;
using System.IO;

public class ConexionMySQL
{
  private int tiempo_conexion = 480;
  private bool conectado = false;
  private string sql = "";
  private MySqlConnection conexion;
  private string servidor;
  private string base_datos;
  private string usuario;
  private string contrasenia;
  private string cadenaConexion;

  public string Servidor
  {
    get
    {
      return this.servidor;
    }
    set
    {
      this.servidor = value;
      this.cadenaConexion = (string) null;
      this.Desconectar();
    }
  }

  public string BD
  {
    get
    {
      return this.base_datos;
    }
    set
    {
      this.base_datos = value;
      this.cadenaConexion = (string) null;
      this.Desconectar();
    }
  }

  public string Usuario
  {
    get
    {
      return this.usuario;
    }
    set
    {
      this.usuario = value;
      this.cadenaConexion = (string) null;
      this.Desconectar();
    }
  }

  public string Contrasenia
  {
    get
    {
      return this.contrasenia;
    }
    set
    {
      this.contrasenia = value;
      this.cadenaConexion = (string) null;
      this.Desconectar();
    }
  }

  public int TiempoConexion
  {
    get
    {
      return this.tiempo_conexion;
    }
    set
    {
      this.tiempo_conexion = value;
      this.cadenaConexion = (string) null;
      this.Desconectar();
    }
  }

  public string CadenaConexion
  {
    get
    {
      return this.cadenaConexion;
    }
  }

  public bool Conectado
  {
    get
    {
      return this.conectado;
    }
  }

  public string SQL
  {
    get
    {
      return this.sql;
    }
    set
    {
      this.sql = value;
    }
  }

  public ConexionMySQL()
  {
  }

  public ConexionMySQL(string servidor, string base_datos, string usuario, string contrasenia)
  {
    this.inicializarConexion(servidor, base_datos, usuario, contrasenia, this.tiempo_conexion);
  }

  private void inicializarConexion(
    string servidor,
    string base_datos,
    string usuario,
    string contrasenia,
    int tiempo_conexion)
  {
    this.servidor = servidor;
    this.base_datos = base_datos;
    this.usuario = usuario;
    this.contrasenia = contrasenia;
    this.tiempo_conexion = tiempo_conexion;
    this.cadenaConexion = "server=" + this.servidor + ";database=" + this.base_datos + ";uid=" + this.usuario + ";pwd=" + this.contrasenia + ";Connection Timeout=" + this.tiempo_conexion.ToString() + ";";
    this.conexion = new MySqlConnection(this.cadenaConexion);
  }

  private void inicializarConexion()
  {
    this.cadenaConexion = "server=" + this.servidor + ";database=" + this.base_datos + ";uid=" + this.usuario + ";pwd=" + this.contrasenia + ";Connection Timeout=" + this.tiempo_conexion.ToString() + ";";
    this.conexion = new MySqlConnection(this.cadenaConexion);
  }

  public bool Conectar()
  {
    if (this.conectado)
      return this.conectado;
    if (this.cadenaConexion == null)
      this.inicializarConexion();
    try
    {
      Console.Write("Intentando enlace MySQL Server en " + this.usuario + "@" + this.Servidor + "...");
      this.conexion.Open();
      this.conectado = true;
      Console.WriteLine(" conectado!");
    }
    catch (Exception ex)
    {
      Debug.WriteLine(ex.Message);
      throw new Exception(ex.Message);
    }
    return this.conectado;
  }

  public void Desconectar()
  {
    if (!this.conectado)
      return;
    try
    {
      this.conexion.Close();
      this.conectado = false;
    }
    catch (MySqlException ex)
    {
      Debug.WriteLine(ex.Message);
      throw new Exception(ex.Message);
    }
  }

  public bool EjecutarNoResultado()
  {
    bool flag = false;
    try
    {
      if (this.sql != "")
      {
        if (!this.conectado)
          this.Conectar();
        if (this.conectado)
        {
          using (MySqlCommand mySqlCommand = new MySqlCommand(this.sql, this.conexion))
            mySqlCommand.ExecuteNonQuery();
          flag = true;
        }
      }
    }
    catch (MySqlException ex)
    {
      Debug.WriteLine(ex.Message);
      throw new Exception(ex.Message);
    }
    return flag;
  }

  public MySqlDataReader EjecutarConResultado()
  {
    MySqlDataReader mySqlDataReader = (MySqlDataReader) null;
    try
    {
      if (this.sql != "")
      {
        if (!this.conectado)
          this.Conectar();
        if (this.conectado)
          mySqlDataReader = new MySqlCommand(this.sql, this.conexion).ExecuteReader();
      }
    }
    catch (MySqlException ex)
    {
      Debug.WriteLine(ex.Message);
      throw new Exception(ex.Message);
    }
    return mySqlDataReader;
  }

  public MySqlDataReader obtenerResultado(MySqlDataReader dataReaders)
  {
    MySqlDataReader mySqlDataReader = (MySqlDataReader) null;
    if (dataReaders.Read())
      mySqlDataReader = dataReaders;
    return mySqlDataReader;
  }

  public void Respaldar()
  {
    try
    {
      DateTime now = DateTime.Now;
      StreamWriter streamWriter = new StreamWriter("C:\\" + (object) now.Year + (object) now.Month + (object) now.Day + "_" + (object) now.Hour + (object) now.Minute + (object) now.Second + (object) now.Millisecond + ".sql");
      Process process = Process.Start(new ProcessStartInfo()
      {
        FileName = "mysqldump",
        RedirectStandardInput = false,
        RedirectStandardOutput = true,
        Arguments = string.Format("-u{0} -p{1} -h{2} {3}", (object) this.usuario, (object) this.contrasenia, (object) this.servidor, (object) this.base_datos),
        UseShellExecute = false
      });
      string end = process.StandardOutput.ReadToEnd();
      streamWriter.WriteLine(end);
      process.WaitForExit();
      streamWriter.Close();
      process.Close();
    }
    catch (IOException ex)
    {
      Debug.WriteLine(ex.Message);
      throw new Exception(ex.Message);
    }
  }

  public void Restaurar()
  {
    try
    {
      StreamReader streamReader = new StreamReader("C:\\MySqlBackup.sql");
      string end = streamReader.ReadToEnd();
      streamReader.Close();
      Process process = Process.Start(new ProcessStartInfo()
      {
        FileName = "mysql",
        RedirectStandardInput = true,
        RedirectStandardOutput = false,
        Arguments = string.Format("-u{0} -p{1} -h{2} {3}", (object) this.usuario, (object) this.contrasenia, (object) this.servidor, (object) this.base_datos),
        UseShellExecute = false
      });
      process.StandardInput.WriteLine(end);
      process.StandardInput.Close();
      process.WaitForExit();
      process.Close();
    }
    catch (IOException ex)
    {
      Debug.WriteLine(ex.Message);
      throw new Exception(ex.Message);
    }
  }
}
