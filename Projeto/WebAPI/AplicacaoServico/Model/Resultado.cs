using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AplicacaoServico.Model
{
    public class Resultado
    {
        public string horario { get; set; }

         private static string _URL_PROJETO = ConfigurationManager.AppSettings["URL_PROJETO"];

        public void GerarArquivo()
        {

            string caminho = $"{_URL_PROJETO}\\Resultado";

            string data = DateTime.Now.ToString("yyyyMMdd");
            string hora = DateTime.Now.ToString("HHmmss");

            horario = hora;

            var diretorio = Path.Combine(caminho, $"Resultado_{data}_{horario}.csv");

            using (StreamWriter file = new StreamWriter(diretorio, true))
            {
                file.WriteLine($"Moeda;Data_Referencia;VL_COTACAO;Data_cotacao");
                file.Close();
            }

        }

        public void GravarResultado(string ID_MOEDA, string DATA_REF, string vlr_cotacao, string dat_cotacao )
        {
            string caminho = $"{_URL_PROJETO}\\Resultado";

            string data = DateTime.Now.ToString("yyyyMMdd");
            string hora = DateTime.Now.ToString("HHmmss");

            var diretorio = Path.Combine(caminho, $"Resultado_{data}_{horario}.csv");

            using (StreamWriter file = new StreamWriter(diretorio, true))
            {
                
                file.WriteLine($"{ID_MOEDA};{DATA_REF};{vlr_cotacao};{dat_cotacao}");
                file.Close();
            }

        }
    }
}
