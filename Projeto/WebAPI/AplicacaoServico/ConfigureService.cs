using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace AplicacaoServico
{
    public class ConfigureService
    {

        internal static void Configure()
        {
            HostFactory.Run(configure =>
            {
                configure.Service<ServicoTopShelf>(service =>
                {
                    service.ConstructUsing(s => new ServicoTopShelf());
                    service.WhenStarted(s => s.Start());
                    service.WhenStopped(s => s.Stop());
                });
                //Configure a Conta que o serviço do Windows usa para rodar
                configure.RunAsLocalSystem();
                configure.SetServiceName("MeuServicoWindowsComTopshelf");
                configure.SetDisplayName("MeuServicoWindowsComTopshelf");
                configure.SetDescription("Meu serviço Windows com Topshelf");
            });
        }
    }
}
