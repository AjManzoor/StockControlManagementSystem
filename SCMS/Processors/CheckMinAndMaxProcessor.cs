using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SCSM.Data.Respositories;

namespace SCMS
{
    class CheckMinAndMaxProcessor
    {
        private Mediator _mediator;
        public string Name { get; set; }
        public OrderItem itemOrder { get; set; }


        //constructor
        public CheckMinAndMaxProcessor(Mediator mediator, string name, OrderItem itemorder)
        {

            Name = name;
            itemOrder = itemorder;
            _mediator = mediator;
            _mediator.MessageReceived += new MessageReceivedEventHandler(Recieve);

        }

        //what to do once we recieve the message
        private void Recieve(string message, string from) //make the message the item code
        {
            //waits for the following message from the event channel once it gets it executes the following code
            if (from == "startEvent" && message == "order")
            {
                //Calls our buisness layer to check if the item entered is within the minimum and maximum range
                var DatabaseCall = new SCSM.Business.DatabaseCalls();
                var val = DatabaseCall.CheckMinAndMaxRequired(itemOrder);

                //if too many alert the user
                if (val == "too many")
                {
                    MessageBox.Show("You need to order less (Maximum orders exceeded)");
                    //send a message to the mediator to indicate failure. The next processor will not start
                    Send("cancel");
                }
                else if (val == "too little")
                {
                    MessageBox.Show("You need to order more (Minimum orders not met)");
                    //send a message to the mediator to indicate failure. The next processor will not start

                    Send("cancel");
                }
                else
                {
                    /*Send a message to the mediator to indicate success. Mediator will then send a message via the event channel
                    so the next process knows when to start*/
                    Send("order");
                }
            }

        }

        //function send a message back to the mediator 
        private void Send(string message)
        {
            _mediator.Send(message, Name);
        }
    }
}
