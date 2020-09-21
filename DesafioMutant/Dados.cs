using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Json;
using Newtonsoft.Json.Linq;
using DesafioMutant.UserData;
using System.Linq;

namespace DesafioMutant
{
    public class Dados
    {
        /// <summary>
        /// Método para retornar o conteúdo da API
        /// </summary>
        /// <param name="url">URL da API</param>
        /// <returns></returns>
        public static string BaixarDados(string url) 
        {
            WriteLog("Baixar Dados");
            string text = string.Empty;

            try
            {
                WriteLog("Criando request.");
                HttpWebRequest requesto = (HttpWebRequest)WebRequest.Create(url);
                requesto.AutomaticDecompression = DecompressionMethods.GZip;

                using (HttpWebResponse responso = (HttpWebResponse)requesto.GetResponse())
                using (Stream streamo = responso.GetResponseStream())
                using (StreamReader reader = new StreamReader(streamo))
                {
                    WriteLog("Lendo resposta da api.");
                    text = reader.ReadToEnd();
                }
            }
            catch(Exception ex)
            {
                WriteLog(ex.Message);
            }

            return text;
        }

        /// <summary>
        /// Método para salvar os dados dos usuários no banco
        /// </summary>
        /// <param name="list">Lista dos usuários a serem salvos</param>
        public static int SalvarDados(string list)
        {
            WriteLog("Salvar Dados");
            int rowsAffected = 0;

            string connString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\felip\\source\\repos\\DesafioMutant\\DesafioMutant\\dbMutant.mdf;Integrated Security=True";
            List<User> listJSON = new List<User>();
            List<User> sortedList = new List<User>();

            try
            {
                WriteLog("Serializando resposta da api para JSON.");
                listJSON = JsonConvert.DeserializeObject<List<User>>(list);
                
                //Organizando a lista em ordem alfabetica.
                sortedList = listJSON.OrderBy(x => x.Name).ToList();
            }
            catch(Exception ex)
            {
                WriteLog(ex.Message);
            }

            string query = "INSERT INTO UserPersonalInfo (UserId, Name, Username) VALUES (@UserId, @Name, @Username)\n" +
                "INSERT INTO UserAddress (UserId, Street, Suite, City, Zipcode, GeoLatLng) VALUES (@UserId, @Street, @Suite, @City, @Zipcode, @GeoLatLng)\n" +
                "INSERT INTO UserContactInfo(UserId, Email, Phone, Website, CompanyName, CompanyCatchphrase, CompanyBS) VALUES (@UserId, @Email, @Phone, @Website, @CompanyName, @CompanyCatchphrase, @CompanyBS)";

            foreach (var user in sortedList)
            {
                //Verifica se o usuário está hospedado em apartamento
                if (!user.Address.Suite.Contains("Apt."))
                {
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        SqlCommand command = new SqlCommand(query, conn);

                        command.Parameters.Add("@UserId", System.Data.SqlDbType.Int);
                        command.Parameters["@UserId"].Value = user.Id;

                        command.Parameters.Add("@Name", System.Data.SqlDbType.Text);
                        command.Parameters["@Name"].Value = user.Name;

                        command.Parameters.Add("@Username", System.Data.SqlDbType.Text);
                        command.Parameters["@Username"].Value = user.Username;

                        command.Parameters.Add("@Street", System.Data.SqlDbType.Text);
                        command.Parameters["@Street"].Value = user.Address.Street;

                        command.Parameters.Add("@Suite", System.Data.SqlDbType.Text);
                        command.Parameters["@Suite"].Value = user.Address.Suite;

                        command.Parameters.Add("@City", System.Data.SqlDbType.Text);
                        command.Parameters["@City"].Value = user.Address.City;

                        command.Parameters.Add("@Zipcode", System.Data.SqlDbType.Text);
                        command.Parameters["@Zipcode"].Value = user.Address.Zipcode;

                        command.Parameters.Add("@GeoLatLng", System.Data.SqlDbType.Text);
                        command.Parameters["@GeoLatLng"].Value = user.Address.Geo.Lat + ';' + user.Address.Geo.Lng;

                        command.Parameters.Add("@Email", System.Data.SqlDbType.Text);
                        command.Parameters["@Email"].Value = user.Email;

                        command.Parameters.Add("@Phone", System.Data.SqlDbType.Text);
                        command.Parameters["@Phone"].Value = user.Phone;

                        command.Parameters.Add("@Website", System.Data.SqlDbType.Text);
                        command.Parameters["@Website"].Value = user.Website;

                        command.Parameters.Add("@CompanyName", System.Data.SqlDbType.Text);
                        command.Parameters["@CompanyName"].Value = user.Company.Name;

                        command.Parameters.Add("@CompanyCatchphrase", System.Data.SqlDbType.Text);
                        command.Parameters["@CompanyCatchphrase"].Value = user.Company.CatchPhrase;

                        command.Parameters.Add("@CompanyBS", System.Data.SqlDbType.Text);
                        command.Parameters["@CompanyBS"].Value = user.Company.BS;

                        try
                        {
                            conn.Open();
                            rowsAffected = command.ExecuteNonQuery();
                            WriteLog(rowsAffected.ToString() + " linha(s) afetada(s)");
                            Console.WriteLine(rowsAffected.ToString() + " linha(s) afetada(s)");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message.ToString());
                            WriteLog(ex.Message);
                        }
                    }
                }
            }

            return rowsAffected;
        }

        /// <summary>
        /// Método de criação de logs
        /// </summary>
        /// <param name="logText">Texto do log</param>
        public static void WriteLog(string logText)
        {
            //Seta o caminho e o nome do arquivo de log
            string _logpath = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory);
            _logpath = Directory.GetParent(Directory.GetParent(Directory.GetParent(_logpath).FullName).FullName).FullName + "\\Logs\\";
            string filename = "log " + DateTime.Now.Day.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString() + ".txt";
            TextWriter textWriter = new StreamWriter(_logpath + filename, true);

            logText = DateTime.Now.ToString() + " - " + logText + "\n";

            textWriter.Write(logText);
            textWriter.Flush();
            textWriter.Close();


        }
    }
}
