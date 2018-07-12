using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCSM.Data.Respositories;
using System.Windows.Forms;

namespace SCMS.Processors
{
    class ValidationProcessor
    {
            private Mediator _mediator;
            public string Name { get; set; }
            public AddStockItem stockItem { get; set; }


            //constructor
            public ValidationProcessor(Mediator mediator, string name, AddStockItem stockitem)
            {

                Name = name;
                stockItem = stockitem;
                _mediator = mediator;
                _mediator.MessageReceived += new MessageReceivedEventHandler(Recieve);

            }

        //what to do once we recieve the message
        private void Recieve(string message, string from) //make the message the item code
        {
            SCSM.Business.Validation Validator = new SCSM.Business.Validation();

            //get a message that the initial event has been triggered and the success message (add) has been sent
            if (from == "startEvent" && message == "add")
            {
                //if the input isn't correct send a message to the mediator telling it it has failed
                if (!Validator.CheckInput(stockItem.itemCode))
                {
                    MessageBox.Show("Please enter an item code");
                    Send("Cancel");
                    return;
                }

                //if the input isn't correct send a message to the mediator telling it it has failed
                if (!Validator.CheckInput(stockItem.itemName))
                {
                    MessageBox.Show("Please enter an item name");
                    Send("Cancel");
                    return;
                }

                //if the input isn't correct send a message to the mediator telling it it has failed
                if (!Validator.CheckInputPrice(stockItem.itemPrice))
                {
                    MessageBox.Show("Please enter a number in the following format => 12.99");
                    Send("Cancel");
                    return;
                }

                //if the input isn't correct send a message to the mediator telling it it has failed
                if (!Validator.CheckInput(stockItem.itemQuantity))
                {
                    MessageBox.Show("Please enter an a postive value");
                    Send("Cancel");
                    return;
                }

                //if the input isn't correct send a message to the mediator telling it it has failed
                if (!Validator.CheckInputDate(stockItem.stockArrivalDate))
                {
                    MessageBox.Show("Please enter a date in the future");
                    Send("Cancel");
                    return;
                }

                //if the input isn't correct send a message to the mediator telling it it has failed
                if (!Validator.CheckInput(stockItem.minRequired))
                {
                    MessageBox.Show("Please enter a positive value");
                    Send("Cancel");
                    return;

                }

                //if the input isn't correct send a message to the mediator telling it it has failed
                if (!Validator.CheckInput(stockItem.maxRequired))
                {
                    MessageBox.Show("Please enter a positive value");
                    Send("Cancel");
                    return;
                }
                /*Send a message to the mediator to indicate success. Mediator will then send a message via the event channel
                    so the next process knows when to start*/
                Send("add");

            }

        }

            //send a message to the mediator
            public void Send(string message)
            {
                _mediator.Send(message, Name);
            }
    }
}
