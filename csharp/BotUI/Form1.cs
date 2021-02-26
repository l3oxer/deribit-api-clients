using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Diagnostics;
using Org.OpenAPITools.Api;
using Org.OpenAPITools.Client;
using Org.OpenAPITools.Model;
using System.Net;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Linq;
using System.IO;

namespace BotUI {
    public partial class Form1 : Form {


        private AuthenticationApi authApiInstance;
        private PublicApi publicApiInstance;
        private PrivateApi privateApiInstance;
        private string accessToken = string.Empty;
        private string refreshToken = string.Empty;


        public Form1() {
            InitializeComponent();
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        private void button1_Click(object sender, EventArgs e) {
            try {
                // Change the user name for a subaccount

                var apiInstance = new PublicApi(Configuration.Default);
                var expectedResult = "";  // string | The value \"exception\" will trigger an error response. This may be useful for testing wrapper libraries. (optional) 

                Object result = apiInstance.PublicTestGet(expectedResult);
                
                MessageBox.Show(result.ToString());
            } catch (ApiException ex) {
                MessageBox.Show("Exception whenapiInstance.PublicTestGet: " + ex.Message);
                MessageBox.Show("Status Code: " + ex.ErrorCode);
                MessageBox.Show(ex.StackTrace);
            }
        }

        private void Form1_Load(object sender, EventArgs e) {

            Configuration.Default.BasePath = "https://test.deribit.com/api/v2";
            // Configure HTTP basic authorization: bearerAuth
            Configuration.Default.Username = "l3oxergames@gmail.com";
            Configuration.Default.Password = "@Aa90909090";
            
            //Configuration.Default.AccessToken = "gjF0ypJxao-8hmycbfgemBCjTtRnR9MMLxFxSkVoMyA";

            authApiInstance = new AuthenticationApi(Configuration.Default);
            privateApiInstance = new PrivateApi(Configuration.Default);

    }

        private void authButton_Click(object sender, EventArgs e) {
            
            var grantType = "client_credentials";  // string | Method of authentication
            var clientId = "yWELzn_u";  // string | Required for grant type `client_credentials` and `client_signature`
            var clientSecret = "gjF0ypJxao-8hmycbfgemBCjTtRnR9MMLxFxSkVoMyA";  // string | Required for grant type `client_credentials`
            var scope = "session:box trade:read_write";  // string | Describes type of the access for assigned token, possible values: `connection`, `session`, `session:name`, `trade:[read, read_write, none]`, `wallet:[read, read_write, none]`, `account:[read, read_write, none]`, `expires:NUMBER` (token will expire after `NUMBER` of seconds).</BR></BR> **NOTICE:** Depending on choosing an authentication method (```grant type```) some scopes could be narrowed by the server. e.g. when ```grant_type = client_credentials``` and ```scope = wallet:read_write``` it's modified by the server as ```scope = wallet:read``` (optional) 

            try {
                // Authenticate
                Object rootResponse = authApiInstance.PublicAuthGet(grantType, Configuration.Default.Username,Configuration.Default.Password, clientId, clientSecret, string.Empty, string.Empty, string.Empty, string.Empty,string.Empty, scope:scope);
                MessageBox.Show(rootResponse.ToString());
                var settings = new JsonSerializerSettings {
                    Converters =
                            {
                                new ForeignJsonNetContainerConverter(), new ForeignJsonNetValueConverter()
                            },
                };
                settings.Formatting = Formatting.Indented;
                RootAuthApi rootResponseObj = JsonConvert.DeserializeObject<RootAuthApi>(rootResponse.ToString(), settings);
                //Clipboard.SetText(rootResponse.ToString());
                AuthResult authRes= rootResponseObj.Result;
                refreshToken = authRes.RefreshToken;
                Configuration.Default.AccessToken = accessToken = authRes.AccessToken.Trim();
                refreshTokenTimer.Enabled = true;
            } catch (ApiException ex) {
                MessageBox.Show("Exception when calling AuthenticationApi.PublicAuthGet: " + ex.Message);
                MessageBox.Show("Status Code: " + ex.ErrorCode);
                MessageBox.Show(ex.StackTrace);
            }
        }

        private void refreshTokenTimer_Tick(object sender, EventArgs e) {
            var grantType = "refresh_token";  // string | Method of authentication

            try {
                // Authenticate
                Object rootResponse = authApiInstance.PublicAuthGet(grantType, string.Empty, string.Empty, string.Empty, string.Empty, refreshToken,string.Empty, string.Empty, string.Empty, string.Empty);
                MessageBox.Show(rootResponse.ToString());
                var settings = new JsonSerializerSettings {
                    Converters =
                            {
                                new ForeignJsonNetContainerConverter(), new ForeignJsonNetValueConverter()
                            },
                };
                settings.Formatting = Formatting.Indented;
                RootAuthApi rootResponseObj = JsonConvert.DeserializeObject<RootAuthApi>(rootResponse.ToString(), settings);
                //Clipboard.SetText(rootResponse.ToString());
                AuthResult authRes = rootResponseObj.Result;
                refreshToken = authRes.RefreshToken;
                Configuration.Default.AccessToken = accessToken = authRes.AccessToken;

            } catch (ApiException ex) {
                MessageBox.Show("Exception when calling AuthenticationApi.PublicAuthGet: " + ex.Message);
                MessageBox.Show("Status Code: " + ex.ErrorCode);
                MessageBox.Show(ex.StackTrace);
            }
        }

        private void createTestBuyOrder_Click(object sender, EventArgs e) {
        ;

            //ApiClient apiclient = new ApiClient(Configuration.Default);
            Configuration.Default.AddDefaultHeader("Access-Control-Allow-Origin", "*");
            Configuration.Default.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var apiInstance = new PrivateApi(Configuration.Default);
            var instrumentName = "BTC-PERPETUAL";  // string | Instrument name
            var amount = 500.0M;  // decimal? | It represents the requested order size. For perpetual and futures the amount is in USD units, for options it is amount of corresponding cryptocurrency contracts, e.g., BTC or ETH
            var type = "limit";  // string | The order type, default: `\"limit\"` (optional) 
            var label = "Test Buy Order by Bot";  // string | user defined label for the order (maximum 32 characters) (optional) 
            var price = 45000.0M;  // decimal? | <p>The order price in base currency (Only for limit and stop_limit orders)</p> <p>When adding order with advanced=usd, the field price should be the option price value in USD.</p> <p>When adding order with advanced=implv, the field price should be a value of implied volatility in percentages. For example,  price=100, means implied volatility of 100%</p> (optional) 
           // var timeInForce = timeInForce_example;  // string | <p>Specifies how long the order remains in effect. Default `\"good_til_cancelled\"`</p> <ul> <li>`\"good_til_cancelled\"` - unfilled order remains in order book until cancelled</li> <li>`\"fill_or_kill\"` - execute a transaction immediately and completely or not at all</li> <li>`\"immediate_or_cancel\"` - execute a transaction immediately, and any portion of the order that cannot be immediately filled is cancelled</li> </ul> (optional)  (default to good_til_cancelled)
           // var maxShow = 8.14;  // decimal? | Maximum amount within an order to be shown to other customers, `0` for invisible order (optional)  (default to 1M)
           // var postOnly = true;  // bool? | <p>If true, the order is considered post-only. If the new price would cause the order to be filled immediately (as taker), the price will be changed to be just below the bid.</p> <p>Only valid in combination with time_in_force=`\"good_til_cancelled\"`</p> (optional)  (default to true)
           // var reduceOnly = true;  // bool? | If `true`, the order is considered reduce-only which is intended to only reduce a current position (optional)  (default to false)
            var stopPrice = 45000.0M;  // decimal? | Stop price, required for stop limit orders (Only for stop orders) (optional) 
          //  var trigger = trigger_example;  // string | Defines trigger type, required for `\"stop_limit\"` order type (optional) 
          //  var advanced = advanced_example;  // string | Advanced option order type. (Only for options) (optional) 

            try {
                // Places a buy order for an instrument.
                //privateApiInstance.Configuration.AccessToken = Configuration.Default.AccessToken;
                MessageBox.Show("access token: " + privateApiInstance.Configuration.AccessToken);
              //  MessageBox.Show(privateApiInstance.GetBasePath());
                
                foreach (string key in  Configuration.Default.DefaultHeader.Keys) {
                  if(  privateApiInstance.DefaultHeader().TryGetValue(key, out string value)) {
                        MessageBox.Show("key: " + key + " value: " + value);
                    }
                }
                //   Configuration.Default.AddDefaultHeader("authorization", "bearer " + accessToken); ;
               
             //  apiInstance.AddDefaultHeader("Access-Control-Allow-Origin","*");
                //  privateApiInstance.AddDefaultHeader("scope", "trade:read_write");
                //ApiClient.Default.con = Configuration;
              //  MessageBox.Show(ApiClient.Default.Configuration.AccessToken);
                Object result = apiInstance.PrivateBuyGetWithHttpInfo(instrumentName, amount, type, label, price, string.Empty, null, null,null, stopPrice, string.Empty, string.Empty);
                MessageBox.Show(result.ToString());
                Clipboard.SetText(result.ToString());
            } catch (ApiException ex) {
              
                MessageBox.Show("Exception when calling PrivateApi.PrivateBuyGet: " + ex.Message);
                MessageBox.Show("Status Code: " + ex.ErrorCode);
                MessageBox.Show(ex.StackTrace);
          
                
            }
        }
    }

    public class AuthResult {
        [JsonProperty("token_type")]
        public string TokenType;

        [JsonProperty("state")]
        public string State;

        [JsonProperty("scope")]
        public string Scope;

        [JsonProperty("refresh_token")]
        public string RefreshToken;

        [JsonProperty("expires_in")]
        public int ExpiresIn;

        [JsonProperty("access_token")]
        public string AccessToken;
    }

    public class RootAuthApi {
        [JsonProperty("jsonrpc")]
        public string Jsonrpc;

        [JsonProperty("result")]
        public AuthResult Result;

        [JsonProperty("usIn")]
        public long UsIn;

        [JsonProperty("usOut")]
        public long UsOut;

        [JsonProperty("usDiff")]
        public int UsDiff;

        [JsonProperty("testnet")]
        public bool Testnet;
    }



    public class ForeignJsonNetContainerConverter : ForeignJsonNetBaseConverter {
        static readonly string[] Names = new[]
        {
        "Newtonsoft.Json.Linq.JObject",
        "Newtonsoft.Json.Linq.JArray",
        "Newtonsoft.Json.Linq.JConstructor",
        "Newtonsoft.Json.Linq.JRaw",
    };

        protected override IReadOnlyCollection<string> TypeNames { get { return Names; } }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            var json = value.ToString();
            // Fix indentation
            using (var stringReader = new StringReader(json))
            using (var jsonReader = new JsonTextReader(stringReader)) {
                writer.WriteToken(jsonReader);
            }
        }
    }

    public class ForeignJsonNetValueConverter : ForeignJsonNetBaseConverter {
        static readonly string[] Names = new[]
        {
        "Newtonsoft.Json.Linq.JValue",
    };

        protected override IReadOnlyCollection<string> TypeNames { get { return Names; } }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            var underlyingValue = ((dynamic)value).Value;
            if (underlyingValue == null) {
                writer.WriteNull();
            } else {
                // JValue.ToString() will be wrong for DateTime objects, we need to serialize them instead.
                serializer.Serialize(writer, underlyingValue);
            }
        }
    }

    public abstract class ForeignJsonNetBaseConverter : Newtonsoft.Json.JsonConverter {
        protected abstract IReadOnlyCollection<string> TypeNames { get; }

        public override bool CanConvert(Type objectType) {
            if (objectType.IsPrimitive)
                return false;
            // Do not use the converter for Native JToken types, only non-native types with the same name(s).
            if (objectType == typeof(JToken) || objectType.IsSubclassOf(typeof(JToken)))
                return false;
            var fullname = objectType.FullName;
            if (TypeNames.Contains(fullname))
                return true;
            return false;
        }

        public override bool CanRead { get { return false; } }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            throw new NotImplementedException();
        }
    }

}
