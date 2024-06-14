using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;

class RSAUtility
{
    public static (string privateKeyPem, string publicKeyPem) GenerateKeysAndExportToPem()
    {
        // Generate RSA key pair
        var rsaKeyPair = GenerateRsaKeyPair();

        // Export private key to PEM format
        var privateKeyPem = ExportPrivateKey(rsaKeyPair.Private);

        // Export public key to PEM format
        var publicKeyPem = ExportPublicKey(rsaKeyPair.Public);

        return (privateKeyPem, publicKeyPem);
    }

    private static AsymmetricCipherKeyPair GenerateRsaKeyPair()
    {
        var keyGenerationParameters = new KeyGenerationParameters(new SecureRandom(), 2048);
        var keyPairGenerator = new RsaKeyPairGenerator();
        keyPairGenerator.Init(keyGenerationParameters);
        return keyPairGenerator.GenerateKeyPair();
    }

    private static string ExportPrivateKey(AsymmetricKeyParameter privateKey)
    {
        using (var stringWriter = new StringWriter())
        {
            var pemWriter = new PemWriter(stringWriter);
            pemWriter.WriteObject(privateKey);
            pemWriter.Writer.Flush();
            return stringWriter.ToString();
        }
    }

    private static string ExportPublicKey(AsymmetricKeyParameter publicKey)
    {
        using (var stringWriter = new StringWriter())
        {
            var pemWriter = new PemWriter(stringWriter);
            pemWriter.WriteObject(publicKey);
            pemWriter.Writer.Flush();
            return stringWriter.ToString();
        }
    }
    public static string Encrypt(string publicKeyPem, string plaintext)
    {
        using (var rsa = GetRsaFromPublicKey(publicKeyPem))
        {
            var plaintextBytes = Encoding.UTF8.GetBytes(plaintext);
            var encryptedBytes = rsa.Encrypt(plaintextBytes, RSAEncryptionPadding.Pkcs1);
            return Convert.ToBase64String(encryptedBytes);
        }
    }

    // Method to decrypt data using RSA private key
    public static string Decrypt(string privateKeyPem, string encryptedBase64)
    {
        using (var rsa = GetRsaFromPrivateKey(privateKeyPem))
        {
            var encryptedBytes = Convert.FromBase64String(encryptedBase64);
            var decryptedBytes = rsa.Decrypt(encryptedBytes, RSAEncryptionPadding.Pkcs1);
            return Encoding.UTF8.GetString(decryptedBytes);
        }
    }
    // Method to get RSA object from PEM-encoded public key
    private static RSACryptoServiceProvider GetRsaFromPublicKey(string publicKeyPem)
    {
        using (var stringReader = new StringReader(publicKeyPem))
        {
            var pemReader = new PemReader(stringReader);
            var publicKeyParameters = pemReader.ReadObject() as RsaKeyParameters;
            var rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(DotNetUtilities.ToRSAParameters(publicKeyParameters));
            return rsa;
        }
    }

    // Method to get RSA object from PEM-encoded private key
    private static RSACryptoServiceProvider GetRsaFromPrivateKey(string privateKeyPem)
    {
        using (var stringReader = new StringReader(privateKeyPem))
        {
            var pemReader = new PemReader(stringReader);
            var pemObject = pemReader.ReadObject();

            // Check if it's a PKCS#8 private key
            if (pemObject is AsymmetricCipherKeyPair)
            {
                var keyPair = (AsymmetricCipherKeyPair)pemObject;
                var privateKeyParams = (RsaPrivateCrtKeyParameters)keyPair.Private;
                var rsa = new RSACryptoServiceProvider();
                rsa.ImportParameters(DotNetUtilities.ToRSAParameters(privateKeyParams));
                return rsa;
            }
            else if (pemObject is RsaPrivateCrtKeyParameters)
            {
                var privateKeyParams = (RsaPrivateCrtKeyParameters)pemObject;
                var rsa = new RSACryptoServiceProvider();
                rsa.ImportParameters(DotNetUtilities.ToRSAParameters(privateKeyParams));
                return rsa;
            }
            else
            {
                throw new InvalidOperationException("Invalid private key format");
            }
        }
    }
}
