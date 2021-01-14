using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AplicacaoServico.Model
{
    public class Servico
    {
        private static LogServico _log = new LogServico();
        private static Resultado _Resultado = new Resultado();

        private static string _URL_PROJETO = ConfigurationManager.AppSettings["URL_PROJETO"];

        public static string URI = "https://localhost:44382/api/FilaProcessamento/GetItemFila";        


        public static string DadosMoeda = $"{_URL_PROJETO}\\DadosMoeda.csv";
        public static string TabelaDePara = $"{_URL_PROJETO}\\TabelaDePara.csv";
        public static string DadosCotacao = $"{_URL_PROJETO}\\DadosCotacao.csv";        

        
        private static string RequesteGET_DELETE(string metodo, string parametro, string tipo)
        {
            List<Fila> retorno = new List<Fila>();
            Fila item = new Fila();
            var request = (HttpWebRequest)WebRequest.Create(URI);
            // request.Headers.Add("Token", TOKEN);
            request.Method = tipo;
            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new System.IO.StreamReader(response.GetResponseStream()).ReadToEnd();

               retorno = JsonConvert.DeserializeObject<List<Fila>>(responseString.ToString());

                if(retorno[0].Mensagem != null)
                {
                    _log.GravaLogErro($"{retorno[0].Mensagem}");
                    System.Console.WriteLine($"{DateTime.Now} - {retorno[0].Mensagem}");                    
                    return ($"{retorno[0].Mensagem}");
                    
                }

                if(retorno[0].moeda != null && retorno[0].data_fim != null && retorno[0].data_inicio != null && retorno[0].Mensagem == null)
                {
                    _log.GravaLogSucesso($"A api retornou {retorno.Count} items na fila.");
                    System.Console.WriteLine($"{DateTime.Now} - A api retornou {retorno.Count} items na fila.");
                    System.Console.WriteLine($"{DateTime.Now} - Gerando arquivo de resultado.");
                    _Resultado.GerarArquivo();
                    System.Console.WriteLine($"{DateTime.Now} - Processando as consultas....");
                    System.Console.WriteLine($"{DateTime.Now} - Aguarde....");

                    /*Pegando os dados da API e formatando num datatable*/
                    for (int i = 0; i < retorno.Count; i++)
                    {
                        item.moeda = retorno[i].moeda.ToString();
                        item.data_inicio = Convert.ToDateTime(retorno[i].data_inicio).ToString("dd/MM/yyyy");
                        item.data_fim = Convert.ToDateTime(retorno[i].data_fim).ToString("dd/MM/yyyy");

                        /************************************/
                        /*Abrindo Arquivo CSV DAdos Moeda*/
                        /**********************************/
                        string caminhoArquivo = $"{DadosMoeda}";
                        FileStream fs = new System.IO.FileStream(caminhoArquivo, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);
                        StreamReader sr = new StreamReader(fs, Encoding.GetEncoding("iso-8859-1"));
                       

                        string line = sr.ReadLine();
                        string[] value = line.Split(';');
                        DataTable dt = new DataTable();
                        DataRow row;
                        /*Montando as colunas*/
                        foreach (string dc in value)
                        {
                            dt.Columns.Add(new DataColumn(dc));
                        }
                        /*Montando as linhas*/
                        while (!sr.EndOfStream)
                        {
                            value = sr.ReadLine().Split(';');

                            if (value.Length == dt.Columns.Count)
                            {
                                /*Convertendo string em datetime para comparar o periodo de data*/
                                DateTime dataInicial;
                                DateTime dataFinal; 
                                DateTime data_Ref; 
                                DateTime.TryParse(item.data_inicio.ToString(), out dataInicial).Equals(true);
                                DateTime.TryParse(item.data_fim.ToString(), out dataFinal).Equals(true);
                                DateTime.TryParse(value[1].ToString(), out data_Ref).Equals(true);

                                /*Vai me trazer todas as moedas que tem o mesmo nome e dentro do periodo DATA_REF do arquivo DadosMoeda*/
                                if (value[0].ToString() == $"{item.moeda}" && data_Ref >= dataInicial && data_Ref <= dataFinal )
                                {
                                    row = dt.NewRow();
                                    row.ItemArray = value;
                                    dt.Rows.Add(row);

                                    item.DATA_REF = value[1].ToString();
                                    item.ID_MOEDA = value[0].ToString();

                                    /*************************************************************************************************************************************************************************/
                                    /******************************************************/
                                    /*Abrindo Arquivo CSV DAdos TabelaDePara*/
                                    /*********************************************/
                                    string caminhoArquivoTabelaDePara = $"{TabelaDePara}";
                                    FileStream fs2 = new System.IO.FileStream(caminhoArquivoTabelaDePara, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);
                                    StreamReader sr2 = new StreamReader(fs2, Encoding.GetEncoding("iso-8859-1"));
                                   

                                    string line2 = sr2.ReadLine();
                                    string[] value2 = line2.Split(';');
                                    DataTable dt2 = new DataTable();
                                    DataRow row2;
                                    /*Montando as colunas*/
                                    foreach (string dc2 in value2)
                                    {
                                        dt2.Columns.Add(new DataColumn(dc2));
                                    }
                                    /*Montando as linhas*/
                                    while (!sr2.EndOfStream)
                                    {
                                        value2 = sr2.ReadLine().Split(';');

                                        if (value2.Length == dt2.Columns.Count)
                                        {
                                            /*Vai me trazer o codigo da cotação da moeda*/
                                            if (value2[0].ToString() == $"{item.moeda}")
                                            {
                                                row2 = dt2.NewRow();
                                                row2.ItemArray = value2;
                                                dt2.Rows.Add(row2);

                                                item.cod_cotacao = value2[1].ToString();

                                            }

                                            /******************************************************/
                                            /*Abrindo Arquivo CSV DAdos DadosCotacao **************/
                                            /********************************************************/
                                            string caminhoArquivoTabelaCotacao = $"{DadosCotacao}";
                                            FileStream fs3 = new System.IO.FileStream(caminhoArquivoTabelaCotacao, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);
                                            StreamReader sr3 = new StreamReader(fs3, Encoding.GetEncoding("iso-8859-1"));
                                           

                                            string line3 = sr3.ReadLine();
                                            string[] value3 = line3.Split(';');
                                            DataTable dt3 = new DataTable();
                                            DataRow row3;
                                            /*Montando as colunas*/
                                            foreach (string dc3 in value3)
                                            {
                                                dt3.Columns.Add(new DataColumn(dc3));
                                            }
                                            /*Montando as linhas*/
                                            while (!sr3.EndOfStream)
                                            {
                                                value3 = sr3.ReadLine().Split(';');

                                                if (value3.Length == dt3.Columns.Count)
                                                {
                                                    DateTime data_cotacao2;
                                                    DateTime.TryParse(value3[2].ToString(), out data_cotacao2).Equals(true);

                                                    /*Vai me trazer o codigo da cotação*/
                                                    if (value3[1].ToString() == $"{item.cod_cotacao}" && data_cotacao2 >= dataInicial && data_cotacao2 <= dataFinal)
                                                    {
                                                        row3 = dt3.NewRow();
                                                        row3.ItemArray = value3;
                                                        dt3.Rows.Add(row3);

                                                        item.vlr_cotacao = value3[0].ToString();
                                                        item.dat_cotacao = value3[2].ToString();

                                                        _Resultado.GravarResultado(item.ID_MOEDA, item.DATA_REF, item.vlr_cotacao, item.dat_cotacao);

                                                    }

                                                }
                                            }

                                        }
                                    }

                                }
                                
                            }
                        }

                        Thread.Sleep(2000);

                        // _Resultado.GravarResultado();



                    }

                  
                   

                }              


                return responseString;
            }
            catch (Exception)
            {

                return ("Nenhum registro encontrado ao consumir a API!");
            }

        }

        public static string RequestGET(string metodo, string parametro)
        {
            return RequesteGET_DELETE(metodo, parametro, "GET");
        }

    }
}
