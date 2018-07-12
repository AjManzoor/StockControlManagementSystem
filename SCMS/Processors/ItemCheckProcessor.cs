using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCSM.Data.Respositories;
using System.Windows.Forms;
using SCSM.Data;

namespace SCMS.Processors
{
    public class ItemCheckProcessor
    {
        private Mediator _mediator;
        public string Name { get; set; }
        public AddStockItem stockItem { get; set; }

        public ItemCheckProcessor(Mediator mediator, string name, AddStockItem stockitem)
        {
            Name = name;
            _mediator = mediator;
            //subscribe to the mediator message recieved via the delegate
            _mediator.MessageReceived += new MessageReceivedEventHandler(Recieve);
            stockItem = stockitem;
        }

        private void Recieve(string message, string from)
        {
            var databaseCall = new SCSM.Business.DatabaseCalls();

            //if process one is a sucess then the check if the item exists
            if (from == "Process 1" && message == "add")
            {
                if (databaseCall.CheckIfItemExists(stockItem.itemCode))
                {
                    //if the item does exist tell send a notification and send message to the mediator to tell it it has failed
                    MessageBox.Show("This item already exists you can not add an existing item");
                    Send("cancel");
                }
                else
                {
                    /*Send a message to the mediator to indicate success. Mediator will then send a message via the event channel
                    so the next process knows when to start*/
                    Send("add");
                }

            }
            
        }

        
        public void Send(string message)
        {
            _mediator.Send(message, Name);
        }
    }
}
