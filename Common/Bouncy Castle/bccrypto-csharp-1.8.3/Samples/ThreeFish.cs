using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;

namespace Samples
{
    public class ThreeFish256 : ICryptoTransform
    {
        protected IBlockCipher engine;

        public ThreeFish256()
        {
            engine = new ThreefishEngine(ThreefishEngine.BLOCKSIZE_256);
        }


        public int InputBlockSize
        {
            get
            {
                return engine.GetBlockSize();
            }
        }

        public int OutputBlockSize => throw new NotImplementedException();

        public bool CanTransformMultipleBlocks
        {
            get { return false; }
        }

        public bool CanReuseTransform { get { return false; } }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {
            throw new NotImplementedException();
        }

        public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            throw new NotImplementedException();
        }
    }
}
