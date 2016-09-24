using System;
using System.Diagnostics;
using DediLib;
using Xunit;
using Xunit.Abstractions;

namespace Test.DediLib
{
    public class TestInjectionContext
    {
        private readonly ITestOutputHelper _output;

        public TestInjectionContext(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Get_InterfaceNotRegistered_ThrowsInvalidOperationException()
        {
            using (var context = new InjectionContext())
            {
                Assert.Throws<InvalidOperationException>(() => context.Get<ITestInterface>());
            }
        }

        [Fact]
        public void TryGet_InterfaceNotRegistered_Null()
        {
            using (var context = new InjectionContext())
            {
                Assert.Null(context.TryGet<ITestInterface>());
            }
        }

        [Fact]
        public void Get_InterfaceRegistered_InstanceOfRegisteredInterface()
        {
            using (var context = new InjectionContext())
            {
                context.Register<ITestInterface>(c => new TestClass());
                Assert.IsAssignableFrom<TestClass>(context.Get<ITestInterface>());
            }
        }

        [Fact]
        public void IsRegistered_InterfaceNotRegistered_False()
        {
            using (var context = new InjectionContext())
            {
                Assert.False(context.IsRegistered<ITestInterface>());
                Assert.False(context.IsRegistered(typeof(ITestInterface)));
            }
        }

        [Fact]
        public void IsRegistered_InterfaceRegistered_True()
        {
            using (var context = new InjectionContext())
            {
                context.Register<ITestInterface>(c => new TestClass());
                Assert.True(context.IsRegistered<ITestInterface>());
                Assert.True(context.IsRegistered(typeof(ITestInterface)));
            }
        }

        [Fact]
        public void IsRegistered_SingletonRegistered_True()
        {
            using (var context = new InjectionContext())
            {
                context.Singleton<ITestInterface>(c => new TestClass());
                Assert.True(context.IsRegistered<ITestInterface>());
                Assert.True(context.IsRegistered(typeof(ITestInterface)));
            }
        }

        [Fact]
        public void TryGet_InterfaceRegistered_InstanceOfRegisteredInterface()
        {
            using (var context = new InjectionContext())
            {
                context.Register<ITestInterface>(c => new TestClass());
                Assert.IsAssignableFrom<TestClass>(context.TryGet<ITestInterface>());
            }
        }

        [Fact]
        public void Get_ClassWithNonPublicConstructor_ThrowsInvalidOperationException()
        {
            using (var context = new InjectionContext())
            {
                Assert.Throws<InvalidOperationException>(() => context.Get<TestClassWithNonPublicConstructor>());
            }
        }

        [Fact]
        public void TryGet_ClassWithNonPublicConstructor_Null()
        {
            using (var context = new InjectionContext())
            {
                Assert.Null(context.TryGet<TestClassWithNonPublicConstructor>());
            }
        }

        [Fact]
        public void Get_ClassWithInterfaceConstructor_InstanceForInterfaceIsCreated()
        {
            using (var context = new InjectionContext())
            {
                context.Register<ITestInterface>(c => new TestClass());

                var instance = context.Get<TestClassWithInterfaceConstructor>();
                Assert.IsAssignableFrom<TestClass>(instance.TestInterface);
            }
        }

        [Fact]
        public void TryGet_ClassWithInterfaceConstructor_InstanceForInterfaceIsCreated()
        {
            using (var context = new InjectionContext())
            {
                context.Register<ITestInterface>(c => new TestClass());

                var instance = context.TryGet<TestClassWithInterfaceConstructor>();
                Assert.IsAssignableFrom<TestClass>(instance.TestInterface);
            }
        }

        [Fact]
        public void Get_ClassWithIInjectionContextConstructor_InjectionContextIsPassedIntoConstructor()
        {
            using (var context = new InjectionContext())
            {
                var instance = context.Get<TestClassWithIInjectionContext>();
                Assert.Same(context, instance.InjectionContext);
            }
        }

        [Fact]
        public void TryGet_ClassWithIInjectionContextConstructor_InjectionContextIsPassedIntoConstructor()
        {
            using (var context = new InjectionContext())
            {
                var instance = context.TryGet<TestClassWithIInjectionContext>();
                Assert.Same(context, instance.InjectionContext);
            }
        }

        [Fact]
        public void Register_GenericTypeNotAnInterface_InvalidOperationException()
        {
            using (var context = new InjectionContext())
            {
                Assert.Throws<InvalidOperationException>(() => context.Register(c => new TestClass()));
            }
        }

        [Fact]
        public void Register_GetTwice_DifferentInstances()
        {
            using (var context = new InjectionContext())
            {
                context.Register<ITestInterface>(c => new TestClass());

                var instance1 = context.Get<ITestInterface>();
                var instance2 = context.Get<ITestInterface>();

                Assert.IsAssignableFrom<TestClass>(instance1);
                Assert.IsAssignableFrom<TestClass>(instance2);

                Assert.NotSame(instance2, instance1);
            }
        }

        [Fact]
        public void RegisterGeneric_GetTwice_DifferentInstances()
        {
            using (var context = new InjectionContext())
            {
                context.Register<ITestInterface, TestClass>();

                var instance1 = context.Get<ITestInterface>();
                var instance2 = context.Get<ITestInterface>();

                Assert.IsAssignableFrom<TestClass>(instance1);
                Assert.IsAssignableFrom<TestClass>(instance2);

                Assert.NotSame(instance2, instance1);
            }
        }

        [Fact]
        public void Register_OverrideRegistration_LastRegisterWins()
        {
            using (var context = new InjectionContext())
            {
                context.Register<ITestInterface>(c => new TestClass());
                context.Register<ITestInterface>(c => new OtherTestClass());
                Assert.IsAssignableFrom<OtherTestClass>(context.Get<ITestInterface>());
            }
        }

        [Fact]
        public void RegisterGeneric_OverrideRegistration_LastRegisterWins()
        {
            using (var context = new InjectionContext())
            {
                context.Register<ITestInterface, TestClass>();
                context.Register<ITestInterface, OtherTestClass>();
                Assert.IsAssignableFrom<OtherTestClass>(context.Get<ITestInterface>());
            }
        }

        [Fact]
        public void Singleton_GenericTypeNotAnInterface_InvalidOperationException()
        {
            using (var context = new InjectionContext())
            {
                Assert.Throws<InvalidOperationException>(() => context.Singleton(c => new TestClass()));
            }
        }

        [Fact]
        public void Singleton_GetTwice_SameInstance()
        {
            using (var context = new InjectionContext())
            {
                context.Singleton<ITestInterface>(c => new TestClass());

                var instance1 = context.Get<ITestInterface>();
                var instance2 = context.Get<ITestInterface>();

                Assert.Same(instance2, instance1);
            }
        }

        [Fact]
        public void SingletonGeneric_GetTwice_SameInstance()
        {
            using (var context = new InjectionContext())
            {
                context.Singleton<ITestInterface, TestClass>();

                var instance1 = context.Get<ITestInterface>();
                var instance2 = context.Get<ITestInterface>();

                Assert.Same(instance2, instance1);
            }
        }

        [Fact]
        public void Singleton_OverrideRegistration_LastRegisterWins()
        {
            using (var context = new InjectionContext())
            {
                context.Singleton<ITestInterface>(c => new TestClass());
                context.Singleton<ITestInterface>(c => new OtherTestClass());
                Assert.IsAssignableFrom<OtherTestClass>(context.Get<ITestInterface>());
            }
        }

        [Fact]
        public void SingletonGeneric_OverrideRegistration_LastRegisterWins()
        {
            using (var context = new InjectionContext())
            {
                context.Singleton<ITestInterface, TestClass>();
                context.Singleton<ITestInterface, OtherTestClass>();
                Assert.IsAssignableFrom<OtherTestClass>(context.Get<ITestInterface>());
            }
        }

        [Fact]
        public void CreateScope_inherits_registered_components_from_parent_scope()
        {
            using (var context = new InjectionContext())
            {
                context.Register<ITestInterface>(c => new TestClass());

                using (var subContext = context.CreateScope())
                {
                    Assert.IsAssignableFrom<TestClass>(subContext.Get<ITestInterface>());
                }
            }
        }

        [Fact]
        public void CreateScope_child_scope_overrides_existing_parent_registration()
        {
            using (var context = new InjectionContext())
            {
                context.Register<ITestInterface>(c => new TestClass());

                using (var subContext = context.CreateScope())
                {
                    subContext.Register<ITestInterface>(c => new OtherTestClass());

                    Assert.IsAssignableFrom<OtherTestClass>(subContext.Get<ITestInterface>());
                }
            }
        }

        [Fact]
        public void CreateScope_parent_scope_changes_do_not_affect_child_scopes()
        {
            using (var context = new InjectionContext())
            {
                context.Register<ITestInterface>(c => new TestClass());

                using (var subContext = context.CreateScope())
                {
                    context.Register<ITestInterface>(c => new OtherTestClass());

                    Assert.IsAssignableFrom<OtherTestClass>(context.Get<ITestInterface>());
                    Assert.IsAssignableFrom<TestClass>(subContext.Get<ITestInterface>());
                }
            }
        }

        [Trait("Category", "Benchmark")]
        [Fact]
        public void Benchmark_RegisterGet_TestClass()
        {
            using (var context = new InjectionContext())
            {
                context.Register<ITestInterface, TestClass>();

                const int count = 10000000;
                var sw = Stopwatch.StartNew();
                for (var i = 0; i < count; i++)
                {
                    context.Get<ITestInterface>();
                }
                sw.Stop();

                var opsPerSec = count / sw.Elapsed.TotalMilliseconds * 1000;
                _output.WriteLine($"{sw.Elapsed} ({opsPerSec:N0} ops/sec)");
            }
        }

        [Trait("Category", "Benchmark")]
        [Fact]
        public void Benchmark_SingletonGet_TestClass()
        {
            using (var context = new InjectionContext())
            {
                context.Singleton<ITestInterface>(c => new TestClass());

                const int count = 10000000;
                var sw = Stopwatch.StartNew();
                for (var i = 0; i < count; i++)
                {
                    context.Get<ITestInterface>();
                }
                sw.Stop();

                var opsPerSec = count / sw.Elapsed.TotalMilliseconds * 1000;
                _output.WriteLine($"{sw.Elapsed} ({opsPerSec:N0} ops/sec)");
            }
        }

        private class TestClass : ITestInterface
        {
        }

        private class OtherTestClass : ITestInterface
        {
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private class TestClassWithNonPublicConstructor
        {
            private TestClassWithNonPublicConstructor()
            {
            }
        }

        public class TestClassWithIInjectionContext
        {
            public IInjectionContext InjectionContext { get; }

            public TestClassWithIInjectionContext(IInjectionContext injectionContext)
            {
                InjectionContext = injectionContext;
            }
        }

        public class TestClassWithInterfaceConstructor
        {
            public ITestInterface TestInterface { get; }

            public TestClassWithInterfaceConstructor(ITestInterface testInterface)
            {
                TestInterface = testInterface;
            }
        }

        public interface ITestInterface
        {
        }

        public interface ISingleInterface
        {
        }

        public class SingleClass : ISingleInterface
        {
        }

        public interface IMultipleInterface
        {
        }

        public class MultipleClass1 : IMultipleInterface
        {
        }

        public class MultipleClass2 : IMultipleInterface
        {
        }
    }
}
