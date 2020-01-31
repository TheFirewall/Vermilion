using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vermilion.Event;

namespace Vermilion
{
    public class MainEventArgs : EventArgs
    {

        public virtual bool OnCallEvent()
        {
            return true;
        }

        public bool Cancel { get; set; }
    }
}
