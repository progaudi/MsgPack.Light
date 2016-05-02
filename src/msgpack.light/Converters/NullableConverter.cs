using System;
using System.IO;

namespace MsgPack.Light.Converters
{
    public class NullableConverter<T> : IMsgPackConverter<T?>
        where T : struct
    {
        private MsgPackContext _context;

        public T? Read(IMsgPackReader reader, Func<T?> creator)
        {
            var type = reader.ReadDataType();

            if (type == DataTypes.Null)
                return null;

            var structConverter = _context.GetConverter<T>();

            Func<T> nullableCreator;
            if (creator == null)
            {
                nullableCreator = null;
            }
            else
            {
                nullableCreator = () =>
                {
                    var result = creator();
                    return result ?? default(T);
                };
            }

            reader.Seek(-1, SeekOrigin.Current);

            return structConverter.Read(reader, nullableCreator);
        }

        public void Write(T? value, IMsgPackWriter writer)
        {
            if (value.HasValue)
            {
                var valueConverter = _context.GetConverter<T>();
                valueConverter.Write(value.Value, writer);
            }
            else
            {
                _context.NullConverter.Write(null, writer);
            }
        }

        public void Initialize(MsgPackContext context)
        {
            _context = context;
        }
    }
}
