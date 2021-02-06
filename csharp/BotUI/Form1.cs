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

namespace BotUI {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        private void button1_Click(object sender, EventArgs e) {

        }

        private void Form1_Load(object sender, EventArgs e) {

            Configuration.Default.BasePath = "https://www.deribit.com/api/v2";
            // Configure HTTP basic authorization: bearerAuth
            Configuration.Default.Username = "l3oxergames@gmail.com";
            Configuration.Default.Password = "@Aa909090";
            Configuration.Default.AccessToken = "dFDXK2tOnwFfVWwhlGKOg3r5KDJy0eQnHWzTKMfO9wY";
            




            var accountApiInstance = new AccountManagementApi(Configuration.Default);
            var sid = 56;  // int? | The user id for the subaccount

            
            //var apiInstance = new AuthenticationApi(Configuration.Default);
           
           

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
    }
}
