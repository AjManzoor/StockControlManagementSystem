using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SCMS
{
    class ConfirmationProcessor
    {
        private Mediator _mediator;
        public string Name { get; set; }
        public string itemCode { get; set; }

        public ConfirmationProcessor(Mediator mediator, string name, string itemcode)
        {

            Name = name;
            _mediator = mediator;
            _mediator.MessageReceived += new MessageReceivedEventHandler(Recieve);
            itemCode = itemcode;
        }

        private void Recieve(string message, string from)
        {
            //listen to the message from the mediator
            if (from == "Process 2" && message == "delete")
            {
                var confirmResult = MessageBox.Show("Are you sure to delete " + itemCode + " ?",
                                     "Confirm Delete!!",
                                     MessageBoxButtons.YesNo);

            if(confirmResult == DialogResult.Yes)
            {
                    /*Send a message to the mediator to indicate success. Mediator will then send a message via the event channel
                    so the next process knows when to start*/
                    Send("delete");
            }

            //if they decide the don't want to delete send a cancel message which tells it not to run the other processors
            else
            {
                Send("Cancel");
            }
               
            }
        }

        public void Send(string message)
        {
            _mediator.Send(message, Name);
        }
    }
}
