using StudentInformationSystem.Entity;
using StudentInformationSystem.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInformationSystem.BusinessLayer.Repository
{
    public class StudentRepository : IStudentRepository
    {
        public readonly string _connectionString;

        public StudentRepository()
        {
            _connectionString = DbConnUtil.GetConnString();
        }

        public Student GetStudentById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT * FROM Students WHERE student_id = @StudentId", connection))
                {
                    command.Parameters.AddWithValue("@StudentId", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Student(
                                //reader.GetInt32(reader.GetOrdinal("student_id")),
                                reader.GetString(reader.GetOrdinal("first_name")),
                                reader.GetString(reader.GetOrdinal("last_name")),
                                reader.GetDateTime(reader.GetOrdinal("date_of_birth")),
                                reader.GetString(reader.GetOrdinal("email")),
                                reader.GetString(reader.GetOrdinal("phone_number")),
                                reader.GetInt32(reader.GetOrdinal("student_id"))
                            );
                        }
                    }
                }
            }
            return null;
        }

        public IEnumerable<Student> GetAllStudents()
        {
            var students = new List<Student>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT * FROM Students", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            students.Add(new Student(
                                reader.GetInt32(reader.GetOrdinal("student_id")),
                                reader.GetString(reader.GetOrdinal("first_name")),
                                reader.GetString(reader.GetOrdinal("last_name")),
                                reader.GetDateTime(reader.GetOrdinal("date_of_birth")),
                                reader.GetString(reader.GetOrdinal("email")),
                                reader.GetString(reader.GetOrdinal("phone_number"))
                            ));
                        }
                    }
                }
            }
            return students;
        }

        public void AddStudent(Student student)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("INSERT INTO Students (first_name, last_name, date_of_birth, email, phone_number) VALUES (@FirstName, @LastName, @DateOfBirth, @Email, @PhoneNumber)", connection))
                {
                    command.Parameters.AddWithValue("@FirstName", student.FirstName);
                    command.Parameters.AddWithValue("@LastName", student.LastName);
                    command.Parameters.AddWithValue("@DateOfBirth", student.DateOfBirth);
                    command.Parameters.AddWithValue("@Email", student.Email);
                    command.Parameters.AddWithValue("@PhoneNumber", student.PhoneNumber);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public void UpdateStudent(Student student)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("UPDATE Students SET first_name = @FirstName, last_name = @LastName, date_of_birth = @DateOfBirth, email = @Email, phone_number = @PhoneNumber WHERE student_id = @StudentId", connection))
                {
                    command.Parameters.AddWithValue("@StudentId", student.StudentId);
                    command.Parameters.AddWithValue("@FirstName", student.FirstName);
                    command.Parameters.AddWithValue("@LastName", student.LastName);
                    command.Parameters.AddWithValue("@DateOfBirth", student.DateOfBirth);
                    command.Parameters.AddWithValue("@Email", student.Email);
                    command.Parameters.AddWithValue("@PhoneNumber", student.PhoneNumber);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteStudent(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("DELETE FROM Students WHERE student_id = @StudentId", connection))
                {
                    command.Parameters.AddWithValue("@StudentId", id);
                    command.ExecuteNonQuery();
                }
            }

        }
    }
}
