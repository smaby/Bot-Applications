using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockWithLUIS.JSON
{

    public class StockLUIS
    {
        /// <summary>
        /// This is an query
        /// </summary>
        public string text { get; set; }

        public Entity[] entities
        { get; set; }

        public Intent[] intents { get; set; }

              
    }

    /// <summary>
    /// Defines structure of Entity. THis is defined from JSON file created from LUIS
    /// </summary>
    public class Entity
    {
        public string entity { get; set; }
        public string type { get; set; }
        public int startIndex { get; set; }
        public int endIndex { get; set; }
        public double score { get; set; }
        
    }

    /// <summary>
    /// Defines Structure of Indent. This comes from the JSON file created from the LUIS
    /// </summary>
    public class Intent
    {
        public string intent { get; set; }
        public double score { get; set; }

    }
}
