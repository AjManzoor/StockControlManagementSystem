using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCSM.Data.Respositories;
using SCSM.Data;
using System.Windows.Forms;

namespace SCMS.Processors
{
   public class BudgetProcessor
    {
        private Mediator _mediator;
        public string Name { get; set; }
        public AddStockItem stockItem { get; set; }

        //static variable monthly budget
        private static int MONTHLY_BUDGET = 20000;

        public BudgetProcessor(Mediator mediator, string name, AddStockItem stockitem)
        {
            Name = name;
            _mediator = mediator;
            _mediator.MessageReceived += new MessageReceivedEventHandler(Recieve);
            stockItem = stockitem;
        }

        private void Recieve(string message, string from)
        {
            var databaseCall = new SCSM.Business.DatabaseCalls();

            //get the message from the mediator id the previous processes passed their success message it executes the appropriate logic
            if (from == "Process 2" && message == "add")
            {
                //if the amount spent so far exceeds the monthly budget sends a warning
                if (databaseCall.CalculateMoneySpent() > MONTHLY_BUDGET)
                {
                    MessageBox.Show("We are already overbudget please reconsider adding more stock and instead focus on selling the goods we have");

                    /*Send a message to the mediator to indicate success. Mediator will then send a message via the event channel
                                        so the next process knows when to start*/
                    Send("add");
                }
                Send("add");


            }

        }

        public void Send(string message)
        {
            _mediator.Send(message, Name);
        }
    }
}
