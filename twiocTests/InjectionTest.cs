using NUnit.Framework;
using twioc.Tests.TestClasses;

namespace twioc.Tests
{
    [TestFixture]
    public class InjectionTest
    {
        [Test]
        public void RegisterType_Transient()
        {
            var injector = new Injection();

            injector.Register<ITimer, MyTimer>(LifestyleType.Transient);

            var actual1 = injector.Resolve(typeof(ITimer));
            var actual2 = injector.Resolve(typeof(ITimer));

            Assert.IsInstanceOf<ITimer>(actual1);
            Assert.IsInstanceOf<ITimer>(actual2);
            Assert.AreNotSame(actual1, actual2);
        }

        [Test]
        public void RegisterType_Singleton()
        {
            var injector = new Injection();

            injector.Register<ITimer, MyTimer>(LifestyleType.Singleton);

            var actual1 = injector.Resolve(typeof(ITimer));
            var actual2 = injector.Resolve(typeof(ITimer));

            Assert.IsInstanceOf<ITimer>(actual1);
            Assert.IsInstanceOf<ITimer>(actual2);
            Assert.AreSame(actual1, actual2);
        }

        [Test]
        public void RegisterComplexType_Transient()
        {
            var injector = new Injection();

            injector.Register<ITimer, MyTimer>(LifestyleType.Transient);
            //The Injector container will handel unregistered resolves but the 
            //following line could be uncommented to officially register the "Complex" type.
            //injector.Register<IComplex, Complex>(LifestyleType.Transient);

            var actual1 = injector.Resolve(typeof(Complex));
            var actual2 = injector.Resolve(typeof(Complex));

            Assert.IsInstanceOf<IComplex>(actual1);
            Assert.IsInstanceOf<IComplex>(actual2);
            Assert.AreNotSame(actual1, actual2);
        }

        [Test]
        public void RegisterComplexType_Singleton()
        {
            var injector = new Injection();

            injector.Register<ITimer, MyTimer>(LifestyleType.Transient);
            //Here we fully register the complex type to utilize the container LSM for singleton
            injector.Register<IComplex, Complex>(LifestyleType.Singleton);

            var actual1 = injector.Resolve(typeof(IComplex));
            var actual2 = injector.Resolve(typeof(IComplex));

            Assert.IsInstanceOf<IComplex>(actual1);
            Assert.IsInstanceOf<IComplex>(actual2);
            Assert.AreSame(actual1, actual2);
        }

        [Test]
        public void RegisterComplexType_TransientWithSingletonDependency()
        {
            var injector = new Injection();

            injector.Register<ITimer, MyTimer>(LifestyleType.Singleton);
            //The Injector container will handel unregistered resolves but the 
            //following line could be uncommented to officially register the "Complex" type.
            //injector.Register<IComplex, Complex>(LifestyleType.Transient);

            var actual1 = (Complex)injector.Resolve(typeof(Complex));
            var actual2 = (Complex)injector.Resolve(typeof(Complex));

            Assert.AreNotSame(actual1, actual2);
            Assert.AreSame(actual1.Timer, actual2.Timer);
        }

        //No test for RegisterComplexType_SingletonWithTransientDependency because
        //the signleton nature of the complex type will mean we only have a single 
        //instance of all injected objects as well.

        //I could build a test that registered complex type A, and complex type B 
        //each with the same singleton dependency, but I didn't want to clutter the 
        //project at this point.
    }
}
