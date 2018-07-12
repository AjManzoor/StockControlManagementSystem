using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SCSM.Business;

namespace SCMS
{

    public class CheckRevenueGenerateProcessor
    {
        private Mediator _mediator;
        public string Name { get; set; }
        public string itemCode { get; set; }


        //constructor
        public CheckRevenueGenerateProcessor(Mediator mediator, string name, string itemcode)
        {
            
            Name = name;
            itemCode = itemcode;
            _mediator = mediator;
            _mediator.MessageReceived += new MessageReceivedEventHandler(Recieve);

        }

        //what to do once we recieve the message
        private void Recieve(string message, string from) //make the message the item code
        {

            if (from == "startEvent" && message == "delete")
            {
                var DatabaseCall = new SCSM.Business.DatabaseCalls();
                var revenue = DatabaseCall.GenerateRevenue(itemCode);
                DialogResult confirmResult = DialogResult.Yes;

                //if the revenue generated is greater than 2000 ask them if they really want to delete the item as it's well selling
                if (revenue > 2000)
                {
                    confirmResult = MessageBox.Show("The item with item code " + itemCode + " generated £" + revenue + " Are you sure you want to delete this item",
                                    "Confirm Delete!!" + message,
                                    MessageBoxButtons.YesNo);
                    if (confirmResult == DialogResult.Yes)
                    {
                        //if they do send the sucess message so the mediator knows to run the next proccess
                        Send("delete");
                    }
                    else
                    {
                        //if they decide not to tell the mediator to call any other events
                        Send("cancel");

                    }


                }
                else
                {
                    Send("delete");

                }
            }
            else
            {
                //when we get the message from the event channel telling us process one has been a success we then execute the following logic
                if (from == "Process 1" && message == "order")
                {
                    //initalise the controller (MVC Pattern_
                    var DatabaseCall = new SCSM.Business.DatabaseCalls();

                    //Get the how much revenue the item generatated
                    var revenue = DatabaseCall.GenerateRevenue(itemCode);
                    DialogResult confirmResult = DialogResult.Yes;

                    //if it generated less than £2000 ask the user if they really want to reorder
                    if (revenue < 2000)
                    {
                        confirmResult = MessageBox.Show("The item with item code " + itemCode + " generated £" + revenue + " Are you sure you want to order more of this item",
                                        "Confirm Delete!!" + message,
                                        MessageBoxButtons.YesNo);
                        if (confirmResult == DialogResult.Yes)
                        {
                            //if they do send the mediator the message order which implies success the next processor start
                            Send("order");
                        }
                        else
                        {
                            //if they don't want to reorder send cancel which implies failure the next processor will not start
                            Send("cancel");

                        }


                    }
                    else
                    {
                    /*Send a message to the mediator to indicate success. Mediator will then send a message via the event channel
                    so the next process knows when to start*/
                        Send("delete");

                    }
                }

            }

        }

        //send a message to the mediator
        public void Send(string message)
        {
            //MessageBox.Show("sending mesg");
            _mediator.Send(message, Name);
        }
    }
}
