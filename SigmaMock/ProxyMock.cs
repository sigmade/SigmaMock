using System.Linq.Expressions;
using System.Reflection;

namespace SigmaMock
{
    public class ProxyMock<T> : DispatchProxy where T : class
    {
        private readonly static List<MethodData> _methodDataList = new();

        public T Create()
        {
            var proxy = Create<T, ProxyMock<T>>();
            return proxy;
        }

        public ProxyMock<T> SetupMethod(Expression<Func<T, object>> expression, object? returnValue, int callNumber = 1)
        {
            if (expression.Body is MethodCallExpression methodCall)
            {
                _methodDataList.Add(new()
                {
                    Name = methodCall.Method.Name,
                    CallNumber = callNumber,
                    ReturnedValue = returnValue
                });
            }
            else
            {
                throw new ArgumentException("Expression is not a method call");
            }

            return this;
        }

        protected override dynamic? Invoke(MethodInfo? targetMethod, object?[]? args)
        {
            var methodData = _methodDataList.Single(m => m.Name == targetMethod?.Name);

            return methodData.ReturnedValue;
        }
    }

    public class MethodData
    {
        public string Name { get; set; }
        public object? ReturnedValue { get; set; }
        public Type TypeReturnedValue { get; set; }
        public int CallNumber { get; set; }
    }
}
