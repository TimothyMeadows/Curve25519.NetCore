# Curve25519.NetCore
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT) [![nuget](https://img.shields.io/nuget/v/Curve25519.NetCore.svg)](https://www.nuget.org/packages/Curve25519.NetCore/)

An elliptic curve offering 128 bits of security and designed for use with the elliptic curve Diffie–Hellman (ECDH) key agreement scheme. It is one of the fastest ECC curves and is not covered by any known patents. Depends on [SecureRandom.NetCore](https://github.com/TimothyMeadows/SecureRandom.NetCore) (v2.x).

# Install

From a command prompt
```bash
dotnet add package Curve25519.NetCore
```

```bash
Install-Package Curve25519.NetCore
```

You can also search for package via your nuget ui / website:

https://www.nuget.org/packages/Curve25519.NetCore/

# Examples

You can find more examples in the github examples project.

```csharp
var curve25519 = new Curve25519();
var alicePrivate = curve25519.CreateRandomPrivateKey();
var alicePublic = curve25519.GetPublicKey(alicePrivate);

var bobPrivate = curve25519.CreateRandomPrivateKey();
var bobPublic = curve25519.GetPublicKey(bobPrivate);

var aliceShared = curve25519.GetSharedSecret(alicePrivate, bobPublic);
var bobShared = curve25519.GetSharedSecret(bobPrivate, alicePublic);
var equal = aliceShared.SequenceEqual(bobShared);
```


## Framework support

This project now targets **.NET 8**.

## Security notes

- Private and public key lengths are validated before cryptographic operations.
- Shared secrets that evaluate to all-zero are rejected according to [RFC 7748, Section 6.1](https://www.rfc-editor.org/rfc/rfc7748#section-6.1).
- Generated private keys are clamped before use.
