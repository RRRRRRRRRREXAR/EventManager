using EventMangerBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject.Modules;
using EventMangerBLL.Interfaces;

namespace EventManager.Util
{
    public class EventModule:NinjectModule
    {
        public override void Load()
        {
            Bind<IEventService>().To<EventService>();
        }
    }
}