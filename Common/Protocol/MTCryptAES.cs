//+------------------------------------------------------------------+
//|                                             MetaTrader 5 Web API |
//|                             Copyright 2000-2021, MetaQuotes Ltd. |
//|                                               www.metaquotes.net |
//+------------------------------------------------------------------+
using MetaQuotes.MT5WebAPI.Common.Utils;
//---
namespace MetaQuotes.MT5WebAPI.Common.Protocol
  {
  class MTCryptAES
    {
    //--- The number of 32-bit words comprising the plaintext and columns comrising the state matrix of an AES cipher.
    private const int M_NB = 4;
    // The number of 32-bit words comprising the cipher key in this AES cipher.
    private int m_Nk;
    //--- The number of rounds in this AES cipher.
    private int m_Nr;
    //--- The S-Box substitution table.
    private static readonly byte[] m_Sbox = new byte[]
                                              {
                                                      0x63, 0x7c, 0x77, 0x7b, 0xf2, 0x6b, 0x6f, 0xc5, 0x30, 0x01, 0x67, 0x2b,
                                                      0xfe, 0xd7, 0xab, 0x76, 0xca, 0x82, 0xc9, 0x7d, 0xfa, 0x59, 0x47, 0xf0,
                                                      0xad, 0xd4, 0xa2, 0xaf, 0x9c, 0xa4, 0x72, 0xc0, 0xb7, 0xfd, 0x93, 0x26,
                                                      0x36, 0x3f, 0xf7, 0xcc, 0x34, 0xa5, 0xe5, 0xf1, 0x71, 0xd8, 0x31, 0x15,
                                                      0x04, 0xc7, 0x23, 0xc3, 0x18, 0x96, 0x05, 0x9a, 0x07, 0x12, 0x80, 0xe2,
                                                      0xeb, 0x27, 0xb2, 0x75, 0x09, 0x83, 0x2c, 0x1a, 0x1b, 0x6e, 0x5a, 0xa0,
                                                      0x52, 0x3b, 0xd6, 0xb3, 0x29, 0xe3, 0x2f, 0x84, 0x53, 0xd1, 0x00, 0xed,
                                                      0x20, 0xfc, 0xb1, 0x5b, 0x6a, 0xcb, 0xbe, 0x39, 0x4a, 0x4c, 0x58, 0xcf,
                                                      0xd0, 0xef, 0xaa, 0xfb, 0x43, 0x4d, 0x33, 0x85, 0x45, 0xf9, 0x02, 0x7f,
                                                      0x50, 0x3c, 0x9f, 0xa8, 0x51, 0xa3, 0x40, 0x8f, 0x92, 0x9d, 0x38, 0xf5,
                                                      0xbc, 0xb6, 0xda, 0x21, 0x10, 0xff, 0xf3, 0xd2, 0xcd, 0x0c, 0x13, 0xec,
                                                      0x5f, 0x97, 0x44, 0x17, 0xc4, 0xa7, 0x7e, 0x3d, 0x64, 0x5d, 0x19, 0x73,
                                                      0x60, 0x81, 0x4f, 0xdc, 0x22, 0x2a, 0x90, 0x88, 0x46, 0xee, 0xb8, 0x14,
                                                      0xde, 0x5e, 0x0b, 0xdb, 0xe0, 0x32, 0x3a, 0x0a, 0x49, 0x06, 0x24, 0x5c,
                                                      0xc2, 0xd3, 0xac, 0x62, 0x91, 0x95, 0xe4, 0x79, 0xe7, 0xc8, 0x37, 0x6d,
                                                      0x8d, 0xd5, 0x4e, 0xa9, 0x6c, 0x56, 0xf4, 0xea, 0x65, 0x7a, 0xae, 0x08,
                                                      0xba, 0x78, 0x25, 0x2e, 0x1c, 0xa6, 0xb4, 0xc6, 0xe8, 0xdd, 0x74, 0x1f,
                                                      0x4b, 0xbd, 0x8b, 0x8a, 0x70, 0x3e, 0xb5, 0x66, 0x48, 0x03, 0xf6, 0x0e,
                                                      0x61, 0x35, 0x57, 0xb9, 0x86, 0xc1, 0x1d, 0x9e, 0xe1, 0xf8, 0x98, 0x11,
                                                      0x69, 0xd9, 0x8e, 0x94, 0x9b, 0x1e, 0x87, 0xe9, 0xce, 0x55, 0x28, 0xdf,
                                                      0x8c, 0xa1, 0x89, 0x0d, 0xbf, 0xe6, 0x42, 0x68, 0x41, 0x99, 0x2d, 0x0f,
                                                      0xb0, 0x54, 0xbb, 0x16
                                              };
    // MTLog table based on 0xe5
    private static readonly byte[] m_Ltable = new byte[]
                                                {
                                                        0x00, 0xff, 0xc8, 0x08, 0x91, 0x10, 0xd0, 0x36, 0x5a, 0x3e, 0xd8, 0x43,
                                                        0x99, 0x77, 0xfe, 0x18, 0x23, 0x20, 0x07, 0x70, 0xa1, 0x6c, 0x0c, 0x7f,
                                                        0x62, 0x8b, 0x40, 0x46, 0xc7, 0x4b, 0xe0, 0x0e, 0xeb, 0x16, 0xe8, 0xad,
                                                        0xcf, 0xcd, 0x39, 0x53, 0x6a, 0x27, 0x35, 0x93, 0xd4, 0x4e, 0x48, 0xc3,
                                                        0x2b, 0x79, 0x54, 0x28, 0x09, 0x78, 0x0f, 0x21, 0x90, 0x87, 0x14, 0x2a,
                                                        0xa9, 0x9c, 0xd6, 0x74, 0xb4, 0x7c, 0xde, 0xed, 0xb1, 0x86, 0x76, 0xa4,
                                                        0x98, 0xe2, 0x96, 0x8f, 0x02, 0x32, 0x1c, 0xc1, 0x33, 0xee, 0xef, 0x81,
                                                        0xfd, 0x30, 0x5c, 0x13, 0x9d, 0x29, 0x17, 0xc4, 0x11, 0x44, 0x8c, 0x80,
                                                        0xf3, 0x73, 0x42, 0x1e, 0x1d, 0xb5, 0xf0, 0x12, 0xd1, 0x5b, 0x41, 0xa2,
                                                        0xd7, 0x2c, 0xe9, 0xd5, 0x59, 0xcb, 0x50, 0xa8, 0xdc, 0xfc, 0xf2, 0x56,
                                                        0x72, 0xa6, 0x65, 0x2f, 0x9f, 0x9b, 0x3d, 0xba, 0x7d, 0xc2, 0x45, 0x82,
                                                        0xa7, 0x57, 0xb6, 0xa3, 0x7a, 0x75, 0x4f, 0xae, 0x3f, 0x37, 0x6d, 0x47,
                                                        0x61, 0xbe, 0xab, 0xd3, 0x5f, 0xb0, 0x58, 0xaf, 0xca, 0x5e, 0xfa, 0x85,
                                                        0xe4, 0x4d, 0x8a, 0x05, 0xfb, 0x60, 0xb7, 0x7b, 0xb8, 0x26, 0x4a, 0x67,
                                                        0xc6, 0x1a, 0xf8, 0x69, 0x25, 0xb3, 0xdb, 0xbd, 0x66, 0xdd, 0xf1, 0xd2,
                                                        0xdf, 0x03, 0x8d, 0x34, 0xd9, 0x92, 0x0d, 0x63, 0x55, 0xaa, 0x49, 0xec,
                                                        0xbc, 0x95, 0x3c, 0x84, 0x0b, 0xf5, 0xe6, 0xe7, 0xe5, 0xac, 0x7e, 0x6e,
                                                        0xb9, 0xf9, 0xda, 0x8e, 0x9a, 0xc9, 0x24, 0xe1, 0x0a, 0x15, 0x6b, 0x3a,
                                                        0xa0, 0x51, 0xf4, 0xea, 0xb2, 0x97, 0x9e, 0x5d, 0x22, 0x88, 0x94, 0xce,
                                                        0x19, 0x01, 0x71, 0x4c, 0xa5, 0xe3, 0xc5, 0x31, 0xbb, 0xcc, 0x1f, 0x2d,
                                                        0x3b, 0x52, 0x6f, 0xf6, 0x2e, 0x89, 0xf7, 0xc0, 0x68, 0x1b, 0x64, 0x04,
                                                        0x06, 0xbf, 0x83, 0x38
                                                };
    //--- Inverse log table
    private static readonly byte[] m_Atable = new byte[]
                                                {
                                                        0x01, 0xe5, 0x4c, 0xb5, 0xfb, 0x9f, 0xfc, 0x12, 0x03, 0x34, 0xd4, 0xc4,
                                                        0x16, 0xba, 0x1f, 0x36, 0x05, 0x5c, 0x67, 0x57, 0x3a, 0xd5, 0x21, 0x5a,
                                                        0x0f, 0xe4, 0xa9, 0xf9, 0x4e, 0x64, 0x63, 0xee, 0x11, 0x37, 0xe0, 0x10,
                                                        0xd2, 0xac, 0xa5, 0x29, 0x33, 0x59, 0x3b, 0x30, 0x6d, 0xef, 0xf4, 0x7b,
                                                        0x55, 0xeb, 0x4d, 0x50, 0xb7, 0x2a, 0x07, 0x8d, 0xff, 0x26, 0xd7, 0xf0,
                                                        0xc2, 0x7e, 0x09, 0x8c, 0x1a, 0x6a, 0x62, 0x0b, 0x5d, 0x82, 0x1b, 0x8f,
                                                        0x2e, 0xbe, 0xa6, 0x1d, 0xe7, 0x9d, 0x2d, 0x8a, 0x72, 0xd9, 0xf1, 0x27,
                                                        0x32, 0xbc, 0x77, 0x85, 0x96, 0x70, 0x08, 0x69, 0x56, 0xdf, 0x99, 0x94,
                                                        0xa1, 0x90, 0x18, 0xbb, 0xfa, 0x7a, 0xb0, 0xa7, 0xf8, 0xab, 0x28, 0xd6,
                                                        0x15, 0x8e, 0xcb, 0xf2, 0x13, 0xe6, 0x78, 0x61, 0x3f, 0x89, 0x46, 0x0d,
                                                        0x35, 0x31, 0x88, 0xa3, 0x41, 0x80, 0xca, 0x17, 0x5f, 0x53, 0x83, 0xfe,
                                                        0xc3, 0x9b, 0x45, 0x39, 0xe1, 0xf5, 0x9e, 0x19, 0x5e, 0xb6, 0xcf, 0x4b,
                                                        0x38, 0x04, 0xb9, 0x2b, 0xe2, 0xc1, 0x4a, 0xdd, 0x48, 0x0c, 0xd0, 0x7d,
                                                        0x3d, 0x58, 0xde, 0x7c, 0xd8, 0x14, 0x6b, 0x87, 0x47, 0xe8, 0x79, 0x84,
                                                        0x73, 0x3c, 0xbd, 0x92, 0xc9, 0x23, 0x8b, 0x97, 0x95, 0x44, 0xdc, 0xad,
                                                        0x40, 0x65, 0x86, 0xa2, 0xa4, 0xcc, 0x7f, 0xec, 0xc0, 0xaf, 0x91, 0xfd,
                                                        0xf7, 0x4f, 0x81, 0x2f, 0x5b, 0xea, 0xa8, 0x1c, 0x02, 0xd1, 0x98, 0x71,
                                                        0xed, 0x25, 0xe3, 0x24, 0x06, 0x68, 0xb3, 0x93, 0x2c, 0x6f, 0x3e, 0x6c,
                                                        0x0a, 0xb8, 0xce, 0xae, 0x74, 0xb1, 0x42, 0xb4, 0x1e, 0xd3, 0x49, 0xe9,
                                                        0x9c, 0xc8, 0xc6, 0xc7, 0x22, 0x6e, 0xdb, 0x20, 0xbf, 0x43, 0x51, 0x52,
                                                        0x66, 0xb2, 0x76, 0x60, 0xda, 0xc5, 0xf3, 0xf6, 0xaa, 0xcd, 0x9a, 0xa0,
                                                        0x75, 0x54, 0x0e, 0x01
                                                };
    //--- The key schedule in this AES cipher.
    private int[] m_Cipher;
    //--- The state matrix in this AES cipher with Nb columns and 4 rows
    private byte[,] m_CipherTable;
    //--- Rcon is the round constant
    private static readonly uint[] m_Rcon = new uint[]
                                              {
                                                      0x00000000, 0x01000000, 0x02000000, 0x04000000, 0x08000000, 0x10000000,
                                                      0x20000000, 0x40000000, 0x80000000, 0x1b000000, 0x36000000, 0x6c000000,
                                                      0xd8000000, 0xab000000, 0x4d000000, 0x9a000000, 0x2f000000
                                              };
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="key">aes key</param>
    public MTCryptAES(byte[] key)
      {
      Init(key);
      }
    /// <summary>
    /// initialization key
    /// </summary>
    /// <param name="key">aes key</param>
    public void Init(byte[] key)
      {
      if(key == null || key.Length == 0) return;
      //---
      m_Nk = key.Length / 4;
      m_Nr = m_Nk + M_NB + 2;
      //---
      if(m_Nk != 4 && m_Nk != 6 && m_Nk != 8)
        {
        MTLog.Write(MTLogType.Error,string.Format("Key is {0} bits long. *not* 128, 192, or 256.",m_Nk * 32));
        return;
        }
      //---
      KeyExpansion(key); // places expanded key in w
      }
    /// <summary>
    ///  makes a big key out of a small one
    /// </summary>
    /// <param name="key">aes key</param>
    private void KeyExpansion(byte[] key)
      {
      m_Cipher = new int[M_NB * (m_Nr + 1)];
      // the first Nk words of w are the cipher key z
      int i = 0;
      for(; i < m_Nk; i++)
        {
        //m_Cipher[i] = 0;
        // fill an entire word of expanded key m_cipher
        // by pushing 4 bytes into the m_cipher[i] word
        m_Cipher[i] = key[4 * i]; // add a byte in
        m_Cipher[i] <<= 8; // make room for the next byte
        m_Cipher[i] += key[4 * i + 1];
        m_Cipher[i] <<= 8;
        m_Cipher[i] += key[4 * i + 2];
        m_Cipher[i] <<= 8;
        m_Cipher[i] += key[4 * i + 3];
        }
      //---
      for(; i < M_NB * (m_Nr + 1); i++)
        {
        int temp = m_Cipher[i - 1]; // temporary 32-bit word
        //---
        if(i % m_Nk == 0) temp = (int)(SubWord(RotWord(temp)) ^ m_Rcon[i / m_Nk]);
        else if(m_Nk > 6 && i % m_Nk == 4) temp = SubWord(temp);
        //---
        m_Cipher[i] = (int)((m_Cipher[i - m_Nk] ^ temp) & 0x00000000FFFFFFFF);
        }
      }
    /// <summary>
    /// applies a cyclic permutation to a 4-byte word.
    /// </summary>
    /// <param name="w"></param>
    /// <returns></returns>
    private static int RotWord(int w)
      {
      int temp = w >> 24; // put the first 8-bits into temp
      w <<= 8; // make room for temp to fill the lower end of the word
      w = (int)(w & 0x00000000FFFFFFFF);
      //--- Can't do unsigned shifts, so we need to make this temp positive
      temp = (temp < 0 ? (256 + temp) : temp);
      w += temp;
      //---
      return w;

      }
    /// <summary>
    /// applies S-box substitution to each byte of a 4-byte word.
    /// </summary>
    /// <param name="w"></param>
    /// <returns></returns>
    private static int SubWord(int w)
      {
      //--- loop through 4 bytes of a word
      for(int i = 0; i < 4; i++)
        {
        int temp = w >> 24; // put the first 8-bits into temp
        //--- Can't do unsigned shifts, so we need to make this temp positive
        temp = (temp < 0 ? (256 + temp) : temp);
        w <<= 8; // make room for the substituted byte in w;
        w = (int)(w & 0x00000000FFFFFFFF);
        w += m_Sbox[temp]; // add the substituted byte back
        }
      //---
      return w;
      }
    /// <summary>
    /// adds the key schedule for a round to a state matrix.
    /// </summary>
    /// <param name="round"></param>
    private void AddRoundKey(int round)
      {
      for(int i = 0; i < 4; i++)
        {
        for(int j = 0; j < M_NB; j++)
          {
          //--- place the i-th byte of the j-th word from expanded key w into temp
          int temp = m_Cipher[round * M_NB + j] >> (3 - i) * 8;
          //--- Cast temp from a 32-bit word into an 8-bit byte.
          temp %= 256;
          //-- Can't do unsigned shifts, so we need to make this temp positive
          temp = (temp < 0 ? (256 + temp) : temp);
          //---
          m_CipherTable[i,j] ^= (byte)temp; // xor temp with the byte at location (i,j) of the state
          }
        }
      }
    /// <summary>
    /// mixes each column of a state matrix.
    /// </summary>
    /// <returns></returns>
    private void MixColumns()
      {
      //--- There are m_nb columns
      for(int i = 0; i < M_NB; i++)
        {
        byte s0 = m_CipherTable[0,i];
        byte s1 = m_CipherTable[1,i];
        byte s2 = m_CipherTable[2,i];
        byte s3 = m_CipherTable[3,i];
        //---
        m_CipherTable[0,i] = (byte)(Mult(0x02,s0) ^ Mult(0x03,s1) ^ Mult(0x01,s2) ^ Mult(0x01,s3));
        m_CipherTable[1,i] = (byte)(Mult(0x01,s0) ^ Mult(0x02,s1) ^ Mult(0x03,s2) ^ Mult(0x01,s3));
        m_CipherTable[2,i] = (byte)(Mult(0x01,s0) ^ Mult(0x01,s1) ^ Mult(0x02,s2) ^ Mult(0x03,s3));
        m_CipherTable[3,i] = (byte)(Mult(0x03,s0) ^ Mult(0x01,s1) ^ Mult(0x01,s2) ^ Mult(0x02,s3));
        }
      }
    /// <summary>
    /// applies a cyclic shift to the last 3 rows of a state matrix.
    /// </summary>
    private void ShiftRows()
      {
      byte[] temp = new byte[4];
      for(int i = 1; i < 4; i++)
        {
        int j;
        for(j = 0; j < M_NB; j++) temp[j] = m_CipherTable[i,(j + i) % M_NB];
        for(j = 0; j < M_NB; j++) m_CipherTable[i,j] = temp[j];
        }
      }
    /// <summary>
    /// applies S-Box substitution to each byte of a state matrix.
    /// </summary>
    private void SubBytes()
      {
      for(int i = 0; i < 4; i++)
        {
        for(int j = 0; j < M_NB; j++) m_CipherTable[i,j] = m_Sbox[m_CipherTable[i,j]];
        }
      }
    /// <summary>
    /// multiplies two polynomials a(x), b(x) in GF(2^8) modulo the irreducible polynomial m(x) = x^8+x^4+x^3+x+1
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    private static byte Mult(byte a,byte b)
      {
      int sum = m_Ltable[a] + m_Ltable[b];
      sum %= 255;
      //--- Get the antilog
      sum = m_Atable[sum];
      return (byte)(a == 0 ? 0 : (b == 0 ? 0 : sum));
      }
    /// <summary>
    /// Encrypts the 16-byte plain text.
    /// </summary>
    /// <param name="x">16 byte array</param>
    /// <returns>x 16-byte ciphertext</returns>
    public byte[] EncryptBlock(byte[] x)
      {
      m_CipherTable = new byte[4,M_NB];
      //---
      byte[] y = new byte[x.Length]; // 16-byte string
      //--- place input x into the initial state matrix in column order
      for(int i = 0; i < 4 * M_NB; i++)
        {
        //--- we want integerger division for the second index
        m_CipherTable[i % 4,(i - i % M_NB) / M_NB] = x[i];
        }
      //--- add round key
      AddRoundKey(0);
      //---
      for(int i = 1; i < m_Nr; i++)
        {
        //--- substitute bytes
        SubBytes();
        //--- shift rows
        ShiftRows();
        //--- mix columns
        MixColumns();
        //--- add round key
        AddRoundKey(i);
        }
      //--- substitute bytes
      SubBytes();
      //--- shift rows
      ShiftRows();
      //--- add round key
      AddRoundKey(m_Nr);
      //--- place state matrix s into y in column order
      for(int i = 0; i < 4 * M_NB; i++) y[i] = m_CipherTable[i % 4,(i - i % M_NB) / M_NB];
      //---
      return y;
      }
    }
  }
