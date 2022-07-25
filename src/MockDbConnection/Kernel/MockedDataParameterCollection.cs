using System.Collections;
using System.Data.Common;

namespace MockDBConnection.Kernel
{
    internal class MockedDataParameterCollection : DbParameterCollection
    {
        private readonly object _sync = string.Empty;

        private readonly List<DbParameter> _parameters = new();

        public override int Count => _parameters.Count;

        public override object SyncRoot => _sync;

        public override int Add(object value)
        {
            _parameters.Add(ToParam(value));
            return _parameters.IndexOf(ToParam(value));
        }

        public override void AddRange(Array values)
        {
            foreach (var item in values)
            {
                _parameters.Add(ToParam(item));
            }
        }

        public override void Clear()
        {
            _parameters.Clear();
        }

        public override bool Contains(object value)
        {
            return _parameters.Contains(value);
        }

        public override bool Contains(string value)
        {
            return _parameters.FirstOrDefault(p => p.ParameterName == value) != null;
        }

        public override void CopyTo(Array array, int index)
        {
            _parameters.CopyTo(array.OfType<DbParameter>().ToArray(), index);
        }

        public override IEnumerator GetEnumerator()
        {
            return _parameters.GetEnumerator();
        }

        public override int IndexOf(object value)
        {
            return _parameters.IndexOf(ToParam(value));
        }

        public override int IndexOf(string parameterName)
        {
            var item = _parameters.FirstOrDefault(p => p.ParameterName == parameterName);
            if (item != null)
            {
                return _parameters.IndexOf(item);
            }
            return -1;

        }

        public override void Insert(int index, object value)
        {
            _parameters.Insert(index, ToParam(value));
        }

        public override void Remove(object value)
        {
            _parameters.Remove(ToParam(value));
        }

        public override void RemoveAt(int index)
        {
            _parameters.RemoveAt(index);
        }

        public override void RemoveAt(string parameterName)
        {
            _parameters.RemoveAll(p => p.ParameterName == parameterName);
        }

        protected override DbParameter GetParameter(int index)
        {
            return _parameters[index];
        }

        protected override DbParameter GetParameter(string parameterName)
        {
            return _parameters.First(p => p.ParameterName == parameterName);
        }

        protected override void SetParameter(int index, DbParameter value)
        {
            _parameters[index] = value;
        }

        protected override void SetParameter(string parameterName, DbParameter value)
        {
            var item = _parameters.First(p => p.ParameterName == parameterName);
            var idx = _parameters.IndexOf(item);
            _parameters[idx] = value;
        }

        private static DbParameter ToParam(object value)
        {
            return (DbParameter)value;
        }
    }
}