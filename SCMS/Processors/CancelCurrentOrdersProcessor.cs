using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SCSM.Business;

namespace SCMS
{
    class CancelCurrentOrdersProcessor
    {
        private Mediator _mediator;
        public string Name { get; set; }
        public string itemCode { get; set; }

        public CancelCurrentOrdersProcessor(Mediator mediator, string name, string itemcode)
        {

            Name = name;
            _mediator = mediator;
            _mediator.MessageReceived += new MessageReceivedEventHandler(Recieve);
            itemCode = itemcode;
        }

        private void Recieve(string message, string from)
        {
            //if we get the message from the mediator run the following call
            if (from == "Process 1" && message == "delete")
            {
                //ask the user if the want to cancel current orders
                var DatabaseCall = new SCSM.Business.DatabaseCalls();
                DialogResult confirmResult = MessageBox.Show("Do you also want to cancel all current orders for " + itemCode,
                                                           "Confirm Cancel",
                                            MessageBoxButtons.YesNo);

                if (confirmResult == DialogResult.Yes)
                {
                    //if they do want to cancel the orders 

                    DatabaseCall.CancelOrders(message);
                    /*Send a message to the mediator to indicate success. Mediator will then send a message via the event channel
                    so the next process knows when to start*/
                    Send("delete");
                }

                else
                {
                    /*Send a message to the mediator to indicate success. Mediator will then send a message via the event channel
                    so the next process knows when to start*/
                    Send("delete");
                }
            }
        }

        public void Send(string message)
        {
            _mediator.Send(message, Name);
        }
    }
}
