var curve25519 = new Curve25519.NetCore.Curve25519();
var alicePrivate = curve25519.CreateRandomPrivateKey();
var alicePublic = curve25519.GetPublicKey(alicePrivate);

var bobPrivate = curve25519.CreateRandomPrivateKey();
var bobPublic = curve25519.GetPublicKey(bobPrivate);

var aliceShared = curve25519.GetSharedSecret(alicePrivate, bobPublic);
var bobShared = curve25519.GetSharedSecret(bobPrivate, alicePublic);

Console.WriteLine($"Shared secrets match: {aliceShared.SequenceEqual(bobShared)}");
