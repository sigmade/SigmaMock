using System.Reflection;

namespace SigmaMock
{
    public class ProxyMock<T> : DispatchProxy where T : class
    {
        private static Dictionary<string, object> _returnValues = new();

        public T Create()
        {
            var proxy = Create<T, ProxyMock<T>>() as ProxyMock<T>;
            return proxy as T;
        }

        public ProxyMock<T> SetupReturnValue(string methodName, object returnValue)
        {
            _returnValues[methodName] = returnValue;
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
