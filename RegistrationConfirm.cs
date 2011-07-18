    


//It needs to be tested. I might be generating that response accidently.


        private void registerPhone(string phoneNumber, string imei)
        {
            string url = serverUrl + string.Format("RegisterIMEI?msisdn=%2b{0}&imei={1}", phoneNumber, imei);
            commandCheckTextBlock.Text = url;
            var request = HttpWebRequest.Create(url);
            var result = (IAsyncResult)request.BeginGetResponse(RegisterPhoneResponseCallback, request);

        }

        private void RegisterPhoneResponseCallback(IAsyncResult result)
        {
            var request = (HttpWebRequest)result.AsyncState;
            var response = request.EndGetResponse(result);

            using (var stream = response.GetResponseStream())
            using (var reader = new StreamReader(stream))
            {
                string output = string.Empty;
                var contents = reader.ReadToEnd();
                if (output != null)
                {
                    Encoding enc = Encoding.UTF8;
                    byte[] bytes = enc.GetBytes(contents);
                    foreach (var i in bytes)
                        output += i.ToString();
                    if (output == "0002") output +="\n Registration success.";
                    else output += "\n Registration failed.";
                } else output = "\n Response is null!";
                Dispatcher.BeginInvoke(() => { registerPhoneTextblock.Text = output; });
            }

         }