using KidMartStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidMartStore.Patterns.Singleton
{
    public sealed class DatabaseContextSingleton
    {
        private static KidMartStoreEntities _instance;
        private static readonly object _lock = new object();

        private DatabaseContextSingleton() { }

        public static KidMartStoreEntities Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)  // Đảm bảo chỉ một luồng có thể khởi tạo instance
                    {
                        if (_instance == null)
                        {
                            _instance = new KidMartStoreEntities();
                        }
                    }
                }
                return _instance;
            }
        }
    }
}