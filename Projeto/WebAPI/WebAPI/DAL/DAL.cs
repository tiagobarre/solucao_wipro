using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.DAL
{
    public class DAL
    {
        private static string Server = "localhost";
        private static string DataBase = "projeto_wipro";
        private static string User = "root";
        private static string Password = "Fadami@12";
        public string redirecionar { get; set; }


        private string db = $"Server={Server};DataBase={DataBase};Uid={User};Pwd={Password}";

        private MySqlConnection connection;

        public DAL() // Construtor da classe
        {
            connection = new MySqlConnection(db);
            try
            {
                connection.Open();
                connection.Close();
            }
            catch (MySqlException ex)
            {

                redirecionar = $"{ex.Message}";

            }



        }

        // Retornar Dados do Banco
        public DataTable RetDataTable(string sql)
        {
            DataTable Dados = new DataTable();

            try
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(sql, connection);
                MySqlDataAdapter DataAdapter = new MySqlDataAdapter(command);
                DataAdapter.Fill(Dados);
                connection.Close();
            }
            catch (MySqlException ex)
            {
                redirecionar = $"{ex.Message}";
            }



            return Dados;

        }

        // Executa INSERT,UPDATE,DELETE
        public void ExecutarComandoSQL(string sql)
        {
            try
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(sql, connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (MySqlException ex)
            {
                redirecionar = $"{ex.Message}";
            }

        }

    }
}
