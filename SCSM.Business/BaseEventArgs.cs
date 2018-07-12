using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSM.Business
{
    //Arguments to be passed by the mediator. Allows events to pass multiple sources of data we only need one.
    public class BaseEventArgs : EventArgs
    {
        public string Message { get; set; }
    }
}
