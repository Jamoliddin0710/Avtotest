
using Microsoft.Data.Sqlite;
using System.Data.Common;

namespace Avtotest_WEB_FULLY.Models
{
    public class QuestionRepository
    {
        private string sqlite = "Data Source=Avtotest.Db";
        private SqliteConnection _connection;
        
        public QuestionRepository()
        {
            _connection = new SqliteConnection(sqlite);
        }

        public int GetQuestionCount()
        { 
            _connection.Open();
            var command = _connection.CreateCommand();
            
           command.CommandText = "SELECT COUNT(*) FROM questions";
            var data = command.ExecuteReader();
           
            while (data.Read())
            {
                var count = data.GetInt32(0);
                _connection.Close();
                data.Close();
                return count;
            }
            _connection.Close();
            return 0;
        }

        public List<QuestionEntity> GetQuestionsRange(int from, int count)
        {
            var Questions = new List<QuestionEntity>();
            for (int i = from; i < from + count; i++)
            {
                Questions.Add(GetQuestionById(i));
            }

            return Questions;
        }

        public QuestionEntity GetQuestionById(int id)
        {
            _connection.Open();
            var question = new QuestionEntity();
            var command = _connection.CreateCommand();
            command.CommandText = $"SELECT * FROM questions WHERE id={id}";
            var data = command.ExecuteReader();
            while (data.Read())
            {
                question.Id = data.GetInt32(0);
                question.Question = data.GetString(1);
                question.Description = data.GetString(2);
                question.Media = data.IsDBNull(3) ? null : data.GetString(3);
                // question.Media tekshirish 
            }
            question.Choices = GetQuestionChoices(id);
            _connection.Close();
           return question;
        }

        public List<Choice> GetQuestionChoices(int id)
        {
            var choices = new List<Choice>();
            var command = _connection.CreateCommand();
            command.CommandText = $"SELECT * FROM choices WHERE questionid={id}";
            var data = command.ExecuteReader();
            while (data.Read())
            {
                var choice = new Choice();
                choice.id = data.GetInt32(0);
                choice.Text = data.GetString(1);
                choice.Answer = data.GetBoolean(2);

                if (choices.Any(ch => ch.Text == choice.Text))
                    break;

                choices.Add(choice);
            }

            return choices;
        }
        
    }
 
}
