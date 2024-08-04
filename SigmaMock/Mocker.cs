using System.Linq.Expressions;
using System.Reflection;

namespace SigmaMock
{
    public class Mocker<T> : DispatchProxy where T : class
    {
        private readonly static List<MethodData> _methodDataList = new();

        public T CreateInstance()
        {
            var proxy = Create<T, Mocker<T>>();
            return proxy;
        }

        public Mocker<T> SetupMethod(Expression<Func<T, object>> expression, object? returnValue, int callNumber = 1)
        {
            if (expression.Body is MethodCallExpression methodCall)
            {
                var currentRow = _methodDataList.FirstOrDefault(c => c.Name == methodCall.Method.Name);

                if (currentRow != null)
                {
                    _methodDataList.Remove(currentRow);
                }

                _methodDataList.Add(new()
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

        public Mocker<T> SetupMethod(Expression<Action<T>> expression, int callNumber = 1)
        {
            if (expression.Body is MethodCallExpression methodCall)
            {
                var currentRow = _methodDataList.FirstOrDefault(c => c.Name == methodCall.Method.Name);

                if (currentRow != null)
                {
                    _methodDataList.Remove(currentRow);
                }

                _methodDataList.Add(new()
                {
                    Name = methodCall.Method.Name,
                    CallNumberExpected = callNumber,
                    ReturnedValue = null
                });
            }
            else
            {
                throw new ArgumentException("Expression is not a method call");
            }

            return this;
        }

        public void Verify()
        {
            foreach (var method in _methodDataList)
            {
                if (method.CallNumberExpected != method.CallNumberActual &&
                    method.CallNumberExpected != -1)
                {
                    throw new Exception($"Method {method.Name} Actual {method.CallNumberActual}. Expected {method.CallNumberExpected}");
                }
            }
        }

        protected override dynamic? Invoke(MethodInfo? targetMethod, object?[]? args)
        {
            foreach (var method in _methodDataList)
            {
                if (method.Name == targetMethod?.Name)
                {
                    method.CallNumberActual++;
                    return method.ReturnedValue;
                }
            }

            return null;
        }
    }

    public class MethodData
    {
        public string Name { get; set; }
        public object? ReturnedValue { get; set; }
        public int CallNumberExpected { get; set; }
        public int CallNumberActual { get; set; }
    }
}
