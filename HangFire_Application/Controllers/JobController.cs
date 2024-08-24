using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace HangFire_Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {


        [HttpGet]
        public void ListaInteiros()
        {
            for (int i = 0; i < 100000; i++)
            {
                Console.WriteLine(i);
            }
        }
        //Tarefa para executar serviço em segundo plano
        [HttpPost]
        [Route("CriarBackgroundJob")]
        public IActionResult CriarBackgroundJob()
        {
            BackgroundJob.Enqueue(() => ListaInteiros());
            return Ok();
        }

        //Tarefa para executar agendamento de execução de serviços
        [HttpPost]
        [Route("CriarScheduledJob")]
        public IActionResult CriarScheduledJob()
        {
            var scheduleDateTime = DateTime.UtcNow.AddSeconds(5);
            var dateTimeOffSet = new DateTimeOffset(scheduleDateTime);

            BackgroundJob.Schedule(() => Console.WriteLine("Tarefa Agendada para 5 segundos"), dateTimeOffSet);

            return Ok();
        }

        //Tarefa de continuação
        [HttpPost]
        [Route("CriarContinuationJob")]
        public IActionResult CriarContinuationJob()
        {
            var scheduleDateTime = DateTime.UtcNow.AddSeconds(5);
            var dateTimeOffSet = new DateTimeOffset(scheduleDateTime);

            var job1 = BackgroundJob.Schedule(() => Console.WriteLine("Tarefa Agendada para 5 segundos"), dateTimeOffSet);

            var job2 = BackgroundJob.ContinueJobWith(job1, () => Console.WriteLine("Segundo Job"));

            var job3 = BackgroundJob.ContinueJobWith(job1, () => Console.WriteLine("Terceiro Job"));

            var job4 = BackgroundJob.ContinueJobWith(job1, () => Console.WriteLine("Quarto Job"));


            return Ok();
        }

        //Tarefa execução de job de tempo em tempo
        [HttpPost]
        [Route("CriarRecurringJob")]
        public IActionResult CriarRecurringJob()
        {
            RecurringJob.AddOrUpdate( "RecurringJob1", () => Console.WriteLine("RecurringJob"), "*  *  *  *  *");
            return Ok();
        }
    }
}
