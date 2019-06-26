using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;

namespace LinqToSQL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // create an object to data context
        LinqToSQLDataClassesDataContext dataContext;
        public MainWindow()
        {
            InitializeComponent();

            // connect the database
            string connectionString = ConfigurationManager.ConnectionStrings["LinqtoSQL.Properties.Settings.BallDBConnectionString"].ConnectionString;
            dataContext = new LinqToSQLDataClassesDataContext(connectionString);

            //InsertUniversities();
            //InsertStudents();
            //InsertLectures();
            //InsertStudentLectureAssociations();
            //GetUniversityOfToni();
            //GetLectureOfToni();
            //GetAllStudentsFromYale();
            //GetAllUniversitiesWithTransgenders();
            //GetAllLecturesAtMizzou();
            //UpdateToni();
            DeleteJamie();

        }

        // insert universities into database
        public void InsertUniversities()
        {
            // delete all entries
            dataContext.ExecuteCommand("delete from University");

            University yale = new University();
            University mizzou = new University();
            yale.Name = "Yale";
            mizzou.Name = "Mizzou";
            // prepare data to be submitted to database
            dataContext.Universities.InsertOnSubmit(yale);
            dataContext.Universities.InsertOnSubmit(mizzou);

            // submit
            dataContext.SubmitChanges();

            // display data in the MainDataGrid
            MainDataGrid.ItemsSource = dataContext.Universities;
        }

        public void InsertStudents()
        {
            // add university objects to University table
            University yale = dataContext.Universities.First(un => un.Name.Equals("Yale"));
            University mizzou = dataContext.Universities.First(un => un.Name.Equals("Mizzou"));

            // create a list of students
            List<Student> students = new List<Student>();

            // add student info to the list of students
            students.Add(new Student { Name = "Carla", Gender = "female", UniversityId = yale.Id });
            students.Add(new Student { Name = "Toni", Gender = "male", University = yale });
            students.Add(new Student { Name = "Leyle", Gender = "female", University = mizzou });
            students.Add(new Student { Name = "Jamie", Gender = "trans-gender", University = mizzou });

            // submit all student entries
            dataContext.Students.InsertAllOnSubmit(students);
            dataContext.SubmitChanges();

            // add data to the grid
            MainDataGrid.ItemsSource = dataContext.Students;
        }

        public void InsertLectures()
        {
            // insert lecture objects into the Lecture table
            dataContext.Lectures.InsertOnSubmit(new Lecture { Name = "Math" });
            dataContext.Lectures.InsertOnSubmit(new Lecture { Name = "Science" });

            // submit change
            dataContext.SubmitChanges();

            // add data to the grid
            MainDataGrid.ItemsSource = dataContext.Lectures;
        }

        public void InsertStudentLectureAssociations()
        {
            // create student and lecture objects
            Student Carla = dataContext.Students.First(st => st.Name.Equals("Carla"));
            Student Toni = dataContext.Students.First(st => st.Name.Equals("Toni"));
            Student Leyle = dataContext.Students.First(st => st.Name.Equals("Leyle"));
            Student Jamie = dataContext.Students.First(st => st.Name.Equals("Jamie"));

            Lecture Math = dataContext.Lectures.First(le => le.Name.Equals("Math"));
            Lecture Science = dataContext.Lectures.First(le => le.Name.Equals("Science"));

            // insert objects into the database
            dataContext.StudentLectures.InsertOnSubmit(new StudentLecture { Student = Carla, Lecture = Math });
            dataContext.StudentLectures.InsertOnSubmit(new StudentLecture { Student = Toni, Lecture = Math });
            dataContext.StudentLectures.InsertOnSubmit(new StudentLecture { Student = Leyle, Lecture = Science });
            dataContext.StudentLectures.InsertOnSubmit(new StudentLecture { Student = Jamie, Lecture = Science });

            // submit changes
            dataContext.SubmitChanges();

            // add data to the data grid
            MainDataGrid.ItemsSource = dataContext.StudentLectures;
        }

        public void GetUniversityOfToni()
        {
            // get a student object of "Toni" from the Students table
            Student Toni = dataContext.Students.First(st => st.Name.Equals("Toni"));

            // create a University object with value of Toni's University
            University TonisUniversity = Toni.University;

            // make a list of universities and add Toni's University
            List<University> universities = new List<University>();
            universities.Add(TonisUniversity);

            // add data to grid
            MainDataGrid.ItemsSource = universities;
        }

        public void GetLectureOfToni()
        {
            // get a student object of "Toni" from the Students table
            Student Toni = dataContext.Students.First(st => st.Name.Equals("Toni"));

            // get Tonis lectures from StudentLectures table
            var tonisLectures = from sl in Toni.StudentLectures select sl.Lecture;

            // add to grid
            MainDataGrid.ItemsSource = tonisLectures;
        }

        public void GetAllStudentsFromYale()
        {
            // get the university name from students, select the students from that university
            var studentsFromYale = from student in dataContext.Students
                                   where student.University.Name == "Yale"
                                   select student;

            // add data to grid
            MainDataGrid.ItemsSource = studentsFromYale;
        }

        public void GetAllUniversitiesWithTransgenders()
        {
            // compare Students and Universities tables and make sure that they contain the same universities
            // students that match the criteria will check is gender is equal to "trans-gender"
            // select those students
            var transgenderUniversities = from student in dataContext.Students
                                          join university in dataContext.Universities
                                          on student.University equals university
                                          where student.Gender == "trans-gender"
                                          select university;

            // add to grid
            MainDataGrid.ItemsSource = transgenderUniversities;
        }

        public void GetAllLecturesAtMizzou()
        {
            // compare Student Lectures and Students tables
            // find equal values from StudentLectures StudentId and Id from Students
            // find instance where students University is equal to "Mizzou"
            // select the lectures from those instances
            var mizzouLectures = from sl in dataContext.StudentLectures
                                 join student in dataContext.Students on sl.StudentId equals student.Id
                                 where student.University.Name == "Mizzou"
                                 select sl.Lecture;

            // add data to grid
            MainDataGrid.ItemsSource = mizzouLectures;
        }

        public void UpdateToni()
        {
            
            Student Toni = dataContext.Students.First(st => st.Name.Equals("Toni"));

            // change Toni's name
            Toni.Name = "Antonio";

            // submit changes
            dataContext.SubmitChanges();

            // update all students information
            MainDataGrid.ItemsSource = dataContext.Students;
        }

        public void DeleteJamie()
        {
            Student Jamie = dataContext.Students.First(st => st.Name.Equals("Jamie"));

            // delete the entry "Jamie"
            dataContext.Students.DeleteOnSubmit(Jamie);

            dataContext.SubmitChanges();

            MainDataGrid.ItemsSource = dataContext.Students;
        }
    }
}
