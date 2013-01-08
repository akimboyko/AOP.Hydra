// Type: AOP.Hydra.PostSharp.UnitTests.InjectedAspectTests
// Assembly: AOP.Hydra.PostSharp.UnitTests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// Assembly location: D:\work\Courses\AOP.Hydra\examples\IL weaving\AOP.Hydra.PostSharp.UnitTests.dll

using AOP.Hydra.PostSharp;
using NUnit.Framework;
using PostSharp.Aspects;
using PostSharp.Aspects.Internals;
using Rhino.Mocks;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AOP.Hydra.PostSharp.UnitTests
{
  [TestFixture]
  public class InjectedAspectTests
  {
    [CompilerGenerated]
    private static System.Action<ITransaction> CS\u0024\u003C\u003E9__CachedAnonymousMethodDelegate2;

    public InjectedAspectTests()
    {
      base.\u002Ector();
    }

    [Test]
    public void InjectedAspect_Dependency_ShouldBeCommited()
    {
      MockRepository mockRepository = new MockRepository();
      ITransaction transaction = mockRepository.DynamicMock<ITransaction>(new object[0]);
      using (mockRepository.Record())
      {
        ITransaction mock = transaction;
        if (InjectedAspectTests.CS\u0024\u003C\u003E9__CachedAnonymousMethodDelegate2 == null)
        {
          // ISSUE: method pointer
          InjectedAspectTests.CS\u0024\u003C\u003E9__CachedAnonymousMethodDelegate2 = new System.Action<ITransaction>((object) null, __methodptr(\u003CInjectedAspect_Dependency_ShouldBeCommited\u003Eb__1));
        }
        System.Action<ITransaction> action = InjectedAspectTests.CS\u0024\u003C\u003E9__CachedAnonymousMethodDelegate2;
        RhinoMocksExtensions.Expect<ITransaction>(mock, action).Repeat.Once();
      }
      InjectedAspectTests.FakeService fakeService = new InjectedAspectTests.FakeService();
      fakeService.Transaction = transaction;
      fakeService.Process();
      RhinoMocksExtensions.VerifyAllExpectations((object) transaction);
    }

    [CompilerGenerated]
    private static void \u003CInjectedAspect_Dependency_ShouldBeCommited\u003Eb__1(ITransaction m)
    {
      m.Commit();
    }

    private class FakeService : IService
    {
      [CompilerGenerated]
      private ITransaction \u003CTransaction\u003Ek__BackingField;

      public ITransaction Transaction
      {
        get
        {
          return this.\u003CTransaction\u003Ek__BackingField;
        }
        set
        {
          this.\u003CTransaction\u003Ek__BackingField = value;
        }
      }

      [CompilerGenerated]
      static FakeService()
      {
        InjectedAspectTests.FakeService.\u003C\u003Ez__Aspects.Initialize();
      }

      public FakeService()
      {
        base.\u002Ector();
      }

      public void Process()
      {
        MethodInterceptionArgsImpl interceptionArgsImpl = new MethodInterceptionArgsImpl((object) this, Arguments.Empty);
        interceptionArgsImpl.Method = InjectedAspectTests.FakeService.\u003C\u003Ez__Aspects.m1;
        interceptionArgsImpl.TypedBinding = (MethodBinding) InjectedAspectTests.FakeService.\u003CProcess\u003Ec__Binding.singleton;
        InjectedAspectTests.FakeService.\u003C\u003Ez__Aspects.a0.OnInvoke((MethodInterceptionArgs) interceptionArgsImpl);
      }

      private void \u003CProcess\u003E()
      {
        Console.WriteLine("Processing...");
      }

      [CompilerGenerated]
      [DebuggerNonUserCode]
      internal sealed class \u003C\u003Ez__Aspects
      {
        internal static MethodBase m1;
        internal static readonly InjectedAspectAttribute a0;

        [CompilerGenerated]
        static \u003C\u003Ez__Aspects()
        {
          // ISSUE: method reference
          InjectedAspectTests.FakeService.\u003C\u003Ez__Aspects.m1 = MethodBase.GetMethodFromHandle(__methodref (InjectedAspectTests.FakeService.Process));
          InjectedAspectTests.FakeService.\u003C\u003Ez__Aspects.a0 = (InjectedAspectAttribute) \u003C\u003Ez__AspectsImplementationDetails1034438899.aspects1[0];
          InjectedAspectTests.FakeService.\u003C\u003Ez__Aspects.a0.RuntimeInitialize(InjectedAspectTests.FakeService.\u003C\u003Ez__Aspects.m1);
        }

        public static void Initialize()
        {
        }
      }

      [CompilerGenerated]
      private sealed class \u003CProcess\u003Ec__Binding : MethodBinding
      {
        public static InjectedAspectTests.FakeService.\u003CProcess\u003Ec__Binding singleton;

        [DebuggerNonUserCode]
        static \u003CProcess\u003Ec__Binding()
        {
          InjectedAspectTests.FakeService.\u003CProcess\u003Ec__Binding.singleton = new InjectedAspectTests.FakeService.\u003CProcess\u003Ec__Binding();
        }

        [DebuggerNonUserCode]
        private \u003CProcess\u003Ec__Binding()
        {
          base.\u002Ector();
        }

        public override void Invoke(ref object instance, Arguments arguments, object aspectArgs)
        {
          ((InjectedAspectTests.FakeService) instance).\u003CProcess\u003E();
        }
      }
    }
  }
}
