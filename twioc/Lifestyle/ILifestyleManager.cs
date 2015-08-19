using System;

namespace twioc
{
    interface ILifestyleManager
    {
        Type ConcreteType { get; set; }
        Func<object> binding { get; set; }
        bool SupportsStorage { get; }
        Guid Key { get; set; }
        IStorage Storage { get; }
    }


}

