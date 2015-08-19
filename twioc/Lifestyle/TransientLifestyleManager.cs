using System;

namespace twioc
{
    public class TransientLifestyleManager : ILifestyleManager
    {
        public Func<object> binding { get; set; }
        public Type ConcreteType { get; set; }
        public bool SupportsStorage
        {
            get { return false; }
        }
        public Guid Key { get; set; }

        public IStorage Storage
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public TransientLifestyleManager() { }
    }
}
