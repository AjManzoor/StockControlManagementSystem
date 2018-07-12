using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SCMS
{
    //Mediator class

    /*Declare a delegate
     * Delegates are just pointers to functions. Allows all subscribers to access the mediator class.
     * This allows all the functions which are subscribed to the mediator to send and recieve messages via the mediator
     * This allows me to implement the mediator toplogy as the mediator is used as the central channel to send messages
     * to all the processors by doing this the mediator can dictate the order in which the processors are run
     */
    public delegate void MessageReceivedEventHandler(string message, string from);

    public class Mediator
    {
        public event MessageReceivedEventHandler MessageReceived;

        //send is the event channel. It's how the different processes get the messages so they know which order to run
        public void Send(string message, string from)
        {
            if (MessageReceived != null)
            {
                //MessageBox.Show(("Sending" + message + from));
                //Console.WriteLine("Sending '{0}' from {1}", message, from);
                MessageReceived(message, from);
            }
        }
    }
}
