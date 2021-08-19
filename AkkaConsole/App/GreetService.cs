using System;
using System.Collections.Generic;
using System.Text;

namespace App
{
    public interface IGreetService
    {
        string Greet(string name);
    }
    public class GreetService : IGreetService
    {
        public string Greet(string name) => $"Hello {name}";
    }
}
