/*
Copyright(c) <2018> <University of Washington>
Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
using System.Collections.Generic;
using System.Data.SqlClient;
using Dqdv.External.Contracts.Azure;
using Microsoft.SqlServer.Management.AlwaysEncrypted.AzureKeyVaultProvider;

namespace Dqdv.External
{
    /// <summary>
    /// Azure sql column encryption key store provider implementation
    /// </summary>
    public class AzureSqlColumnEncryptionKeyStoreProvider : IAzureSqlColumnEncryptionKeyStoreProvider
    {
        ////////////////////////////////////////////////////////////
        // Constants, Enums and Class members
        ////////////////////////////////////////////////////////////

        private readonly IAzureKeyVaultTokenProvider _keyVaultTokenProvider;

        ////////////////////////////////////////////////////////////
        // Constructors
        ////////////////////////////////////////////////////////////

        public AzureSqlColumnEncryptionKeyStoreProvider(IAzureKeyVaultTokenProvider keyVaultTokenProvider)
        {
            _keyVaultTokenProvider = keyVaultTokenProvider;
        }

        ////////////////////////////////////////////////////////////
        // Public Methods/Atributes
        ////////////////////////////////////////////////////////////

        /// <inheritdoc />
        /// <summary>
        /// Register the provider with ADO.net
        /// </summary>
        public void Register()
        {
            var azureKeyVaultProvider = new SqlColumnEncryptionAzureKeyVaultProvider(_keyVaultTokenProvider.GetTokenAsync);
            var providers = new Dictionary<string, SqlColumnEncryptionKeyStoreProvider> { { SqlColumnEncryptionAzureKeyVaultProvider.ProviderName, azureKeyVaultProvider } };
            
            // register the provider with ADO.net
            SqlConnection.RegisterColumnEncryptionKeyStoreProviders(providers);
        }
    }
}
