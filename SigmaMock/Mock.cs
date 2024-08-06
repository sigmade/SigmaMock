using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace SigmaMock
{
    public class Mocker<T> where T : class
    {
        private MockProxy<T> proxy;
        public T Object => proxy as T;

        public Mocker()
        {
            proxy = DispatchProxy.Create<T, MockProxy<T>>() as MockProxy<T>;
        }

        public Mocker<T> SetupMethod(Expression<Func<T, object>> expression, object? returnValue, int callNumber = 1)
        {
            if (expression.Body is MethodCallExpression methodCall)
            {
                proxy.SetReturnedData(new()
                {
                    Name = methodCall.Method.Name,
                    CallNumberExpected = callNumber,
                    ReturnedValue = returnValue
                });
            }
            else
            {
                throw new ArgumentException("Expression is not a method call");
            }

            return this;
        }

        public void CheckMethodCalls()
        {
            proxy.CheckMethodCalls();
        }
    }

    public class MockProxy<T> : DispatchProxy where T : class
    {
        private ConcurrentDictionary<string, MethodData> _methodDataList = [];

        public void SetReturnedData(MethodData data)
        {
            var metRes = _methodDataList.TryGetValue(data.Name, out var methodData);

            if (metRes)
            {
                var removeRes = _methodDataList.TryRemove(data.Name, out var data2);

                if (!removeRes)
                {
                    throw new Exception();
                }
            }

            var res = _methodDataList.TryAdd(data.Name, data);

            if (!res)
            {
                throw new Exception();
            }
        }

        protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
        {
            foreach (var method in _methodDataList)
            {
                if (method.Value.Name == targetMethod?.Name)
                {
                    method.Value.CallNumberActual++;
                    return method.Value.ReturnedValue;
                }
            }

            return null;
        }

        public void CheckMethodCalls()
        {
            foreach (var method in _methodDataList)
            {
                if (method.Value.CallNumberExpected != method.Value.CallNumberActual &&
                    method.Value.CallNumberExpected != -1)
                {
                    throw new Exception($"Method {method.Value.Name} Actual {method.Value.CallNumberActual}. Expected {method.Value.CallNumberExpected}");
                }
            }
        }
    }
}
