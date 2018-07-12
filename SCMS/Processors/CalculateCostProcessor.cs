using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCSM.Business;
using SCSM.Data.Respositories;
using System.Windows.Forms;

namespace SCMS.Processors
{
    class CalculateCostProcessor
    {
        private Mediator _mediator;
        public string Name { get; set; }
        public OrderItem itemOrder { get; set; }

        //constructor
        public CalculateCostProcessor(Mediator mediator, string name, OrderItem itemorder)
        {

            Name = name;
            _mediator = mediator;
            _mediator.MessageReceived += new MessageReceivedEventHandler(Recieve);
            itemOrder = itemorder;
        }

        private void Recieve(string message, string from)
        {
            if (from == "Process 2" && message == "order")
            {
                var databaseCall = new DatabaseCalls();

                //calculates how much its's going to cost
                var cost = databaseCall.CalculateCost(itemOrder);
                MessageBox.Show("This will cost £ " + cost);

                /*Send a message to the mediator to indicate success. Mediator will then send a message via the event channel
                                    so the next process knows when to start*/
                Send("order"); 
            }
        }

        //this is used to send messages back up to the mediator which then sends a message via the event channel
        public void Send(string message)
        {
            _mediator.Send(message, Name);
        }
    }
}
