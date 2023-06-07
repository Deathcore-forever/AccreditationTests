using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDesign.Data
{
    public class Question
    {
        public string ID { get; }
        public string ShowNumber { get; set; }
        public string Name { get; set; }
        public int TestID { get; }
        public string[] Responses { get; }
        public string RightResponse { get; }

        public Question(int testNumber, bool isAdmin, string qID, string qShowNumber, string qName, string[] responses)
        {
            ID = qID;
            ShowNumber = qShowNumber;
            Name = qName;
            RightResponse = responses[0];
            TestID = testNumber;
            if (isAdmin) Responses = responses;
            else  Responses = responses.OrderBy(n => Guid.NewGuid()).ToArray();
        }
    }
}
