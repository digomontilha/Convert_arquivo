using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WinServicePlus1
{
    class IocModule : Ninject.Modules.NinjectModule
    {
        public override void Load()
        {

            Bind<ILog>().ToMethod(ctx =>
            {
                Type type = ctx.Request.ParentContext.Request.Service;
                return LogManager.GetLogger(type);
            });

            Bind<Service.WinService>().ToSelf();

            FileSystemWatcher watcher = new FileSystemWatcher();
            try
            {
                watcher.Path = @"C:\Temp";
            }
            catch (Exception e)
            {
                Console.WriteLine("\nOcorreu um erro: " + e.Message);
                return;
            }

            watcher.NotifyFilter = NotifyFilters.LastWrite
                | NotifyFilters.FileName
                | NotifyFilters.DirectoryName;
                //NotifyFilters.LastAccess
                //| NotifyFilters.Attributes
                //| NotifyFilters.CreationTime
                //| NotifyFilters.Security
                //| NotifyFilters.Size
                // Somente vigiará os arquivos de texto
                watcher.Filter = "*.txt";

        }
    }
}

