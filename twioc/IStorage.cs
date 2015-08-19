using System;

namespace twioc
{
    public interface IStorage
    {
        Guid Add(object obj);
        void Remove(Guid key);
        object Get(Guid key);
        bool HasKey(Guid key);
    }
}
