# Curve25519.NetCore

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![nuget](https://img.shields.io/nuget/v/Curve25519.NetCore.svg)](https://www.nuget.org/packages/Curve25519.NetCore/)

`Curve25519.NetCore` is a .NET implementation of Curve25519 (X25519) focused on elliptic-curve Diffie-Hellman (ECDH) key agreement.

It provides a compact API for:

- Generating random, clamped private keys
- Deriving public keys
- Computing shared secrets with peer public keys

The library targets modern .NET and follows RFC 7748 guidance by rejecting all-zero shared secrets.

---

## Table of contents

- [Requirements](#requirements)
- [Installation](#installation)
- [Quick start](#quick-start)
- [API reference](#api-reference)
  - [`Curve25519`](#curve25519)
- [Validation and test vectors](#validation-and-test-vectors)
- [Best practices](#best-practices)
- [Development](#development)
- [Security notes](#security-notes)
- [License](#license)

---

## Requirements

- **.NET 8 SDK** for building/testing this repository.
- Project target framework: **.NET 8**.
- Dependency: [`SecureRandom.NetCore` (v2.x)](https://github.com/TimothyMeadows/SecureRandom.NetCore) for random private key generation.

---

## Installation

### NuGet Package Manager (CLI)

```bash
dotnet add package Curve25519.NetCore
```

### Package Manager Console

```powershell
Install-Package Curve25519.NetCore
```

### NuGet Gallery

- https://www.nuget.org/packages/Curve25519.NetCore/

---

## Quick start

```csharp
using System;
using System.Linq;
using Curve25519.NetCore;

var curve25519 = new Curve25519();

// Alice key pair
var alicePrivate = curve25519.CreateRandomPrivateKey();
var alicePublic = curve25519.GetPublicKey(alicePrivate);

// Bob key pair
var bobPrivate = curve25519.CreateRandomPrivateKey();
var bobPublic = curve25519.GetPublicKey(bobPrivate);

// Shared secret derivation
var aliceShared = curve25519.GetSharedSecret(alicePrivate, bobPublic);
var bobShared = curve25519.GetSharedSecret(bobPrivate, alicePublic);

var equal = aliceShared.SequenceEqual(bobShared);
Console.WriteLine($"Shared secrets match: {equal}");
```

> `GetSharedSecret(...)` returns raw shared secret bytes. In protocol design, derive final session keys from this output using an appropriate KDF.

---

## API reference

## `Curve25519`

### Constants

```csharp
public const int KeySize = 32;
```

### Key generation and clamping

```csharp
byte[] CreateRandomPrivateKey()
byte[] ClampPrivateKey(byte[] rawKey)
void ClampPrivateKeyInline(byte[] key)
```

- Private key length must be exactly 32 bytes.
- Clamping is required for valid X25519 private scalars.
- `CreateRandomPrivateKey()` generates 32 random bytes and clamps them before returning.

### Public key and agreement methods

```csharp
byte[] GetPublicKey(byte[] privateKey)
byte[] GetSigningKey(byte[] privateKey)
byte[] GetSharedSecret(byte[] privateKey, byte[] peerPublicKey)
```

- `GetPublicKey(...)` derives a 32-byte public key.
- `GetSharedSecret(...)` performs X25519 with length validation.
- An all-zero derived shared secret is rejected with a `CryptographicException`.

---

## Validation and test vectors

The test project includes RFC 7748 vector validation and agreement checks, covering:

- Public key generation from known private keys
- Shared secret derivation consistency
- RFC 7748 interoperability values

Run the test suite:

```bash
dotnet test Curve25519.NetCore.sln
```

---

## Best practices

1. **Treat private keys as sensitive**
   - Store and transport private keys securely.
   - Keep private key material in memory for as short a time as possible.

2. **Always validate key sizes at boundaries**
   - The API enforces 32-byte key inputs; keep this invariant throughout your application.

3. **Use a KDF on shared secrets**
   - Do not use raw ECDH output directly as a symmetric key in production protocols.

4. **Reject invalid agreement outputs**
   - This library rejects all-zero secrets per RFC 7748 recommendations.

5. **Avoid sharing mutable key arrays across threads**
   - Prefer immutable handling/copies when passing keys between components.

---

## Development

### Build

```bash
dotnet build Curve25519.NetCore.sln
```

### Test

```bash
dotnet test Curve25519.NetCore.sln
```

---

## Security notes

- The library validates key lengths before cryptographic operations.
- Generated private keys are clamped before use.
- Shared secrets evaluating to all-zero are rejected according to [RFC 7748, Section 6.1](https://www.rfc-editor.org/rfc/rfc7748#section-6.1).
- As with all cryptographic code, review integration choices (KDF, identity/authentication, key lifecycle) against your threat model.

---

## License

MIT. See [LICENSE](LICENSE).
