using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyNetQ;
using DataModelLibrary;
using DataServiceLibrary;
using SimpleInjector;
using Repositorylibrary;
using System.Data.Entity;

namespace SubscriberProessor.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Email Processor notified");
            var lifestyle = Lifestyle.Singleton;
            var container = new Container();
            container.Register<DbContext, Model1>(); 
            container.Register<IGenericRepository<Subscriber>, Genericrepository<Subscriber>>();
            container.Register<ISubscriberService, SubscriberService>();
            container.Register<IEmailService, EmailService>();
            var ibus = RabbitHutch.CreateBus("host=localhost");
            ibus.Subscribe<SubscriberSavedMessage>("emailnotifier", (ssv) => {
                try
                {
                    Console.WriteLine(string.Format("Consumer received the request for the subscriber Id {0} ", ssv.Id));
                    var emailservice = container.GetInstance<IEmailService>();
                     emailservice.SendMail();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("problem in processing subsciver email functionality", ex.Message);
                }
            });
        }
    }
}
