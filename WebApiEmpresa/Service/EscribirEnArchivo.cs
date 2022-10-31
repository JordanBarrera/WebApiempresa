using Microsoft.AspNetCore.Mvc;
using WebApiEmpresa.Controllers;
using WebApiEmpresa.Entidades;
namespace WebApiEmpresa.Service
{
    public class EscribirEnArchivo : IHostedService
    {
        private readonly IWebHostEnvironment env;

        private readonly string nombreArchivo = "ArchivoEmpresa1.txt";
        // private readonly string archivo = "ListadoEmpresas.txt";
        private Timer timer;

        public EscribirEnArchivo(IWebHostEnvironment env)
        {
            this.env = env;

        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            //Se ejecuta cuando cargamos la aplicacion 1 vez
            timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            Escribir("Proceso Iniciado");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer.Dispose();
            Escribir("Proceso Finalizado");
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            Escribir("Proceso en ejecucion: " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));
            //GuardarMascotas();
        }
        private void Escribir(string msg)
        {
            var ruta = $@"{env.ContentRootPath}\wwwroot\{nombreArchivo}";

            using (StreamWriter writer = new StreamWriter(ruta, append: true)) { writer.WriteLine(msg); }
        }

        private void GuardarDueños()
        {
            //var ruta = $@"{env.ContentRootPath}\wwwroot\{archivo}";
            //ActionResult task = dueñosController.ObtenerGuid();
            //object Dueño = task.Result.Value;
            //using (StreamWriter writer = new StreamWriter(ruta, append: true)) { writer.WriteLine(Dueño); }
        }
    }
}
