using AplicacaoServico.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace AplicacaoServico
{
    public class ServicoTopShelf
    {
        private static LogServico _log = new LogServico();
        public void Start()
        {

           
            var timer1 = new Timer();
            timer1.Interval = 120000; // a cada 2 minutos
            timer1.Elapsed += new System.Timers.ElapsedEventHandler(timer1_Tick);
            timer1.Enabled = true;
            System.Console.WriteLine("Serviço iniciado!");
           

            _log.GravaLogStatus("Serviço iniciado!");

           

        }

        private void timer1_Tick(object sender, ElapsedEventArgs e)
        {

            
            System.Console.WriteLine("Iniciando serviço!");
            System.Console.WriteLine("Serviço executado a cada 2 minutos!");

            _log.GravaLogStatus("Serviço executado a cada 2 minutos!");
            Servico.RequestGET("", string.Empty).ToString();

        }

        public void Stop()
        {
            
            Console.Clear();
            
           System.Console.WriteLine("Meu Serviço do Windows Encerrou");

            _log.GravaLogStatus("Meu Serviço do Windows Encerrou");
        }
    }
}
