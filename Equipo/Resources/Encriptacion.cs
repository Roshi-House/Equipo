// Type: Encriptacion

using System;
using System.Security.Cryptography;
using System.Text;

internal class Encriptacion
{
  private string key;

  public Encriptacion(string sKey)
  {
    this.key = sKey;
  }

  public Encriptacion()
  {
    this.key = "/n0-s3.";
  }

  public string Encriptar(string sTextoEncriptar)
  {
    byte[] bytes = Encoding.UTF8.GetBytes(sTextoEncriptar);
    TripleDESCryptoServiceProvider cryptoServiceProvider = new TripleDESCryptoServiceProvider();
    byte[] hash = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(this.key));
    cryptoServiceProvider.Key = hash;
    cryptoServiceProvider.Mode = CipherMode.ECB;
    cryptoServiceProvider.Padding = PaddingMode.PKCS7;
    byte[] inArray = cryptoServiceProvider.CreateEncryptor().TransformFinalBlock(bytes, 0, bytes.Length);
    cryptoServiceProvider.Clear();
    return Convert.ToBase64String(inArray, 0, inArray.Length);
  }

  public string Desencriptar(string sTextoEncriptado)
  {
    byte[] bytes1 = Convert.FromBase64String("");
    try
    {
      byte[] inputBuffer = Convert.FromBase64String(sTextoEncriptado);
      MD5CryptoServiceProvider cryptoServiceProvider1 = new MD5CryptoServiceProvider();
      byte[] hash = cryptoServiceProvider1.ComputeHash(Encoding.UTF8.GetBytes(this.key));
      cryptoServiceProvider1.Clear();
      TripleDESCryptoServiceProvider cryptoServiceProvider2 = new TripleDESCryptoServiceProvider();
      cryptoServiceProvider2.Key = hash;
      cryptoServiceProvider2.Mode = CipherMode.ECB;
      cryptoServiceProvider2.Padding = PaddingMode.PKCS7;
      byte[] bytes2 = cryptoServiceProvider2.CreateDecryptor().TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);
      cryptoServiceProvider2.Clear();
      return Encoding.UTF8.GetString(bytes2);
    }
    catch (Exception ex)
    {
      return Encoding.UTF8.GetString(bytes1);
    }
  }
}
