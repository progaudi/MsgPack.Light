using System;

using Moq;

namespace ProGaudi.MsgPack.Light.Tests.Generators
{
    internal class ImageInfoConverter : IMsgPackConverter<IImageInfo>, IMsgPackConverter<ImageInfo>, IMsgPackConverter<BigImageInfo>, IMsgPackConverter<IMegaImageInfo>
    {
        private Lazy<IMsgPackConverter<int>> _intConverter;
        private Lazy<IMsgPackConverter<DateTime>> _dateTimeConverter;
        private Lazy<IMsgPackConverter<string>> _stringConverter;

        public void Initialize(MsgPackContext context)
        {
            _intConverter = new Lazy<IMsgPackConverter<int>>(context.GetConverter<int>);
            _dateTimeConverter = new Lazy<IMsgPackConverter<DateTime>>(context.GetConverter<DateTime>);
            _stringConverter = new Lazy<IMsgPackConverter<string>>(context.GetConverter<string>);
        }

        private ImageInfo ReadImplementation(IMsgPackReader reader)
        {
            var imageInfo = new ImageInfo();
            var nullable = reader.ReadMapLength();
            if (!nullable.HasValue)
                return null;
            for (var index = 0; index < nullable.Value; ++index)
            {
                var str = _stringConverter.Value.Read(reader);
                if (!ReadImageInfoBody(str, imageInfo, reader))
                {
                    reader.SkipToken();
                }
            }
            return imageInfo;
        }

        private bool ReadImageInfoBody(string propertyName, ImageInfo imageInfo, IMsgPackReader reader)
        {
            switch (propertyName)
            {
                case "Width":
                    imageInfo.Width = _intConverter.Value.Read(reader);
                    return true;
                case "Height":
                    imageInfo.Height = _intConverter.Value.Read(reader);
                    return true;
                case "Link":
                    imageInfo.Link = _stringConverter.Value.Read(reader);
                    return true;
                case "Credits":
                    imageInfo.Credits = _stringConverter.Value.Read(reader);
                    return true;
                default:
                    return false;
            }
        }

        private void WriteImplementation(IImageInfo value, IMsgPackWriter writer)
        {
            if (value == null)
            {
                writer.Write(DataTypes.Null);
            }
            else
            {
                writer.WriteMapHeader(4U);
                WriteImageInfoBody(value, writer);
            }
        }

        private void WriteImageInfoBody(IImageInfo value, IMsgPackWriter writer)
        {
            _stringConverter.Value.Write("Width", writer);
            _intConverter.Value.Write(value.Width, writer);
            _stringConverter.Value.Write("Height", writer);
            _intConverter.Value.Write(value.Height, writer);
            _stringConverter.Value.Write("Link", writer);
            _stringConverter.Value.Write(value.Link, writer);
            _stringConverter.Value.Write("Credits", writer);
            _stringConverter.Value.Write(value.Credits, writer);
        }

        void IMsgPackConverter<IImageInfo>.Write(IImageInfo value, IMsgPackWriter writer)
        {
            WriteImplementation(value, writer);
        }

        void IMsgPackConverter<ImageInfo>.Write(ImageInfo value, IMsgPackWriter writer)
        {
            WriteImplementation(value, writer);
        }

        public void Write(BigImageInfo value, IMsgPackWriter writer)
        {
            if (value == null)
            {
                writer.Write(DataTypes.Null);
            }
            else
            {
                writer.WriteMapHeader(5U);

                _stringConverter.Value.Write("Size2", writer);
                _intConverter.Value.Write(value.Size, writer);

                WriteImageInfoBody(value, writer);
            }
        }

        public void Write(IMegaImageInfo value, IMsgPackWriter writer)
        {
            if (value == null)
            {
                writer.Write(DataTypes.Null);
            }
            else
            {
                writer.WriteMapHeader(5U);

                _stringConverter.Value.Write("CreationDate", writer);
                _dateTimeConverter.Value.Write(value.SomeDate, writer);

                WriteImageInfoBody(value, writer);
            }
        }

        IMegaImageInfo IMsgPackConverter<IMegaImageInfo>.Read(IMsgPackReader reader)
        {
            var imageInfo = new MegaImageInfo();
            var nullable = reader.ReadMapLength();
            if (!nullable.HasValue)
                return null;
            for (var index = 0; index < nullable.Value; ++index)
            {
                var str = _stringConverter.Value.Read(reader);
                if (ReadImageInfoBody(str, imageInfo, reader))
                {
                    continue;
                }

                if (str == "CreationDate")
                {
                    imageInfo.SomeDate = _dateTimeConverter.Value.Read(reader);
                }
                else
                {
                    reader.SkipToken();
                }
            }

            return imageInfo;
        }

        public BigImageInfo Read(IMsgPackReader reader)
        {
            var imageInfo = new BigImageInfo();
            var nullable = reader.ReadMapLength();
            if (!nullable.HasValue)
                return null;
            for (var index = 0; index < nullable.Value; ++index)
            {
                var str = _stringConverter.Value.Read(reader);
                if (ReadImageInfoBody(str, imageInfo, reader))
                {
                    continue;
                }

                if (str == "Size2")
                {
                    imageInfo.Size = _intConverter.Value.Read(reader);
                }
                else
                {
                    reader.SkipToken();
                }
            }

            return imageInfo;
        }

        IImageInfo IMsgPackConverter<IImageInfo>.Read(IMsgPackReader reader)
        {
            return ReadImplementation(reader);
        }

        ImageInfo IMsgPackConverter<ImageInfo>.Read(IMsgPackReader reader)
        {
            return ReadImplementation(reader);
        }
    }
}