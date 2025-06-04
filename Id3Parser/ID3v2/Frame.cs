using System;
using System.Collections.Generic;
using System.Text;

namespace Id3Parser.ID3v2;

public class Frame
{
        public FrameHeader Header { get; protected set; }
        public ICollection<byte> RawBytes { get; protected set; }
        public string RawString { get; protected set; }
        /// <summary>
        /// Полный размер фрейма в байтах, включая 10 байт заголовка фрейма
        /// </summary>
        public int Length { get => Header.Length + 10; }
        public string FrameID { get => Header.FrameID; }
        public virtual string Value {
            get => null;
            protected set => throw new NotImplementedException("Requires implementation in children class"); }
        public Frame(FrameHeader header, byte[] valueBytes)
        {
            Header = header;
            RawBytes = valueBytes;
            RawString = Encoding.ASCII.GetString(valueBytes);
        }
        public override string ToString()
        {
            return $"{FrameID}: {RawString}";
        }
}
