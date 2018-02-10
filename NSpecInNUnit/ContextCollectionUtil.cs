using System;
using System.Linq;
using System.Threading.Tasks;
using NSpec;
using NSpec.Domain;

namespace NSpecInNUnit
{
    internal static class ContextCollectionUtil
    {
        internal static void PatchBeforeAllsToRunOnce(ContextCollection testSuite)
        {
            var rootContext = testSuite.SingleOrDefault();
            if (rootContext == null) throw new Exception("Failed to identify the root context");
            PatchBeforeAllsToRunOnce(rootContext);
        }

        private static void PatchBeforeAllsToRunOnce(Context context)
        {
            foreach (var subContext in context.Contexts) PatchBeforeAllsToRunOnce(subContext);
            var hook1 = context.BeforeAll;
            if (hook1 != null)
            {
                context.BeforeAll = InvokeOnce(hook1);
            }
            var hook2 = context.BeforeAllInstance;
            if (hook2 != null)
            {
                context.BeforeAllChain.SetProtectedProperty(_ => _.ClassHook, InvokeOnce(hook2));
            }
            var hook3 = context.BeforeAllInstanceAsync;
            if (hook3 != null)
            {
                context.BeforeAllChain.SetProtectedProperty(_ => _.AsyncClassHook, InvokeOnce(hook3));
            }
            var hook4 = context.BeforeAllAsync;
            if (hook4 != null)
            {
                var hasRun = false;
                context.BeforeAllAsync = () =>
                {
                    if (hasRun) return Task.Delay(0);
                    hasRun = true;
                    return hook4();
                };
            }
        }
        
        internal static void DeferAfterAlls(ContextCollection testSuite, Action<Action<nspec>> collector)
        {
            var rootContext = testSuite.SingleOrDefault();
            if (rootContext == null) throw new Exception("Failed to identify the root context");
            DeferAfterAll(rootContext, collector);
        }

        private static void DeferAfterAll(Context context, Action<Action<nspec>> collector)
        {
            foreach (var subContext in context.Contexts) DeferAfterAll(subContext, collector);
            var hook1 = context.AfterAll;
            if (hook1 != null)
            {
                context.AfterAll = InvokeOnce(() =>
                {
                    collector(_ => hook1());
                });
            }
            var hook2 = context.AfterAllInstance;
            if (hook2 != null)
            {
                context.AfterAllChain.SetProtectedProperty(_ => _.ClassHook, InvokeOnce(nspec =>
                {
                    collector(hook2);
                }));
            }

            var hook3 = context.AfterAllInstanceAsync;
            if (hook3 != null)
            {
                context.AfterAllChain.SetProtectedProperty(_ => _.AsyncClassHook, InvokeOnce(nspec =>
                {
                    collector(hook3);
                }));
            }

            var hook4 = context.AfterAllAsync;
            if (hook4 != null)
            {
                var hasRun = false;
                context.AfterAllAsync = () =>
                {
                    if (!hasRun)
                    {
                        hasRun = true;
                        collector(_ => hook4().Wait());
                    }
                    return Task.Delay(0);
                };
            }
        }

        
        private static Action<nspec> InvokeOnce(Action<nspec> a)
        {
            var hasRun = false;
            return nspec =>
            {
                if (hasRun) return;
                hasRun = true;
                a(nspec);
            };
        }
        private static Action InvokeOnce(Action a)
        {
            var hasRun = false;
            return () =>
            {
                if (hasRun) return;
                hasRun = true;
                a();
            };
        }
    }
}