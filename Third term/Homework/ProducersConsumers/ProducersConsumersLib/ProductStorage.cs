using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;


namespace ProducersConsumers
{
    public class ProductStorage
    {
        Semaphore semaphore;
        private List<Action> storage;

        public ProductStorage()
        {
            semaphore = new Semaphore(1, 1);
            storage = new List<Action>();
        }

        public void TakeProduct(Action action)
        {
            semaphore.WaitOne();
            storage.Add(action);
            semaphore.Release();

        }

        public Action SendProduct()
        {
            Action action = null;
            semaphore.WaitOne();
            if (storage.Count > 0)
            {
                action = storage.First();
                storage.RemoveAt(0);
            }
            semaphore.Release();
            return action;
        }
    }
}
