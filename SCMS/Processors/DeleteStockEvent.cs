using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SCMS
{
    public class DeleteStockEvent
    {
        private Mediator _mediator;
        public string Name { get; set; }

        //initial event which starts the processors for deleteing a stock item
        public DeleteStockEvent(Mediator mediator, string name)
        {

            Name = name;
            _mediator = mediator;
            _mediator.MessageReceived += new MessageReceivedEventHandler(Recieve);
            //Send("delete");
        }

        private void Recieve(string message, string from)
        {
            
            
        }

        public void Send(string message)
        {
           // MessageBox.Show("Sending message: " + message + " " + "Name: " + Name);
            _mediator.Send(message, Name);
        }
    }
}
