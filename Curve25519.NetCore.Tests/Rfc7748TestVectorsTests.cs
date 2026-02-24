using System;
using Xunit;

namespace Curve25519.NetCore.Tests;

public sealed class Rfc7748TestVectorsTests
{
    private static readonly byte[] BasePoint = Convert.FromHexString("0900000000000000000000000000000000000000000000000000000000000000");

    [Fact]
    public void X25519_AliceBob_Example_MatchesRfc7748()
    {
        var curve = new Curve25519();

        var alicePrivate = Convert.FromHexString("77076d0a7318a57d3c16c17251b26645df4c2f87ebc0992ab177fba51db92c2a");
        var alicePublicExpected = Convert.FromHexString("8520f0098930a754748b7ddcb43ef75a0dbf3a0d26381af4eba4a98eaa9b4e6a");

        var bobPrivate = Convert.FromHexString("5dab087e624a8a4b79e17f8b83800ee66f3bb1292618b6fd1c2f8b27ff88e0eb");
        var bobPublicExpected = Convert.FromHexString("de9edb7d7b7dc1b4d35b61c2ece435373f8343c85b78674dadfc7e146f882b4f");

        var sharedExpected = Convert.FromHexString("4a5d9d5ba4ce2de1728e3bf480350f25e07e21c947d19e3376f09b3c1e161742");

        var alicePublic = curve.GetPublicKey(curve.ClampPrivateKey(alicePrivate));
        var bobPublic = curve.GetPublicKey(curve.ClampPrivateKey(bobPrivate));

        var aliceShared = curve.GetSharedSecret(curve.ClampPrivateKey(alicePrivate), bobPublic);
        var bobShared = curve.GetSharedSecret(curve.ClampPrivateKey(bobPrivate), alicePublic);

        Assert.Equal(alicePublicExpected, alicePublic);
        Assert.Equal(bobPublicExpected, bobPublic);
        Assert.Equal(sharedExpected, aliceShared);
        Assert.Equal(sharedExpected, bobShared);
    }

    [Fact]
    public void X25519_Iterated_TestVector_MatchesRfc7748_For1And1000Iterations()
    {
        var curve = new Curve25519();
        var k = BasePoint;
        var u = BasePoint;

        for (var i = 1; i <= 1000; i++)
        {
            var previousK = k;
            k = curve.GetSharedSecret(curve.ClampPrivateKey(k), u);
            u = previousK;

            if (i == 1)
            {
                var expectedAfter1 = Convert.FromHexString("422c8e7a6227d7bca1350b3e2bb7279f7897b87bb6854b783c60e80311ae3079");
                Assert.Equal(expectedAfter1, k);
            }
        }

        var expectedAfter1000 = Convert.FromHexString("684cf59ba83309552800ef566f2f4d3c1c3887c49360e3875f2eb94d99532c51");
        Assert.Equal(expectedAfter1000, k);
    }
}
