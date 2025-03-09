using KidMartStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KidMartStore.Controllers
{
    public sealed class DbContextSingleton
    {
        private static readonly Lazy<KidMartStoreEntities> instance =
            new Lazy<KidMartStoreEntities>(() => new KidMartStoreEntities());

        private DbContextSingleton() { }

        public static KidMartStoreEntities Instance => instance.Value;
    }
}