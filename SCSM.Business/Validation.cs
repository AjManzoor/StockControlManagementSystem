using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSM.Business
{
    //validation business logic
    public class Validation
    {
        
        //checks if the inout is empty
        public bool CheckInput(string val)
        {
            if (string.IsNullOrWhiteSpace(val))
            {
                return false;
            }
            return true;
        }

        //checks the input is greater than 0
        public bool CheckInput(int val)
        {
            if (val < 0)
            {
                return false;
            }

            return true;
        }

        //checks if the price is in the right format by trying to parse the value as a double
        public bool CheckInputPrice(string val)
        {
            try
            {
                Convert.ToDouble(val);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        //checks the user hasn't inputted a date in the past
        public bool CheckInputDate(DateTime val)
        {
            if (val <= DateTime.Now.Date)
            {
                return false;
            }

            return true;
        }
    }
    
}
