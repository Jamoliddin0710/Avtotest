using Avtotest_WEB_FULLY.Models;
using Microsoft.Data.Sqlite;
using System.Data.Common;

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
          /*  connection.Open();
            command = connection.CreateCommand();*/
        }

        public void CreateUsersTable()
        {
            connection.Open();
            command = connection.CreateCommand();
            
            command.CommandText = "CREATE TABLE IF NOT EXISTS users(id INTEGER PRIMARY KEY AUTOINCREMENT , name TEXT , phone TEXT, password TEXT , image TEXT) ";
            command.ExecuteNonQuery();
            connection.Close();
        }

     
        public void InsertUser(User user)
        {
            connection.Open();
            command = connection.CreateCommand();
            
            command.CommandText = "INSERT INTO users(name, phone, password , image)" +
                                  $"VALUES('{user.Name}', '{user.Phone}' ,'{user.Password}', '{user.Image}' )";
            // userlarni qo'shadi
            command.ExecuteNonQuery();
            connection.Close();
        }

        public List<User> GetUsers()
        {
            connection.Open();
            command = connection.CreateCommand();

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
            connection.Close();
            return users;
        }

        public User GetUserByIndex(int index)
        {
            connection.Open();
            command = connection.CreateCommand();

            command.CommandText = $"SELECT FROM users WHERE id={index}";
            command.ExecuteNonQuery();
            var data = command.ExecuteReader();
            var user = new User();
            while (data.Read())
            {

                user.Index = data.GetInt32(0);
                user.Name = data.GetString(1);
                user.Phone = data.GetString(2);
                user.Image = data.GetString(3);
            }
            // userlarni index bo'yicha qaytaradi
            connection.Close();
            return user;
        }

        public User GetUserByPhoneNumber(string phoneNumber)
        {
            connection.Open();
            command = connection.CreateCommand();

            var user = new User();
            command.CommandText = $"SELECT * FROM users WHERE phone = '{phoneNumber}'";
            var data = command.ExecuteReader();
            while (data.Read())
            {
                user.Index = data.GetInt32(0);
                user.Name = data.GetString(1);
                user.Phone = data.GetString(2);
                user.Password = data.GetString(3);
                user.Image = data.GetString(4);
            }
            //userlarni telefon nomer bo'yicha qaytaradi
            connection.Close();
            return user;
        }

        public string? Name(int userid)
        {
            string name;
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = $"SELECT name FROM users WHERE id = {userid}";
            var data = command.ExecuteReader();
            while (data.Read())
            {
                name = data.GetString(0);
                connection.Close();
                return name;
            }
            connection.Close();
            return null;
        }
        public void DeleteUser(int index)
        {
            connection.Open();
            command = connection.CreateCommand();

            command.CommandText = $"DELETE FROM users WHERE id={index}";
            command.ExecuteNonQuery();
            connection.Close();
            //userlarni index bo'yicha o'chiradi          
        }

        public void UpdateUser(User user)
        {
            connection.Open();
            command = connection.CreateCommand();

            command.CommandText =
                $"UPDATE users SET name=@name , phone=@phone, password=@password , image=@image WHERE id=@userid ";
            command.Parameters.AddWithValue("@name", user.Name);
            command.Parameters.AddWithValue("@phone", user.Phone);
            command.Parameters.AddWithValue("@password", user.Password);
            command.Parameters.AddWithValue("@image", user.Image);
            command.Parameters.AddWithValue("@userid", user.Index);
            command.Prepare();
            command.ExecuteNonQuery();
            connection.Close();
            // userni yangilaydi 
        }
    }
}
