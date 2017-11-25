using CommonLib;
using DotNetCore.CAP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoA.Services
{
    public interface IMainBusiness
    {
        void ReceiveMessage(Person person);
    }

    public class MainBusiness : IMainBusiness, ICapSubscribe
    {

        //[CapSubscribe("CapDemo.Service")]
        //[CapSubscribe("CapDemo")]
        public void ReceiveMessage(Person person)
        {
            Console.WriteLine("received message from service: {0}", person.ToString());
        }
    }
}
