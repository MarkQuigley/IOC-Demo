using System;

namespace twioc
{
    class SingletonLifestyleManager : ILifestyleManager
    {
        private IStorage _storage;

        public Func<object> binding { get; set; }
        public Type ConcreteType { get; set; }
        public bool SupportsStorage { get { return true; } }
        public IStorage Storage { get { return _storage; } }
        public Guid Key { get; set; }

        public SingletonLifestyleManager(IStorage storage)
        {
            _storage = storage;
        }
    }
}
