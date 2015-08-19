using System;
using System.Collections.Generic;

namespace twioc
{
    public class Storage : IStorage
    {
        private Dictionary<Guid, Object> safe = new Dictionary<Guid, object>();

        public Guid Add(object obj)
        {
            var key = Guid.NewGuid();

            safe.Add(key, obj);

            return key;
        }

        public object Get(Guid key)
        {
            return safe[key];
        }

        public bool HasKey(Guid key)
        {
            return safe.ContainsKey(key);
        }

        public void Remove(Guid key)
        {
            safe.Remove(key);
        }
    }
}
