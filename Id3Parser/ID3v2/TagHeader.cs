using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Id3Parser.ID3v2;

    /// <summary>
    /// Заголовок ID3v2 тега
    /// </summary>
public class TagHeader
{
        /// <summary>
        /// Возвращает байты маркера заголовка ID3v2
        /// </summary>
        public static byte[] MarkerBytes { get; } = Encoding.ASCII.GetBytes("ID3");
        /// <summary>
        /// Версия IDv2 данном документе
        /// </summary>
        public int Version { get; private set; }
        /// <summary>
        /// Подверсия IDv2 в данном документе
        /// </summary>
        public int SubVersion { get; private set; }
        /// <summary>
        /// IDv2 флаги
        /// </summary>
        public (bool unsynchronisation, bool extendedHeader, bool experimentalIndicator) Flags { get; private set; }
        /// <summary>
        /// Длина всего IDv3 в байтах
        /// </summary>
        public int Length { get; private set; }

        /// <summary>
        /// Пустой конструктор
        /// </summary>
        public TagHeader()
        {
            Version = -1;
            SubVersion = -1;
            Length = -1;
        }
        /// <summary>
        /// Конструктор на основе двоичного представления
        /// </summary>
        /// <param name="headerbytes">10 байт заголовка IDv2</param>
        public TagHeader(byte[] headerBytes)
        {
            if (!ValidateHeader(headerBytes)) throw new Exception("Not a ID3v2 header");
            Version = headerBytes[3];
            SubVersion = headerBytes[4];
            Flags = (unsynchronisation: (headerBytes[5] & 0b1000_000) != 0,
                     extendedHeader: (headerBytes[5] & 0b0100_000) != 0,
                     experimentalIndicator: (headerBytes[5] & 0b0010_000) != 0);
            Length = ParseLength(headerBytes.Skip(6).Take(4));
        }
        /// <summary>
        /// Принимает массив из 4-х байт, из них парсит длину ID3v2 тега.
        /// </summary>
        private int ParseLength(IEnumerable<byte> headerLengthBytes)
        {
            if (headerLengthBytes.Count() != 4)
                throw new ArgumentException($"IP3v2 length described by 4 bytes, but {headerLengthBytes.Count()} given.");
            int result = 0;
            for (int i = 3; i >= 0; i--)
            {
                result += (headerLengthBytes.ElementAt(i) & 0b_0111_1111) << 7 * (3-i) ;
            }
            return result;
        }
        /// <summary>
        /// Возвращает истину, если переданная последовательность байт является 
        /// валидным закодированным заголовком ID3v2
        /// </summary>
        private bool ValidateHeader(IEnumerable<byte> headerBytes)
        {
            return headerBytes.Take(3).SequenceEqual(MarkerBytes) && headerBytes.Count() == 10;
        }
        public override string ToString()
        {
            return $"ID3v2 tag, Length: {Length}";
        }
}
