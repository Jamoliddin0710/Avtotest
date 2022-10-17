namespace Avtotest_WEB_FULLY.Models
{
    public class Ticket
    {
        public int id { get; set; }
        public int User_id { get; set; }
        public int FromIndex { get; set; }
        public int QuestionCount { get; set; }
        public int CorrectAnswerCount { get; set; }
        public bool istraining { get; set; }
        public Ticket( int user_id, int fromIndex, int questionCount)
        {
          
            this.User_id = user_id;
            FromIndex = fromIndex;
            QuestionCount = questionCount;
            CorrectAnswerCount = 0;
        }

        public Ticket()
        {

        }
    }
}
