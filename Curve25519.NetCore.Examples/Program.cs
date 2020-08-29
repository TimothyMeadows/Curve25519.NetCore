using System;
using System.Linq;

namespace Curve25519.NetCore.Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            var alicePrivate = Curve25519.CreateRandomPrivateKey();
            var alicePublic = Curve25519.GetPublicKey(alicePrivate);

            var bobPrivate = Curve25519.CreateRandomPrivateKey();
            var bobPublic = Curve25519.GetPublicKey(bobPrivate);

            var aliceShared = Curve25519.GetSharedSecret(alicePrivate, bobPublic);
            var bobShared = Curve25519.GetSharedSecret(bobPrivate, alicePublic);
            var equal = aliceShared.SequenceEqual(bobShared);

            return;
        }
    }
}
