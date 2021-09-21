using Newtonsoft.Json;
using PCLCrypto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CG.Services
{


	public interface IRsaCryptService
	{
		string Crypt(string rsaPublicKey, string dataToCrypt);
		string Decrypt(string rsaPrivateKey, string dataToDecrypt);
		(string privateKey, string publicKey) GenerateKeys();
		bool VerifySignature(string rsaPublicKey, byte[] buffer, byte[] siggnedBuffer);
		string SignData(string rsaPrivateKey, byte[] buffer);
	}
	public class RsaCryptService : IRsaCryptService
	{

		public string Crypt(string rsaPublicKey, string dataToCrypt)
		{
			var rsaPublicParameter = JsonConvert.DeserializeObject<RSAParameters>(rsaPublicKey);
			var rsa = WinRTCrypto.AsymmetricKeyAlgorithmProvider.OpenAlgorithm(AsymmetricAlgorithm.RsaPkcs1);

				var key=rsa.ImportParameters(rsaPublicParameter);
				byte[] buffer = WinRTCrypto.CryptographicEngine.Encrypt(key,dataToCrypt.FromUrlSafeBase64());

				return buffer.ToUrlSafeBase64();
			
		}

		public string Decrypt(string rsaPrivateKey, string dataToDecrypt)
		{
			byte[] bufferToDecrypt = dataToDecrypt.FromUrlSafeBase64();

			var rsa = WinRTCrypto.AsymmetricKeyAlgorithmProvider.OpenAlgorithm(AsymmetricAlgorithm.RsaPkcs1);
			
				var rsaPrivateParameter = JsonConvert.DeserializeObject<RSAParameters>(rsaPrivateKey);
				var key=rsa.ImportParameters(rsaPrivateParameter);

				byte[] decryptedData = WinRTCrypto.CryptographicEngine.Decrypt(key, bufferToDecrypt);

				return decryptedData.ToUrlSafeBase64();
			
		}

		public (string privateKey, string publicKey) GenerateKeys()
		{

			IAsymmetricKeyAlgorithmProviderFactory rsa = WinRTCrypto.AsymmetricKeyAlgorithmProvider;


			using (var keyPair = rsa.OpenAlgorithm(AsymmetricAlgorithm.RsaPkcs1).CreateKeyPair(2048))
			{

				RSAParameters publicKeyParameter = keyPair.ExportParameters(false);
				RSAParameters privateKeyParameter = keyPair.ExportParameters(true);

				string privateKey = JsonConvert.SerializeObject(privateKeyParameter);
				string publicKey = JsonConvert.SerializeObject(publicKeyParameter);

				return (privateKey, publicKey);


			}
		}



		public string SignData(string rsaPrivateKey, byte[] buffer)
		{
			var rsa = WinRTCrypto.AsymmetricKeyAlgorithmProvider.OpenAlgorithm(AsymmetricAlgorithm.RsaPkcs1);
			
				RSAParameters privateKey = JsonConvert.DeserializeObject<RSAParameters>(rsaPrivateKey);
				var key=rsa.ImportParameters(privateKey);

				byte[] signedHashValue = WinRTCrypto.CryptographicEngine.Sign(key,buffer);
				return signedHashValue.ToUrlSafeBase64();
			
		}

		public bool VerifySignature(string rsaPublicKey, byte[] buffer, byte[] siggnedBuffer)
		{
			var rsa = WinRTCrypto.AsymmetricKeyAlgorithmProvider.OpenAlgorithm(AsymmetricAlgorithm.RsaPkcs1);
			
				RSAParameters publicKey = JsonConvert.DeserializeObject<RSAParameters>(rsaPublicKey);
				var key =rsa.ImportParameters(publicKey);

				if (WinRTCrypto.CryptographicEngine.VerifySignature(key,buffer, siggnedBuffer))
					return true;
				return false;
			
		}
	}
}
