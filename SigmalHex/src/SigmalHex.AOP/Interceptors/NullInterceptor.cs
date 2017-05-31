using Castle.DynamicProxy;

namespace SigmalHex.AOP.Interceptors
{
    /// <summary>
    /// Empty Interceptor
    /// </summary>
    public class NullInterceptor : IInterceptor
    {
        /// <summary>
        /// Intercept the method.
        /// </summary>
        /// <param name="invocation"></param>
        public void Intercept(IInvocation invocation)
        {
            invocation.Proceed();
        }
    }
}
