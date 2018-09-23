using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using cantinaScreenerConsole.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace screenerTestProject1
{
    [TestClass]
    public class JSONModelTest
    {
        const string testURL = "https://raw.githubusercontent.com/jdolan/quetoo/master/src/cgame/default/ui/settings/SystemViewController.json";
        
        //A little self-referential, but it should at least ensure our test data loaded ok.
        [TestMethod]
        public void InternalLoadTestStringFunctioning()
        {
            string result = LoadTestStringFromURL();
            Assert.IsNotNull(result);
        }


        //ensure the JSONModel throws with bogus URL
        [TestMethod]
        [ExpectedException(typeof(System.Net.WebException))]
        public void LoadStringThrowsWithBadURL()
        {
            JSONModel model = new JSONModel();            
            string result = JSONModel.LoadStringFromURL("bad");
        }

        //ensure the JSONModel can load a string from URL
        [TestMethod]
        public void CanLoadStringFromURL()
        {
            JSONModel model = new JSONModel();
            string expected = LoadTestStringFromURL();
            string result = JSONModel.LoadStringFromURL(testURL);            
            Assert.AreEqual(expected, result);
        }

        //ensure the JSONModel can parse a string into a JObject
        [TestMethod]
        public void CanParseIntoJObject()
        {
            JSONModel model = new JSONModel();
            string testString = LoadTestStringFromURL();

            model.LoadJSONFromString(testString);

            Assert.IsNotNull(model.LoadedJSON);

            string expected = LoadTestStringFromFile("../../full_json.json");
            Assert.AreEqual(expected, model.ToString());
        }

        //JSModel throws when cannot parse valid JSON
        [TestMethod]
        [ExpectedException(typeof(Newtonsoft.Json.JsonReaderException))]        
        public void CanParseIntoJObjectThrowsWithNonJSON()
        {
            JSONModel model = new JSONModel();
            string testString = "[NotJSON";
            model.LoadJSONFromString(testString);
        }

        [TestMethod]
        public void CanFindViewsByClassName()
        {
            JSONModel model = PrepareModel();

            IEnumerable<JToken> tokens = model.SearchJSON("Input");

            int count = 0;
            JToken first = null;
            foreach (JToken token in tokens)
            {
                if (count == 0)
                {
                    first = token;
                }
                count++;
            }

            Assert.AreEqual(26, count);
            string expectedString = LoadTestStringFromFile("../../first_class_input.json");
            Assert.AreEqual(expectedString, first.ToString());
        }

        [TestMethod]
        public void CanFindViewsByClassNameWillNotFindMismatches()
        {
            JSONModel model = PrepareModel();

            IEnumerable<JToken> tokens = model.SearchJSON("NotPresent");

            int count = 0;
            foreach (JToken token in tokens)
            {
                count++;
            }

            Assert.AreEqual(0, count);
        }

        [TestMethod]
        public void CanFindViewsByIdentifier()
        {
            JSONModel model = PrepareModel();

            IEnumerable<JToken> tokens = model.SearchJSON("Button#apply");

            int count = 0;
            JToken first = null;
            foreach (JToken token in tokens)
            {
                if (count ==0)
                {
                    first = token;
                }
                count++;
            }

            Assert.AreEqual(1, count);

            string expectedString = LoadTestStringFromFile("../../button_apply.json");
            Assert.AreEqual(expectedString, first.ToString());            
        }
        

        [TestMethod]
        public void CanFindViewsByCSSClassName()
        {
            JSONModel model = PrepareModel();

            IEnumerable<JToken> tokens = model.SearchJSON("StackView.container");

            int count = 0;
            JToken first = null;
            foreach (JToken token in tokens)
            {
                if (count == 0)
                {
                    first = token;
                }
                count++;
            }

            Assert.AreEqual(6, count);

            string expectedString = LoadTestStringFromFile("../../first_stackview_container.json");
            Assert.AreEqual(expectedString, first.ToString());
        }

        //Many tests utilize the remote string
        private string LoadTestStringFromURL()
        {            
            using (WebClient client = new WebClient())
            {
                string s = client.DownloadString(testURL);
                return s;
            }
        }

        private string LoadTestStringFromFile(string fileName)
        {
            string contents = File.ReadAllText(fileName);
            return contents;
        }

        private JSONModel PrepareModel()
        {
            JSONModel model = new JSONModel();
            model.LoadJSONFromURL(testURL);
            return model;
        }
    }
}
