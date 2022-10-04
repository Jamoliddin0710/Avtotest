using Avtotest_WEB_FULLY.Models;
using Microsoft.Data.Sqlite;

namespace Avtotest_WEB_FULLY.Repositories
{
    public class UsersRepository
    {
        private const string ConnectionString = "Data Source=users.db";
        private SqliteConnection connection;
        private SqliteCommand command;

        public UsersRepository()
        {
            OpenConnection();
            CreateUsersTable();
        }

        public void OpenConnection()
        {
            connection = new SqliteConnection(ConnectionString);
            connection.Open();
            command = connection.CreateCommand();
          
        }

        public void CreateUsersTable()
        {
            command.CommandText = "CREATE TABLE IF NOT EXISTS users(id INTEGER PRIMARY KEY AUTOINCREMENT , name TEXT , phone TEXT, password TEXT) ";
            command.ExecuteNonQuery();
        }

        public void InsertUser(User user)
        {
            command.CommandText = "INSERT INTO users(name, phone, password)" +
                                  $"VALUES('{user.Name}', '{user.Phone}' ,'{user.Password}' )";
            // userlarni qo'shadi
            command.ExecuteNonQuery();
        }

        public List<User> GetUsers()
        {
            var users = new List<User>();
            command.CommandText = "SELECT * FROM users";
            var data = command.ExecuteReader();
            while (data.Read())
            {
                var user = new User();
                user.Index = data.GetInt32(0);
                user.Name = data.GetString(1);
                user.Phone = data.GetString(2);
                users.Add(user);
            }
            // hamma userlarni qaytaradi
            return users;
        }

        public User GetUserByIndex(int index)
        {
            command.CommandText = $"SELECT FROM users WHERE id={index}";
            command.ExecuteNonQuery();
            var data = command.ExecuteReader();
            var user = new User();
            while (data.Read())
            {
              
                user.Index = data.GetInt32(0);
                user.Name = data.GetString(1);
                user.Phone = data.GetString(2);
            }
            // userlarni index bo'yicha qaytaradi
            return user;
        }

        public User GetUserByPhoneNumber(string phoneNumber)
        {
            var user = new User();
            command.CommandText = $"SELECT * FROM users WHERE phone = '{phoneNumber}'";
            var data = command.ExecuteReader();
            while (data.Read())
            {
                user.Index = data.GetInt32(0);
                user.Name = data.GetString(1);
                user.Phone = data.GetString(2);
                user.Password = data.GetString(3);
            }
            //userlarni telefon nomer bo'yicha qaytaradi
            return user;
        }

        public void DeleteUser(int index)
        {
            command.CommandText = $"DELETE FROM users WHERE id={index}";
            command.ExecuteNonQuery();
            //userlarni index bo'yicha o'chiradi          
        }

        public void UpdateUser(User user)
        {
            command.CommandText =
                $"UPDATE FROM users SET name='{user.Name}', phone='{user.Phone}', password='{user.Password}' WHERE id={user.Index}";
            command.ExecuteNonQuery();
            // userni yangilaydi 
        }
    }
}
