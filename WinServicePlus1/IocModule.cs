using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace converter

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
            watcher.Changed += new FileSystemEventHandler(onChanged);
            watcher.Created += new FileSystemEventHandler(onChanged);
            watcher.Deleted += new FileSystemEventHandler(onDeleted);
            watcher.Renamed += new RenamedEventHandler(onRenamed);
            // Começa a vigiar o diretório
            watcher.EnableRaisingEvents = true;

        }

        private static void onDeleted(object sender, FileSystemEventArgs e)
        {
            DateTime aDay = DateTime.Now;
            Console.WriteLine("O arquivo {0} foi excluído.", e.Name + "-" + aDay.ToString("s"));
        }

        private static void onRenamed(object sender, RenamedEventArgs e)
        {
            DateTime aDay = DateTime.Now;
            Console.WriteLine("O arquivo {0} foi renomeado para: {1}", e.OldName, e.Name + "-" + aDay.ToString("s"));
        }
        private static void onChanged(object sender, FileSystemEventArgs e)
        {
            DateTime aDay = DateTime.Now;
            Console.WriteLine("Arquivo: {0} \t Tipo de Alteração: {1}", e.Name, e.ChangeType + "-" + aDay.ToString("s"));
        }
    }
}

