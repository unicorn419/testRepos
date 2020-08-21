using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Converters;
namespace ConsoleSerializationTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string  s ="{\"id\":\"Sk3KHKYXwvmeGEZJsU6vs4ScXSC7OnRb6uoTOIWpGyc\",\"state\":\"7f09aa42-c896-4070-b80c-1ad05a6b43fd\",\"assets\":[{\"name\":\"asset1\",\"data\":{\"address\":{\"street_address\":\"4101 Yonge Street\",\"country\":\"CA\",\"locality\":\"Toronto\",\"region\":\"ON\",\"postal_code\":\"M2P 1N6\"},\"birthdate\":\"1965-03-03\",\"email\":\"jdoe8@securekey.com\",\"family_name\":\"Doe\",\"given_name\":\"John\",\"middle_name\":\"H\",\"phone_number\":\"14169378708\"}}]}";
            dacUseLicenseResponse dac = Newtonsoft.Json.JsonConvert.DeserializeObject<dacUseLicenseResponse>(s);
        }
    }


    public class dacUseLicenseResponse
    {
        public string id { get; set; }

        public string state { get; set; }

        public Auth auth { get; set; }

        public List<Asset> assets { get; set; }


    }
    public class Auth
    {

        public string subject { get; set; }


    }
    public class VerifiedMeUser
    {
        public int VerifiedMeUserId { get; set; }
        public string given_name { get; set; }
        public string family_name { get; set; }
        public string middle_name { get; set; }
        public string title { get; set; }
        public string honorific { get; set; }
        public string birthdate { get; set; }
        public string customer_ref_num { get; set; }
        public string email { get; set; }
        public string License { get; set; }
        public VerifiedMeUserAddress address { get; set; }
    }

    public class VerifiedMeUserAddress
    {
        public int VerifiedMeUserAddressId { get; set; }
        public string street_address { get; set; }

        public string locality { get; set; }

        public string region { get; set; }
        public string postal_code { get; set; }
        public string country { get; set; }
    }
    public class Asset
    {
        public string name { get; set; }

        public VerifiedMeUser data { get; set; }

    }
}
