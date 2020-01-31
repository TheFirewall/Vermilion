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
    public class EventManager : IEventManager
    {

        public void LoadModuleEvents(IModule evt)
        {
            Assembly newAssembly = evt.GetType().Assembly;

            //Type[] types = newAssembly.GetExportedTypes();

            //foreach (Type type in types)
            {
                try
                {
                    var ctor = evt.GetType().GetConstructor(Type.EmptyTypes);
                    if (ctor != null)
                    {
                        LoadEvents(evt.GetType(), evt);
                    }
                }
                catch (Exception ex)
                {
                    //Log.WarnFormat("Failed loading event type {0} as a event.", type);
                    //Log.Debug("Event loader caught exception, but is moving on.", ex);
                }
            }
        }


        public void LoadEvents(Type type, IModule evt)
        {
            var methods = type.GetMethods();
            foreach (MethodInfo method in methods)
            {
                EventAttribute eventAttribute = Attribute.GetCustomAttribute(method, typeof(EventAttribute), false) as EventAttribute;
                if (eventAttribute == null) continue;

                /*var publisher = eventAttribute.Type.GetField("eventPublisher", BindingFlags.Public | BindingFlags.Static).GetValue(null);//getpublisher.Invoke(eventAttribute.Type, null) as EventPublisher;
                if (publisher == null)
                {
                    var field = eventAttribute.Type.GetField("eventPublisher", BindingFlags.Public | BindingFlags.Static);

                    publisher = new EventPublisher();
                    field.SetValue(null, publisher);
                }*/

                EventInfo eventInfo = eventAttribute.Type.GetEvent("Event");
                //EventInfo eventInfo = publisher.GetType().GetEvent("Event");
                var eType = eventInfo.EventHandlerType;

                Delegate handler = null;

                try
                {
                    handler = Delegate.CreateDelegate(eType, evt, method);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                eventInfo.AddEventHandler(eventAttribute.Type, handler);
                /*if (publisher != null)
                {
                    eventInfo.AddEventHandler(publisher, handler);
                    Console.WriteLine("la00");
                }
                else
                {
                    eventInfo.AddEventHandler(publisher, handler);
                    Console.WriteLine("la0");
                }*/
            }
        }
    }

    public class EventPublisher
    {
        public event EventHandler<MainEventArgs> Event;

        public Type type;

        public bool OnEvent(MainEventArgs e)
        {
            Console.WriteLine("la2");
            //Event?.Invoke(this, e);
            Event(this, e);
            return !e.Cancel;
        }
    }
}
