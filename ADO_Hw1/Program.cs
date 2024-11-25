using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.Xml.Linq;


namespace ADO_Hw1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=VegetablesAndFruits;Integrated Security=True;Connect Timeout=30;";

                Console.WriteLine("1. Connect to Database\n" +
                "2. Display all records\n" +
                "3. Display names only\n" +
                "4. Display colors only\n" +
                "5. Display max calories\n" +
                "6. Display min calories\n" +
                "7. Display average calories\n" +
                "8. Display vegetable count\n" +
                "9. Display fruit count\n" +
                "10. Display items by color\n" +
                "11. Display items below specific calories\n" +
                "12. Display items within calorie range\n" +
                "0. Exit");

            int MenyChoise;
            do
            {
                Console.Write("\nEnter your choice: ");
                MenyChoise = int.Parse(Console.ReadLine());

                switch (MenyChoise)
                {
                    case 1:
                        {
                            try
                            {
                                using (SqlConnection connection = new SqlConnection(ConnectionString))
                                {
                                    connection.Open();
                                    Console.WriteLine("Successfully connected to the database.");
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}");
                            }
                        }
                        break;
                    case 2:
                        {
                            string query1 = "SELECT * FROM Items";

                            using (SqlConnection connection = new SqlConnection(ConnectionString))
                            using (SqlCommand command = new SqlCommand(query1, connection))
                            {
                                connection.Open();
                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    Console.WriteLine("\nAll Records:");
                                    while (reader.Read())
                                    {
                                        Console.WriteLine($"Id: {reader["Id"]}, Name: {reader["Name"]}, Type: {reader["Type"]}, Color: {reader["Color"]}, Calories: {reader["Calories"]}");
                                    }
                                }
                            }
                        }
                        break;
                    case 3:
                        DisplayColumn("Name");
                        break;
                    case 4:
                        DisplayColumn("Color");
                        break;
                    case 5:
                        ExecuteScalarQuery("SELECT MAX(Calories) FROM Items", "Maximum Calories");
                        break;
                    case 6:
                        ExecuteScalarQuery("SELECT MIN(Calories) FROM Items", "Minimum Calories");
                        break;
                    case 7:
                        ExecuteScalarQuery("SELECT AVG(Calories) FROM Items", "Average Calories");
                        break;
                    case 8:
                        DisplayCount("Vegetable");
                        break;
                    case 9:
                        DisplayCount("Fruit");
                        break;
                    case 10:
                        {
                            Console.Write("Enter a color: ");
                        string color = Console.ReadLine();

                        string query2 = "SELECT * FROM Items WHERE Color = @Color";

                            using (SqlConnection connection = new SqlConnection(ConnectionString))
                            using (SqlCommand command = new SqlCommand(query2, connection))
                            {
                                command.Parameters.AddWithValue("@Color", color);
                                connection.Open();

                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    Console.WriteLine($"Items with color {color}:");
                                    while (reader.Read())
                                    {
                                        Console.WriteLine($"Id: {reader["Id"]}, Name: {reader["Name"]}, Type: {reader["Type"]}, Calories: {reader["Calories"]}");
                                    }
                                }
                            }
                        }
                        break;
                    case 11:
                        {
                            Console.Write("Enter maximum calories: ");
                            int maxCalories = int.Parse(Console.ReadLine() ?? "0");

                            string query3 = "SELECT * FROM Items WHERE Calories < @MaxCalories";

                            using (SqlConnection connection = new SqlConnection(ConnectionString))
                            using (SqlCommand command = new SqlCommand(query3, connection))
                            {
                                command.Parameters.AddWithValue("@MaxCalories", maxCalories);
                                connection.Open();

                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    Console.WriteLine($"Items with calories below {maxCalories}:");
                                    while (reader.Read())
                                    {
                                        Console.WriteLine($"Id: {reader["Id"]}, Name: {reader["Name"]}, Type: {reader["Type"]}, Calories: {reader["Calories"]}");
                                    }
                                }
                            }
                        }
                        break;
                    case 12:
                        {
                            Console.Write("Enter minimum calories: ");
                        int minCalories = int.Parse(Console.ReadLine());

                        Console.Write("Enter maximum calories: ");
                        int maxCalories = int.Parse(Console.ReadLine());

                        string query = "SELECT * FROM Items WHERE Calories BETWEEN @MinCalories AND @MaxCalories";

                            using (SqlConnection connection = new SqlConnection(ConnectionString))
                            using (SqlCommand command = new SqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@MinCalories", minCalories);
                                command.Parameters.AddWithValue("@MaxCalories", maxCalories);
                                connection.Open();

                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    Console.WriteLine($"Items with calories between {minCalories} and {maxCalories}:");
                                    while (reader.Read())
                                    {
                                        Console.WriteLine($"Id: {reader["Id"]}, Name: {reader["Name"]}, Type: {reader["Type"]}, Calories: {reader["Calories"]}");
                                    }
                                }
                            }
                        }
                        break;
                    case 0:
                        Console.WriteLine("Exiting...");
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            } while (MenyChoise != 0);

        }
        static void DisplayColumn(string column)
        {
            string ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=VegetablesAndFruits;Integrated Security=True;Connect Timeout=30;";

            string query = $"SELECT DISTINCT {column} FROM Items";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                Console.WriteLine($"\n{column}s:");
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine(reader[column]);
                    }
                }
            }
        }
        static void DisplayCount(string type)
        {
            string ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=VegetablesAndFruits;Integrated Security=True;Connect Timeout=30;";

            string query = "SELECT COUNT(*) FROM Items WHERE Type = @Type";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Type", type);
                connection.Open();

                int count = (int)command.ExecuteScalar();
                Console.WriteLine($"Number of {type}s: {count}");
            }
        }
        static void ExecuteScalarQuery(string query, string label)
        {
            string ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=VegetablesAndFruits;Integrated Security=True;Connect Timeout=30;";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                var result = command.ExecuteScalar();
                Console.WriteLine($"{label}: {result}");
            }
        }
    }
}



