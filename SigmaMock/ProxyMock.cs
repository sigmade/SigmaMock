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
