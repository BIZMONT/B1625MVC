using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B1625MVC.BLL
{
    public class OperationDetails
    {
        public OperationDetails(bool succedeed, string message)
        {
            Succedeed = succedeed;
            Message = message;
        }
        public bool Succedeed { get; private set; }
        public string Message { get; private set; }
    }
}
