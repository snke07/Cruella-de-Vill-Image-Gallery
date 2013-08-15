namespace Dropbox
{
    using System;
    using System.IO;
    using System.Diagnostics;
    using Spring.Social.OAuth1;
    using Spring.Social.Dropbox.Api;
    using Spring.Social.Dropbox.Connect;
    using Spring.IO;
    using System.Threading;

    public class DropboxClient
    {
        private const string DropboxAppKey = "ouwcjfmnx8wj0il";
        private const string DropboxAppSecret = "m0g9rcuixdfegve";

        private const string OAuthTokenFileName = @"D:\OAuthToken.txt";
        private IDropbox client;

        public DropboxClient()
        {
            DropboxServiceProvider dropboxServiceProvider =
                new DropboxServiceProvider(DropboxAppKey, DropboxAppSecret, AccessLevel.AppFolder);

            // Authenticate the application (if not authenticated) and load the OAuth token
            //if (!File.Exists(OAuthTokenFileName))
            //{
            //    AuthorizeAppOAuth(dropboxServiceProvider);
            //}
            OAuthToken oauthAccessToken = new OAuthToken("v10ht168wdf4s5hb", "foe052r43yi58g2");

            // Login in Dropbox
            this.client = dropboxServiceProvider.GetApi(oauthAccessToken.Value, oauthAccessToken.Secret);

            // Upload a file
            
        }

        public string UploadFile(string filePath, string fileName)
        {
            Entry uploadEntry = client.UploadFileAsync(new FileResource(filePath), "/" + fileName).Result;
            return client.GetMediaLinkAsync("/" + fileName).Result.Url;
        }

        private static OAuthToken LoadOAuthToken()
        {
            string[] lines = File.ReadAllLines(OAuthTokenFileName);
            OAuthToken oauthAccessToken = new OAuthToken(lines[0], lines[1]);
            return oauthAccessToken;
        }

        private static void AuthorizeAppOAuth(DropboxServiceProvider dropboxServiceProvider)
        {
            OAuthToken oauthToken = dropboxServiceProvider.OAuthOperations.FetchRequestTokenAsync(null, null).Result;
                
            OAuth1Parameters parameters = new OAuth1Parameters();
            string authenticateUrl = dropboxServiceProvider.OAuthOperations.BuildAuthorizeUrl(
                oauthToken.Value, parameters);

            Process.Start(authenticateUrl);
            Thread.Sleep(10000);
           
            AuthorizedRequestToken requestToken = new AuthorizedRequestToken(oauthToken, null);
            OAuthToken oauthAccessToken = dropboxServiceProvider.OAuthOperations.ExchangeForAccessTokenAsync(requestToken, null).Result;

            string[] oauthData = new string[] { oauthAccessToken.Value, oauthAccessToken.Secret };
            File.WriteAllLines(OAuthTokenFileName, oauthData);
        }
    }
}