using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SCSM.Data.Respositories;

namespace SCMS
{
    class StockItemProcessor
    {
        private Mediator _mediator;
        public string Name { get; set; }
        public string itemCode { get; set; }
        public OrderItem itemOrder { get; set; }
        public AddStockItem stockItem { get; set; }

        public StockItemProcessor(Mediator mediator, string name, string itemcode)
        {

            Name = name;
            _mediator = mediator;
            _mediator.MessageReceived += new MessageReceivedEventHandler(Recieve);
            itemCode = itemcode;
            

        }

        //overload the constructor to take the OrderItem object which is then mapped via the {get; set; } declared above
        public StockItemProcessor(Mediator mediator, string name, OrderItem itemorder)
        {
            Name = name;
            _mediator = mediator;
            _mediator.MessageReceived += new MessageReceivedEventHandler(Recieve);
            itemOrder = itemorder;

        }

        public StockItemProcessor(Mediator mediator, string name, AddStockItem stockitem)
        {
            Name = name;
            _mediator = mediator;
            _mediator.MessageReceived += new MessageReceivedEventHandler(Recieve);
            stockItem = stockitem;

        }


        private void Recieve(string message, string from)
        {
            var databaseCall = new SCSM.Business.DatabaseCalls();

            //if mediator tells us proccess 3 was a sucess we run the following code
            if (from == "Process 3" && message == "delete")
            {

                //delete the item
                databaseCall.DeleteStockItem(itemCode);

                /*Send a message to the mediator to indicate success. Mediator will then send a message via the event channel
                     so the next process knows when to start*/
                Send("delete");
                
            }

            //If it gets the success message from proccess 3 execute the following
            if(from == "Process 3" && message == "order")
            {
                //add it to the db
                databaseCall.OrderStocks(itemOrder);

                /*Send a message to the mediator to indicate success. Mediator will then send a message via the event channel
                so the next process knows when to start*/
                Send("order");
            }

            //If it gets the success message from proccess 3 execute the following
            if (from == "Process 3" && message == "add")
            {
                databaseCall.AddToStock(stockItem);
                /*Send a message to the mediator to indicate success. Mediator will then send a message via the event channel
                    so the next process knows when to start*/
                Send("add");
            }
            
        }

        public void Send(string message)
        {
            _mediator.Send(message, Name);
        }
    }
}
