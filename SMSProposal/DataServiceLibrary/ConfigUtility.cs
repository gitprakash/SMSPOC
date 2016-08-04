namespace DataServiceLibrary
{
    using System;
    using System.Configuration;
    public static class ConfigUtility
    {
        public static string GetAppkeyvalues(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
        public static string GetAPIConfigvalue()
        {
            //username=@username&pass=@password&senderid=@senderId&dest_mobileno=@mobileno&message=@message&response=Y
            
            string apiurl = ConfigUtility.GetAppkeyvalues("apiurl");
            apiurl = apiurl + "username={0}&pass={1}&senderid={2}";
            string username = ConfigUtility.GetAppkeyvalues("username");
            string password = ConfigUtility.GetAppkeyvalues("password");
            string senderId = ConfigUtility.GetAppkeyvalues("senderId");
           apiurl= string.Format(apiurl, username, password, senderId);
            return apiurl;
        }
    }
}