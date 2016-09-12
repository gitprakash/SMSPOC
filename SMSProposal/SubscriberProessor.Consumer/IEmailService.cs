using DataModelLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriberProessor.Consumer
{
    interface IEmailService
    {
        Task SendAgreementFormMail(SubscriberSavedMessage ssm);
    }
}
