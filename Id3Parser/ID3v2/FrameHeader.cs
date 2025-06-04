using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Id3Parser.ID3v2;

    /// <summary>
    /// Заголовок фрейма IDv2 тега
    /// </summary>
    public class FrameHeader
    {
        /// <summary>
        /// Строковой идентификатор кадра
        /// </summary>
        public string FrameID { get; private set; }
        /// <summary>
        /// Длина кадра без учёта 10 байт заголовка кадра
        /// </summary>
        public int Length { get; private set; }
        /// <summary>
        /// Флаги кадра
        /// </summary>
        public (
            bool preservation, 
            bool alterPreservation, 
            bool readOnly,
            bool compression,
            bool encryption,
            bool groupingIdentity
            ) 
            Flags { get; private set; }
        public FrameHeader(byte[] frameHeaderBytes)
        {
            if (!ValidateFrameHeader(frameHeaderBytes)) throw new FormatException("Not a ID3v2 header");
            FrameID = Encoding.ASCII.GetString(frameHeaderBytes.Take(4).ToArray());
            Length = ParseLength(frameHeaderBytes.Skip(4).Take(4));

            Flags = (preservation: (frameHeaderBytes[4] & 0b1000_000) != 0,
                     alterPreservation: (frameHeaderBytes[4] & 0b0100_000) != 0,
                     readOnly: (frameHeaderBytes[4] & 0b0010_000) != 0,
                     compression: (frameHeaderBytes[5] & 0b1000_000) != 0,
                     encryption: (frameHeaderBytes[5] & 0b0100_000) != 0,
                     groupingIdentity: (frameHeaderBytes[5] & 0b0010_000) != 0
                     );
        }
        /// <summary>
        /// Валидирует заголовк фрейма
        /// </summary>
        /// <remarks>
        /// Проверяет размер переданной последовательности (должна быть 10) и
        /// идентифакто тега (должен состоять из цифр и заглавных латинских символов.
        /// </remarks>
        /// <returns>Возвращает истину, если заголовк валиден</returns>
        private bool ValidateFrameHeader(byte[] frameHeaderBytes)
        {
            return frameHeaderBytes.Count() == 10 &&
                   frameHeaderBytes.Take(4).All(b => (b >= 48 && b <= 57) || (b >= 65 && b <= 90));
        }

        private int ParseLength(IEnumerable<byte> headerLengthBytes)
        {
            if (headerLengthBytes.Count() != 4)
                throw new ArgumentException($"IP3v2 frame length described by 4 bytes, but {headerLengthBytes.Count()} given.");
            int result = 0;
            for (int i = 3; i >= 0; i--)
            {
                result += (headerLengthBytes.ElementAt(i)) << 7 * (3 - i);
            }
            return result;
        }
        public override string ToString()
        {
            return $"{FrameID}, Length: {Length}";
        }
    }
