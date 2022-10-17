
using Avtotest_WEB_FULLY.Models;
using Microsoft.Data.Sqlite;

namespace Avtotest_WEB_FULLY.Repositories
{
    public class TicketRepository
    {
        private string _connectionstring = "Data Source=avtotest.db";
        private SqliteConnection _connection;

        public TicketRepository()
        {
            
            _connection = new SqliteConnection(_connectionstring);
            CreateTicketTable();

        }

        private void CreateTicketTable()
        {
            _connection.Open();
            var command = _connection.CreateCommand();
            command.CommandText = "CREATE TABLE IF NOT EXISTS tickets(id INTEGER PRIMARY KEY AUTOINCREMENT , " +
                                  "user_id INTEGER , " +
                                  "from_index INTEGER , " +
                                  "questions_count INTEGER , " +
                                  "correct_count INTEGER , " +
                                  "istraining BOOLEAN )";
            command.ExecuteNonQuery();

            command.CommandText = "CREATE TABLE IF NOT EXISTS tickets_data(id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                  "ticket_id INTEGER , " +
                                  "question_id INTEGER , " +
                                  "choice_id INTEGER , " +
                                  "answer BOOLEAN )";
            command.ExecuteNonQuery();
            _connection.Close();
        }

        public List<Result> MaxCorrect()
        {
            var ticketsresult = new List<Result>();
            _connection.Open();
            var command = _connection.CreateCommand();
            command.CommandText = "Select  user_id,  SUM(correct_count) from tickets where correct_count > 0 group by user_id ORDER BY SUM(correct_count) DESC";
            var data = command.ExecuteReader();
            while (data.Read())
            {
                var ticket = new Result();
                ticket.user_id = data.GetInt32(0);
                ticket.correctanswercount = data.GetInt32(1);
                ticketsresult.Add(ticket);
            }
            _connection.Close();
            return ticketsresult;
        }

      
        public void UpdateCorrectTicket(int ticketid)
        {
            _connection.Open();
            var command = _connection.CreateCommand();
            command.CommandText = $"UPDATE tickets set correct_count = correct_count + 1 WHERE id = {ticketid}";
            command.ExecuteNonQuery();
            _connection.Close();
        }

        public int GetResponseTicketCount(int ticketid)
        {
            _connection.Open();
            int count = 0;
            var command = _connection.CreateCommand();
            command.CommandText = $"SELECT COUNT(*) from tickets_data WHERE ticket_id = {ticketid}";
            var data = command.ExecuteReader();
            while (data.Read())
            {
                count = data.GetInt32(0);
            }

            _connection.Close();
            return count;
        }

        public void InsertTicket(Ticket ticket)  // ticketlarni bazaga qo'shadi har bir ticketda  o'sha userga ticket ichidagi ma'lumotlarni qo'shadi
        // 35 ta ticketni tickets bazasiga qo'shadi 
        {
            _connection.Open();
            var cmd = _connection.CreateCommand();
            cmd.CommandText = "INSERT INTO tickets(user_id , from_index , questions_count , correct_count , istraining ) " +
                              "VALUES(@userid,@fromindex,@questioncount,@correctcount , @istraining )";
            cmd.Parameters.AddWithValue("@userid", ticket.User_id);
            cmd.Parameters.AddWithValue("@fromindex", ticket.FromIndex);
            cmd.Parameters.AddWithValue("@questioncount", ticket.QuestionCount);
            cmd.Parameters.AddWithValue("@correctcount", ticket.CorrectAnswerCount);
            cmd.Parameters.AddWithValue("@istraining", ticket.istraining);
            cmd.Prepare();

            cmd.ExecuteNonQuery();
            _connection.Close();
        }

        public void InsertTicketData(TicketData ticketdata)
        {
            _connection.Open();
            var command = _connection.CreateCommand();
            command.CommandText = "INSERT INTO tickets_data(ticket_id ,question_id ,choice_id ,answer )" +
                                  "VALUES(@ticketid , @questionid , @choiceid , @answer)";
            command.Parameters.AddWithValue("@ticketid", ticketdata.ticketid);
            command.Parameters.AddWithValue("@questionid", ticketdata.questionid);
            command.Parameters.AddWithValue("@choiceid", ticketdata.choiceid);
            command.Parameters.AddWithValue("@answer", ticketdata.answer);
            command.Prepare();
            command.ExecuteNonQuery();
            _connection.Close();
        }

        public List<TicketData> GetTicketDataById(int ticketid)
        {
            List<TicketData> _ticketdatalist = new List<TicketData>();
            _connection.Open();
            var command = _connection.CreateCommand();
            command.CommandText = $"SELECT * FROM tickets_data WHERE ticket_id ={ticketid}";
            var data = command.ExecuteReader();
            while (data.Read())
            {
                var ticketdata = new TicketData();
                ticketdata.id = data.GetInt32(0);
                ticketdata.ticketid = data.GetInt32(1);
                ticketdata.questionid = data.GetInt32(2);
                ticketdata.choiceid = data.GetInt32(3);
                ticketdata.answer = data.GetBoolean(4);
                _ticketdatalist.Add(ticketdata);
            }

            _connection.Close();
            return _ticketdatalist;
        }

        public TicketData? GetTicketDatByQuestionId(int ticketid, int questionid)
        // user javob belgilaganda u savolga javob berdi
        // yoki yo'q shuni tekshirish uchun kerak
        {
            _connection.Open();
            var command = _connection.CreateCommand();
            var ticketdata = new TicketData();
            command.CommandText = "SELECT * FROM tickets_data WHERE question_id=@questionid AND ticket_id = @ticketid";
            command.Parameters.AddWithValue("questionid", questionid);
            command.Parameters.AddWithValue("@ticketid", ticketid);
            command.Prepare();

            var data = command.ExecuteReader();

            while (data.Read())
            {

                ticketdata.id = data.GetInt32(0);
                ticketdata.ticketid = data.GetInt32(1);
                ticketdata.questionid = data.GetInt32(2);
                ticketdata.choiceid = data.GetInt32(3);
                ticketdata.answer = data.GetBoolean(4);
                _connection.Close();
                return ticketdata;
            }

            _connection.Close();
            return null;
        }

        public Ticket GetTicketById(int id, int user_id) //userga tanlangan ticketga mos
                                                         //savollardan olib berish vazifasini  bajaradi
        {
            _connection.Open();
            var ticket = new Ticket();
            var command = _connection.CreateCommand();
            command.CommandText = $"SELECT * FROM tickets WHERE id={id} AND user_id={user_id}";
         
            var data = command.ExecuteReader();
            while (data.Read())
            {
                ticket.id = data.GetInt32(0);
                ticket.User_id = data.GetInt32(1);
                ticket.FromIndex = data.GetInt32(2);
                ticket.QuestionCount = data.GetInt32(3);
                ticket.CorrectAnswerCount = data.GetInt32(4);
                ticket.istraining = data.GetBoolean(5);

            }

            _connection.Close();
            return ticket;
        }

        public int GetLastrowId() // bu oxirgi ticket indeksini olish uchun kerak
        {
            _connection.Open();
            int id = 0;
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT id from tickets ORDER BY id DESC LIMIT 1";
            var data = command.ExecuteReader();
            while (data.Read())
            {
                id = data.GetInt32(0);
            }

            _connection.Close();
            return id;
        }

        public List<Ticket> GetTicketsByUserId(int userId)
        {
           // tickets db ga ma'lumotlarni saqlab beradi bu userni idsiga oid
           // ma'lumot olish va
           // unga o'sha ticketdan tanlab berishda qo'l keladi 
            var tickets = new List<Ticket>();
            _connection.Open();
            var cmd = _connection.CreateCommand();
            cmd.CommandText = $"SELECT id, from_index, questions_count, correct_count FROM tickets WHERE user_id = {userId} AND istraining = {true}";
            var data = cmd.ExecuteReader();
            while (data.Read())
            {
                var ticketData = new Ticket()
                {
                    id  = data.GetInt32(0),
                    FromIndex = data.GetInt32(1),
                    QuestionCount = data.GetInt32(2),
                    CorrectAnswerCount = data.GetInt32(3),
                    User_id = userId
                };
                tickets.Add(ticketData);
            }

            _connection.Close();
            return tickets;
        }

        public void InsertUserTicketsTraining(int userid , int ticket_count , int ticketquestion_count)
      // user uchun id siag asoslanib unga 35 ta ticketni generate qilib beradi  
        {
          
            for (int i = 0; i < ticket_count; i++)
            {
                InsertTicket(new Ticket
                {
                    User_id = userid,
                    CorrectAnswerCount = 0,
                    istraining = true,
                    QuestionCount = ticketquestion_count,
                    FromIndex = i * ticketquestion_count + 1,
                });
            }
         

        }
        /*  public List<Ticket> GetTicketUserId(int userid)
          {
              var tickets = new List<Ticket>();
              _connection.Open();
              var command = _connection.CreateCommand();
              command.CommandText = $"SELECT (*) FROM tickets WHERE user_id={userid} AND istraining=true";
              var data = command.ExecuteReader();
              while (data.Read())
              {
                  var ticket = new Ticket();
                  ticket.id = data.GetInt32(0);
                  ticket.FromIndex = data.GetInt32(1);
                  ticket.User_id = userid;
                  ticket.QuestionCount = data.GetInt32(2);
                  ticket.CorrectAnswerCount = data.GetInt32(3);
                  tickets.Add(ticket);
              }
              _connection.Close();
              return tickets;
          }*/
    }
}

