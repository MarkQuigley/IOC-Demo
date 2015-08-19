using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace twioc
{
    //Created by Mark Quigley for code exmample.
    //Thanks in no small part to the indelible Jon Skeet

    /// <summary>
    /// Named Injection for clarity of the example. In practice this name could be confusing, or overlap. I would perfer a more unique name, ExnihiloContainer for instance.
    /// As mentioned in almost every blog/video on this subject, this container is NOT production ready. There is a possibility for looping that could lead to poor performance or 
    /// even a hung process. Looping detection, better error handeling and a more careful review of thread saftey would be needed at a minimum.
    /// </summary>
    public class Injection
    {
        //Dictionary of the registared types, and the Lifestyle choosen. Storage is for singletons only, when adding a new LSM consider seperate or combined storage depending on requirments.
        private readonly Dictionary<Type, ILifestyleManager> registrations = new Dictionary<Type, ILifestyleManager>();
        private IStorage singletonStorage = new Storage();
        private readonly object locker = new object();

        /// <summary>
        /// Registers a type to resolve to a concrete class along with the lifestye selection.
        /// Note: Thread safety is LILO, if two threads attempt to register the same type the last one will win.
        /// </summary>
        /// <typeparam name="TKey">The type to register</typeparam>
        /// <typeparam name="TConcrete">The class to resolve the type to</typeparam>
        /// <param name="lifestyle">The lifestyle for the resolved object, the default is Transient</param>
        public void Register<TKey, TConcrete>(LifestyleType lifestyle = LifestyleType.Transient) where TConcrete : TKey
        {
            //Consider evolving from a simple switch to a fully built out factory if the distinct list of LSM's grows beyond two or three
            switch (lifestyle)
            {
                case LifestyleType.Singleton:

                    registrations[typeof(TKey)] = new SingletonLifestyleManager(singletonStorage)
                    {
                        ConcreteType = typeof(TConcrete),
                        binding = () => ResolveByType(typeof(TConcrete))
                    };

                    break;

                case LifestyleType.Transient:
                default:

                    registrations[typeof(TKey)] = new TransientLifestyleManager()
                    {
                        ConcreteType = typeof(TConcrete),
                        binding = () => ResolveByType(typeof(TConcrete))
                    };

                    break;
            }
        }

        /// <summary>
        /// Resolves a non-registered type by reflection
        /// </summary>
        /// <param name="type">The concrete type to resolve</param>
        /// <returns>The resolved object</returns>
        private object ResolveByType(Type type)
        {
            try
            {
                //By convention, use the constructor with the most arguments
                var constructor = type.GetConstructors().MaxBy(x => x.GetParameters().Length);

                if (constructor != null)
                {
                    var arguments = constructor.GetParameters().Select(p => Resolve(p.ParameterType)).ToArray();
                    return constructor.Invoke(arguments);
                }
                else
                {
                    throw new Exception(string.Format("Could not resolve: {0}", type.Name));
                }
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Could not resolve: {0}", type.Name), e);
            }
        }

        /// <summary>
        /// Resolves a registered type.
        /// If the LSM is singleton the resolve will return the first created instance only.
        /// </summary>
        /// <param name="type">The type to resolve</param>
        /// <returns>The resolved object</returns>
        public object Resolve(Type type)
        {
            ILifestyleManager manager;

            if (registrations.TryGetValue(type, out manager))
            {
                if (manager.SupportsStorage)
                {
                    //First attempt at thread safety. This is likely insufficient depending on volume, and use. 
                    //TODO: implement Storage as a true singleton service with full thread saftey and maybe even lazyness
                    //See: http://csharpindepth.com/articles/general/singleton.aspx
                    lock (locker)
                    {
                        if (manager.Key == null || !manager.Storage.HasKey(manager.Key))
                        {
                            manager.Key = manager.Storage.Add(manager.binding());
                        }
                    }
                    return manager.Storage.Get(manager.Key);

                }
                else
                    return manager.binding();

            }
            else
                return ResolveByType(type);
        }

    }
}
