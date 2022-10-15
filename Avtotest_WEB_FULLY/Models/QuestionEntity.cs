using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avtotest_WEB_FULLY.Models
{
    public class QuestionEntity
    {

        public int Id { get; set; }
        public string? Question { get; set; }
        public string Description { get; set; }
        public List<Choice> Choices { get; set; }
        public string? Media { get; set; }

    }

 
    public class Choice
    {
        public  int id { get; set; }
        public string Text { get; set; }
        public bool Answer { get; set; }

    }
}
