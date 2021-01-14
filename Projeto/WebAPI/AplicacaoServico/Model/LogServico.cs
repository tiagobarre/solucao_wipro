using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplicacaoServico.Model
{
    public class LogServico
    {
        private static string _URL_PROJETO = ConfigurationManager.AppSettings["URL_PROJETO"];

        public void GravaLogErro(string log)
        {
            string caminho = $"{_URL_PROJETO}\\LogServico";
            var diretorio = Path.Combine(caminho, "ERRO_LOG.txt");

            using (StreamWriter file = new StreamWriter(diretorio, true))
            {
                string data = DateTime.Now.ToString();
                file.WriteLine($"{data} - {log} ");
            }

        }

        public void GravaLogSucesso(string log)
        {
            string caminho = $"{_URL_PROJETO}\\LogServico";
            var diretorio = Path.Combine(caminho, "SUCESSO_LOG.txt");

            using (StreamWriter file = new StreamWriter(diretorio, true))
            {
                string data = DateTime.Now.ToString();
                file.WriteLine($"{data} - {log} ");
            }

        }

        public void GravaLogStatus(string log)
        {
            string caminho = $"{_URL_PROJETO}\\LogServico";
            var diretorio = Path.Combine(caminho, "STATUS_LOG.txt");

            using (StreamWriter file = new StreamWriter(diretorio, true))
            {
                string data = DateTime.Now.ToString();
                file.WriteLine($"{data} - {log} ");
            }

        }
    }
}
