
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;

namespace cantinaScreenerConsole.Model
{
    public class JSONModel
    {
        private JObject loadedJSON;
        public JObject LoadedJSON { get => loadedJSON; set => loadedJSON = value; }

        private string searchObject = "subviews";
        
        public void LoadJSONFromURL(string url)
        {
            string res = LoadStringFromURL(url);
            LoadJSONFromString(res);
        }
        
        public void LoadJSONFromString(string value)
        {
            if (value != null)
            {
                LoadedJSON = JObject.Parse(value);
            }
        }

        public override string ToString()
        {
            if (LoadedJSON != null)
            {
                return LoadedJSON.ToString();
            }
            else
            {
                return null;
            }
        }

        /**
         * The heavy lifter of this application, the search function needs to be able to search through the JSON
         * given particular input strings.
         * 
         * Examples to support:
         * 1. class - the view class name on an object. 
         * 2. classNames - the CSS class name  (denoted with '.')
         * 3. identifier - the view identifier (denoted with '#')
         * 
         */
        public IEnumerable<JToken> SearchJSON(string input)
        {
            //Searching will be done via JSONPath, with an implementation provided via 
            //the JSON.net package. 
            //Documentation here: 
            //https://www.newtonsoft.com/json/help/html/SelectToken.htm
            //http://goessner.net/articles/JsonPath/
            //Pracitce JSONPath queries here: http://jsonpath.com/


            if (LoadedJSON == null)
                return null;            

            string jsonPathQuery = "";


            if (input.Contains("."))
            {
                
                List<JToken> results = new List<JToken>();

                //we need to perform a compound query.
                //First we want to find any views that have class names at all
                string[] splitup = input.Split('.');

                //prepare the outer query
                jsonPathQuery = string.Format("$..{0}[?(@.class=='{1}' && @.classNames)]", searchObject, splitup[0]);
                //jsonPathQuery = string.Format("$..subviews[?(@.classNames)]");

                //Now look in those, and determine if they have a class name that matches
                IEnumerable<JToken> tempResults = LoadedJSON.SelectTokens(jsonPathQuery);
                
                if (tempResults != null)
                {
                    //$..['classNames'][?(@ == 'columns')]
                    string innerQuery = string.Format("$..['classNames'][?(@ == '{0}')]", splitup[1]);                    
                    foreach (JToken aToken in tempResults)
                    {
                        //now query to see if there are in fact class names that match.
                        string temp = aToken.ToString();
                        IEnumerable<JToken> innerResults = aToken.SelectTokens(innerQuery);
                        //This is icky.  
                        //Having trouble finding a way to compose the jsonPathquery as 1 query.                        
                        int count = 0;
                        foreach (JToken anInnerToken in innerResults)
                        {
                            count++;
                            break;//only need to see there is at least one, so lets break out!
                        }
                        if (count > 0)
                        results.Add(aToken);
                    }
                }
                return results;
            }

            else
            {
                //Does the input contain a "." or a "#"
                if (input.Contains("$"))
                {
                    //perform a raw JSONPath query
                    jsonPathQuery = input;
                }
                else if (input.Contains("#"))
                {
                    string[] splitup = input.Split('#');
                    //Identifier 
                    jsonPathQuery = string.Format("$..{0}[?(@.class=='{1}' && @.identifier=='{2}')]", searchObject, splitup[0], splitup[1]);
                }

                else
                {
                    //class
                    jsonPathQuery = string.Format("$..{0}[?(@.class=='{1}')]",searchObject, input);
                }

                return LoadedJSON.SelectTokens(jsonPathQuery);
            }

            
        }

        public static string LoadStringFromURL(string url)
        {
            using (WebClient client = new WebClient())
            {
                string s = client.DownloadString(url);
                return s;
            }
        }


    }
}
