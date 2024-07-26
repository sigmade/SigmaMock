using System.Linq.Expressions;
using System.Reflection;

namespace SigmaMock
{
    public class ProxyMock<T> : DispatchProxy where T : class
    {
        private readonly static Dictionary<string, object> _returnValues = new();

        public T Create()
        {
            var proxy = Create<T, ProxyMock<T>>();
            return proxy;
        }

        public ProxyMock<T> SetupReturnValue(Expression<Func<T, object>> expression, object returnValue)
        {
            if (expression.Body is MethodCallExpression methodCall)
            {
                var methodName = methodCall.Method.Name;
                _returnValues[methodName] = returnValue;
            }
            else
            {
                throw new ArgumentException("Expression is not a method call");
            }

            return this;
        }

        protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
        {
            if (_returnValues.TryGetValue(targetMethod.Name, out var returnValue))
            {
                return returnValue;
            }

            throw new Exception();
        }
    }
}
