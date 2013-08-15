using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationHandler
{
    public static class NotificationHandler
    {
        static void Main(string[] args)
        {
        }

        public static void PublishNotification(string body, int userId)
        {
            string channel = "Cruela Devil-channel";

            PubnubAPI pubnub = new PubnubAPI(
                    "pub-c-de7a8d7a-782c-4d22-97cc-8bf78fc2e08d",               // PUBLISH_KEY
                    "sub-c-436627b2-0597-11e3-a005-02ee2ddab7fe",               // SUBSCRIBE_KEY
                    "sec-c-ZGE1NTgxYTYtMGE5Yi00NzFlLWFkNmUtMTJjNWRkNTA1NWE0",   // SECRET_KEY
                    true                                                        // SSL_ON?
                );
            
            string notification = "userId = " + userId + ";" + body;

            pubnub.Publish(channel, notification);
        }
    }
}
