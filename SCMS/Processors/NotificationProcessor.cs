using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SCMS
{
    class NotificationProcessor
    {

        private Mediator _mediator;
        public string Name { get; set; }
        public string itemCode { get; set; }

        //constructor
        public NotificationProcessor(Mediator mediator, string name, string itemcode)
        {

            Name = name;
            _mediator = mediator;
            _mediator.MessageReceived += new MessageReceivedEventHandler(Recieve);
            itemCode = itemcode;
        }


        private void Recieve(string message, string from)
        {
            //listens for the message then execute the following code
            if (from == "Process 4" && message == "delete")
            {
                //outputs the following messsage letting the user know they have been succesfull
                MessageBox.Show("You have succesfully deleted " + itemCode);
            }

            //listens for the message then execute the following code
            if(from == "Process 4" && message == "order")
            {
                //outputs the following messsage letting the user know they have been succesfull
                MessageBox.Show("You have sucessfully ordered more of " + itemCode);
            }

            if (from == "Process 4" && message == "add")
            {
                MessageBox.Show("You have sucessfully added " + itemCode + " to the inventory");
            }
        }

        public void Send(string message)
        {
            _mediator.Send(message, Name);
        }
    }
}

