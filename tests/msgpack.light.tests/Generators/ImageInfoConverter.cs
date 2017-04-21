using System;

namespace ProGaudi.MsgPack.Light.Tests.Generators
{
    internal class ImageInfoConverter : IMsgPackConverter<IImageInfo>, IMsgPackConverter<ImageInfo>
    {
        private Lazy<IMsgPackConverter<int>> _intConverter;
        private Lazy<IMsgPackConverter<string>> _stringConverter;

        public void Initialize(MsgPackContext context)
        {
            _intConverter = new Lazy<IMsgPackConverter<int>>(context.GetConverter<int>);
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
                switch (str)
                {
                    case "Width":
                        imageInfo.Width = _intConverter.Value.Read(reader);
                        break;
                    case "Height":
                        imageInfo.Height = _intConverter.Value.Read(reader);
                        break;
                    case "Link":
                        imageInfo.Link = _stringConverter.Value.Read(reader);
                        break;
                    case "Credits":
                        imageInfo.Credits = _stringConverter.Value.Read(reader);
                        break;
                    default:
                        reader.SkipToken();
                        break;
                }
            }
            return imageInfo;
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
                _stringConverter.Value.Write("Width", writer);
                _intConverter.Value.Write(value.Width, writer);
                _stringConverter.Value.Write("Height", writer);
                _intConverter.Value.Write(value.Height, writer);
                _stringConverter.Value.Write("Link", writer);
                _stringConverter.Value.Write(value.Link, writer);
                _stringConverter.Value.Write("Credits", writer);
                _stringConverter.Value.Write(value.Credits, writer);
            }
        }

        void IMsgPackConverter<IImageInfo>.Write(IImageInfo value, IMsgPackWriter writer)
        {
            WriteImplementation(value, writer);
        }

        void IMsgPackConverter<ImageInfo>.Write(ImageInfo value, IMsgPackWriter writer)
        {
            WriteImplementation(value, writer);
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