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
using System;
using System.Threading.Tasks;
using Dqdv.External.Contracts.Azure;
using Dqdv.Internal.Contracts.Settings;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace Dqdv.External
{
    /// <summary>
    /// Azure key vault token provider implementation
    /// </summary>
    public class AzureKeyVaultTokenProvider : IAzureKeyVaultTokenProvider
    {
        ////////////////////////////////////////////////////////////
        // Constants, Enums and Class members
        ////////////////////////////////////////////////////////////
        
        private readonly ClientCredential _clientCredential;

        ////////////////////////////////////////////////////////////
        // Constructors
        ////////////////////////////////////////////////////////////

        /// <summary>
        /// Initialize a new instance of <see cref="AzureKeyVaultTokenProvider"/>
        /// </summary>
        /// <param name="settings">Instance of <see cref="IAzureActiveDirectorySettings"/></param>
        /// <param name="clientCredentialFactory">Instance of <see cref="IClientCredentialFactory"/></param>
        public AzureKeyVaultTokenProvider(IAzureActiveDirectorySettings settings, IClientCredentialFactory clientCredentialFactory)
        {
            _clientCredential = clientCredentialFactory.GetClientCredential(settings);
        }

        ////////////////////////////////////////////////////////////
        // Public Methods/Atributes
        ////////////////////////////////////////////////////////////

        /// <inheritdoc />
        /// <summary>
        /// Gets azure key vault access token
        /// </summary>
        /// <param name="authority"></param>
        /// <param name="resource"></param>
        /// <param name="scope"></param>
        /// <exception cref="T:System.InvalidOperationException">Failed to obtain access token</exception>
        /// <returns>jwt access token</returns>
        public async Task<string> GetTokenAsync(string authority, string resource, string scope)
        {
            var authContext = new AuthenticationContext(authority);
            var result = await authContext.AcquireTokenAsync(resource, _clientCredential);

            if (result == null)
                throw new InvalidOperationException("Failed to obtain the JWT token");

            return result.AccessToken;
        }
    }
}
