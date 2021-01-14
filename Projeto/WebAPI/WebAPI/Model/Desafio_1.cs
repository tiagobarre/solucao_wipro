using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Model
{
    public class Desafio_1
    {
       // public int id { get; set; }
        public string moeda { get; set; }
        public string data_inicio { get; set; }
        public string data_fim { get; set; }
        public string Mensagem { get; set; }
       

        public List<Desafio_1> SalvarItem(Desafio_1[] dados)
        {
            List<Desafio_1> retorno = new List<Desafio_1>();
            DAL.DAL objDAL = new DAL.DAL();

           string json = JsonConvert.SerializeObject(dados);
            

            string sql = $"insert into projeto_wipro.desafio_1(json,data) values('{json}', '{DateTime.Now}')";

            objDAL.ExecutarComandoSQL(sql);


            return retorno;
        }

        public List<Desafio_1> PesquisarItem()
        {
            List<Desafio_1> lista = new List<Desafio_1>();
            List<Desafio_1> lista2 = new List<Desafio_1>();
            List<Desafio_1> retorno = new List<Desafio_1>();
            DAL.DAL objDAL = new DAL.DAL();
            Desafio_1 item = new Desafio_1();
            Desafio_1 item2 = new Desafio_1();
                     

            string sql = $"select * from projeto_wipro.desafio_1 order by id desc limit 1";
            DataTable dt = objDAL.RetDataTable(sql);

            if(dt.Rows.Count > 0)
            {
                retorno = JsonConvert.DeserializeObject<List<Desafio_1>>(dt.Rows[0]["json"].ToString());

                int id = int.Parse(dt.Rows[0]["id"].ToString());

                if (retorno.Count > 0)
                {
                    for (int i = 0; i < retorno.Count; i++)
                    {
                        item = new Desafio_1()
                        {
                            moeda = retorno[i].moeda.ToString(),
                            data_inicio = retorno[i].data_inicio.ToString(),
                            data_fim = retorno[i].data_fim.ToString(),

                        };

                        lista.Add(item);
                    }

                    string sql2 = $"delete from projeto_wipro.desafio_1 where id = '{id}'";
                    objDAL.ExecutarComandoSQL(sql2);

                    return lista;
                }

            }          
            else
            {
                item2 = new Desafio_1() { Mensagem = "Não existem objetos a serem retornados!" };
                lista2.Add(item2);
                return lista2;
            }

            return lista;

        } 
        

    }
}
