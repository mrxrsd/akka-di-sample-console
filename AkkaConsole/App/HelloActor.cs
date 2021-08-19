using System;
using System.Collections.Generic;
using System.Text;
using Akka.Actor;

namespace App
{

    public class HelloActor : UntypedActor, IWithTimers
    {
        private readonly IGreetService _greetService;

        public HelloActor(IGreetService greetService)
        {
            _greetService = greetService;
        }
        protected override void OnReceive(object message)
        {
            Console.WriteLine(_greetService.Greet(message?.ToString()));
        }

        public ITimerScheduler Timers { get; set; }

        protected override void PreStart()
        {
            Timers.StartPeriodicTimer("loop", "Aaron", TimeSpan.FromSeconds(2));
        }
    }
}
