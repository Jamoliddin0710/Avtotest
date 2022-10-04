using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace JsonData.Models
{
    public class QuestionRepository
    {
        public List<QuestionEntity> Questions;
        public int QuestionsCount;

        //public List<Ticket> UserTicket = new List<Ticket>();
        public QuestionRepository()
        {
            ReadQuestionJson();

        }
        public void ReadQuestionJson()
        {
            var jsonPath = File.ReadAllText("C:\\Users\\User\\source\\repos\\Autotest_WPF\\Autotest_WPF\\JsonData\\uzlotin.json");
            Questions = JsonConvert.DeserializeObject<List<QuestionEntity>>(jsonPath);

        }

    }
}
