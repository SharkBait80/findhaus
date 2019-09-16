using Autofac;
using HouseService.DataLayer;
using HouseService.DataLayer.Aurora;
using System;
using System.Collections.Generic;
using System.Text;

namespace HouseService
{
    public abstract class AbstractHandler
    {
        protected IContainer Container;
        public AbstractHandler()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<HouseDataService>().As<IHouseDataService>();
            Container = builder.Build();
        }
    }
}
