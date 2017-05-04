using System;

namespace ProGaudi.MsgPack.Light.Tests.Generators
{
    internal class ImageInfoArrayConverter : IMsgPackConverter<IImageInfo>, IMsgPackConverter<ImageInfo>, IMsgPackConverter<BigImageInfo>, IMsgPackConverter<IMegaImageInfo>
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
            var nullable = reader.ReadArrayLength();
            if (!nullable.HasValue)
                return null;

            ReadImageInfoBody(reader, imageInfo);
            return imageInfo;
        }

        private void ReadImageInfoBody(IMsgPackReader reader, ImageInfo imageInfo)
        {
            imageInfo.Width = _intConverter.Value.Read(reader);
            imageInfo.Height = _intConverter.Value.Read(reader);
            imageInfo.Link = _stringConverter.Value.Read(reader);
            imageInfo.Credits = _stringConverter.Value.Read(reader);
        }

        private void WriteImplementation(IImageInfo value, IMsgPackWriter writer)
        {
            if (value == null)
            {
                writer.Write(DataTypes.Null);
            }
            else
            {
                writer.WriteArrayHeader(4U);
                WriteImageInfoBody(value, writer);
            }
        }

        private void WriteImageInfoBody(IImageInfo value, IMsgPackWriter writer)
        {
            _intConverter.Value.Write(value.Width, writer);
            _intConverter.Value.Write(value.Height, writer);
            _stringConverter.Value.Write(value.Link, writer);
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
                writer.WriteArrayHeader(5U);

                WriteImageInfoBody(value, writer);
                _intConverter.Value.Write(value.Size, writer);
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
                writer.WriteArrayHeader(5U);

                WriteImageInfoBody(value, writer);
                _dateTimeConverter.Value.Write(value.SomeDate, writer);
            }
        }

        IMegaImageInfo IMsgPackConverter<IMegaImageInfo>.Read(IMsgPackReader reader)
        {
            var imageInfo = new MegaImageInfo();
            var nullable = reader.ReadArrayLength();
            if (!nullable.HasValue)
                return null;

            ReadImageInfoBody(reader, imageInfo);
            imageInfo.SomeDate = _dateTimeConverter.Value.Read(reader);

            return imageInfo;
        }

        public BigImageInfo Read(IMsgPackReader reader)
        {
            var imageInfo = new BigImageInfo();
            var nullable = reader.ReadArrayLength();
            if (!nullable.HasValue)
                return null;
            ReadImageInfoBody(reader, imageInfo);
            imageInfo.Size = _intConverter.Value.Read(reader);

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