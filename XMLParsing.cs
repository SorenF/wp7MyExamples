/*XML format
   <?xml version="1.0" encoding="UTF-8" standalone="no"?>
<survey display="1-1" id="1296847347" title="Apt Super Powers"
deployed="true" checksum="a14fbb24e655cc10ea9b5645043aabde">
  <category id="1" name="Questions about apt">
    <question id="1" type="_choice" field="" direction="in" editable="false">
      <description>apts super powers are like</description>
      <select>multiple</select>
      <item otr="0">a cow</item>
      <item otr="0">a horse</item>
      <item otr="0">a sheep</item>
      <item otr="0">apt is all powerful</item>
    </question>
  </category>
</survey>
  */

private void ResponseCallback(IAsyncResult result)
        {
            var request = (HttpWebRequest)result.AsyncState;
            var response = request.EndGetResponse(result);

            using (var stream = response.GetResponseStream())
            using (var reader = new StreamReader(stream))
            {
                var contents = reader.ReadToEnd();
                contents = contents.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\"?>", "");
                contents = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\"?><surveys>" + contents + "</surveys>";

                XDocument xd = XDocument.Parse(contents);
                XElement root = xd.Element("surveys");

                String surveyContent = String.Empty;

                var survey = from e in root.Elements()
                              where e.Attribute("id").Value == "1263929563"
                              select e;
                var categories = survey.Descendants("category");
                var questions = categories.Descendants("question");
               
            Dispatcher.BeginInvoke(() =>
                {
                    foreach (var i in survey)
                    {
                        surveyContent += ("Title: " + i.Attribute("title").Value + "\n" +
                                          "Id: " + i.Attribute("id").Value + "\n");
                        foreach(var j in categories)
                        {
                            surveyContent += ("Category: "+ j.Attribute("name").Value + "\n");
                            foreach (var k in questions)
                            {
                                surveyContent += ("Question#" + k.Attribute("id").Value + "   " + k.Descendants("description").First().Value + "\n" +
                                                  "Answer Type: " + k.Attribute("type").Value + "\n");
                                var items = k.Descendants("item");
                                foreach (var l in items)
                                    surveyContent += (l.Value + ",");
                                surveyContent += "\n";
                            }
                        }
                    }
                    httpWebRequestTextBlock.Text = surveyContent;});
            }
        }