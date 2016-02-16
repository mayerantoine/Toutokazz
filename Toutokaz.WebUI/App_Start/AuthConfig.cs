using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Web.WebPages.OAuth;

namespace Toutokaz.WebUI
{
    public class AuthConfig
    {
        public static void RegisterAuth()
        {
            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

            //OAuthWebSecurity.RegisterMicrosoftClient(
            //    clientId: "",
            //    clientSecret: "");

            //OAuthWebSecurity.RegisterTwitterClient(
            //    consumerKey: "",
            //    consumerSecret: "");

            Dictionary<string, object> FacebooksocialData = new Dictionary<string, object>();
            FacebooksocialData.Add("Icon", "~/Content/img/facebook_connect2.jpg");
           
            
     /*OAuthWebSecurity.RegisterFacebookClient(
               appId: "330628387121240",
               appSecret: "5e2bf8fc2bdcb4aff090fe693c42f0db",
                displayName: "Facebook",
                extraData: FacebooksocialData
               );*/


            //staging-touokazz
       OAuthWebSecurity.RegisterFacebookClient(
                    appId: "796936470401242",
                    appSecret: "0ad53c54cccc914787dd43aedbe0d351",
                     displayName: "Facebook",
                     extraData: FacebooksocialData
                    );

            // production toutokazz
         /* OAuthWebSecurity.RegisterFacebookClient(
                appId: "755464477881775",
                appSecret: "927e3101c3da4a113c92ea763eacbef7",
                 displayName: "Facebook",
                 extraData: FacebooksocialData
                );*/


            //OAuthWebSecurity.RegisterGoogleClient();
        }
    }
}