using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilaProcessamentoController : ControllerBase
    {
        // GET: api/<MoedaController>
        [HttpGet]
        [Route("GetItemFila")]
        public List<Desafio_1> GetItemFila()
        {


            return new Desafio_1().PesquisarItem();

        }

       
        // POST api/<MoedaController>
        [HttpPost]
        [Route("AddItemFila")]
        public ReturnAllServices AddItemFila([FromBody] Desafio_1[] dados)
        {
            ReturnAllServices retorno = new ReturnAllServices();
            Desafio_1 obj = new Desafio_1();

            try
            {
                obj.SalvarItem(dados);
                retorno.ErrorMessage = "Salvo com sucesso";
            }
            catch (Exception ex)
            {

                retorno.ErrorMessage = "Erro ao tentar registrar o item: " + ex.Message;
            }


            return retorno;
        }


      
    }
}
