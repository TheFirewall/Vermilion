using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Vermilion.Event.Player;
using Vermilion.Modules;

namespace Vermilion.Event
{
    public interface IEventManager
    {

        void LoadModuleEvents(IModule evt);


        void LoadEvents(Type type, IModule evt);
    }
}
