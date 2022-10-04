using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonData.Models
{
    public class QuestionEntity
    {

        public int Id { get; set; }
        public string? Question { get; set; }
        public string Description { get; set; }
        public List<Choice> Choices { get; set; }
        public Media? Media { get; set; }

    }

    public class Media
    {
        public bool Exist { get; set; }
        public string? Name { get; set; }
    }
    public class Choice
    {
        public string? Text { get; set; }
        public bool Answer { get; set; }

    }
}
