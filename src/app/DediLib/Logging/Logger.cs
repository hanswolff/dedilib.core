using System;

namespace DediLib.Logging
{
    public static class Logger
    {
        private static Func<Type, ILogger> _mapping = type => new NullLogger(type.FullName);
        public static Func<Type, ILogger> Mapping
        {
            get { return _mapping; }
            set { _mapping = value ?? (type => new NullLogger(type.FullName)); }
        }

        public static ILogger GetLogger(Type type)
        {
            return _mapping(type);
        }
    }
}
