using JsonData.Models;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;

var connection = new SqliteConnection("Data Source=avtotest.db");
connection.Open();
var command = connection.CreateCommand();
CreateTable();
void CreateTable()
{
   
     command.CommandText = "CREATE TABLE IF NOT EXISTS " +
                           " questions(id INTEGER PRIMARY KEY AUTOINCREMENT," +
                           " question_text TEXT," +
                           "description TEXT," +
                           " media TEXT) "; 
    command.ExecuteNonQuery();
 /*   command.CommandText = "CREATE TABLE IF NOT EXISTS " +
                          "choices(id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                          "text TEXT, " +
                          "answer BOOLEAN, " +
                          "questionId INTEGER)";
    command.ExecuteNonQuery();*/
    CreateChoice();
}

void CreateChoice()
{
    command.CommandText = "CREATE TABLE IF NOT EXISTS choices(id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                          "text TEXT , answer BOOLEAN, questionid INTEGER ) ";
    command.ExecuteNonQuery();
}

var jsondata = "JsonData/uzlotin.json";
var jsonreader = File.ReadAllText(jsondata);
var Questions = JsonConvert.DeserializeObject<List<QuestionEntity>>(jsonreader);



if (Questions == null)
{
    Console.WriteLine("question null");
}
Console.WriteLine(Questions?.Count);
foreach (var question in Questions)
{
    AddQuestion(question);
}

void AddQuestion(QuestionEntity question)
{

    if (question.Media?.Name == null)
    {
        command.CommandText = "INSERT INTO questions (id, question_text, description) " +
                              $"VALUES({question.Id}, \"{question.Question}\", \"{question.Description}\")";
    }
    else
    {
        command.CommandText = "INSERT INTO questions (id, question_text, description, media ) " +
                              $"VALUES({question.Id}, \"{question.Question}\", \"{question.Description}\", '{question.Media.Name}')";
    }

    command.ExecuteNonQuery();
    AddChoices(question.Choices,question.Id);
}

void AddChoices(List<Choice> choices, int questionid)
{
    foreach (var choice in choices)
    {
        command.CommandText = "INSERT INTO choices(text, answer, questionid)" +
                              $"VALUES(\"{choice.Text}\", {choice.Answer},{questionid})";
        command.ExecuteNonQuery();
    }
}

