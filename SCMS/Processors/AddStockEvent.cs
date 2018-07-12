using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMS.Processors
{
    public class AddStockEvent
    {
        private Mediator _mediator;
        public string Name { get; set; }


        //initial event which triggers all the processors for adding an item to the stocks
        public AddStockEvent(Mediator mediator, string name)
        {

            Name = name;
            _mediator = mediator;
            _mediator.MessageReceived += new MessageReceivedEventHandler(Recieve);
           
        }

        private void Recieve(string message, string from)
        {


        }

        public void Send(string message)
        {
            _mediator.Send(message, Name);
        }
    }
}
