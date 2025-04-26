using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    public class RandomNum
    {
        public byte[] Generate(int length)
        {
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] data = new byte[length / 8];
                rng.GetBytes(data);
                return data;
            }
        }
    }
}
