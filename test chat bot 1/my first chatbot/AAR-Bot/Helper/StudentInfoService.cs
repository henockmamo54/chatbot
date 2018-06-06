using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AAR_Bot.Helper
{
    public class StudentInfoService
    {
        public List<StudentInfo> studentInfoList;                  //학생정보 리스트 생성

        public StudentInfoService()
        {
            this.studentInfoList = readTheStudentInfoFile();
        }

        public List<StudentInfo> readTheStudentInfoFile()
        {
            var mylist = new List<StudentInfo>();
            string path = Path.Combine(System.Web.HttpRuntime.AppDomainAppPath, @"data\");          //path 설정

            DirectoryInfo di = new DirectoryInfo(path);                                             //Directory 설정

            foreach (FileInfo fi in di.GetFiles())                                                  //해당 디렉토리의 파일 읽기
            {
                string mytestpath = path + fi.Name;
                var rows = File.ReadAllLines(mytestpath).Select(a => a.Split(','));
                var myrows = rows.ToList();

                var sync = new object();
                myrows.RemoveAt(0);
                myrows.RemoveAt(0);
                Parallel.ForEach(myrows, row =>
                {
                    StudentInfo info = new StudentInfo();
                    info.FullName = row[1].ToString();
                    info.StudentNumber = row[2];
                    info.YearOfRegistration = row[3];
                    info.YearCourseTaken = row[4];
                    info.SemesterCourseTake = row[5];
                    info.CourseType = row[6];
                    info.CourseName = row[7];
                    info.KoreanCourseNumber = row[8];
                    info.CourseNumber = row[9];
                    info.Credit = double.Parse(row[10]);
                    info.IsStuding = row[11];
                    lock (sync)
                    {
                        mylist.Add(info);// here you can add or remove items from the fileInfo list
                    }


                });

            }
            this.studentInfoList = mylist;
            return mylist;
        }

        public double totalCredits(int studentID)                   //총 이수학점
        {
            var studentinfo = from b in studentInfoList
                              where b.StudentNumber == studentID.ToString()
                              select b;
            int count = studentinfo.Count();
            double mycredit = studentinfo.Sum(item => item.Credit);
            return mycredit;
        }

        public double totalMajorCredits(int studentID)              //총 이수 전공학점
        {
            var studentinfo = from b in studentInfoList
                              where b.StudentNumber == studentID.ToString() && b.CourseType.Contains("전공")
                              select b;
            int count = studentinfo.Count();
            double mycredit = studentinfo.Sum(item => item.Credit);
            return mycredit;
        }

        public double totalElectiveCredits(int studentID)           //총 이수 교양학점
        {
            var studentinfo = from b in studentInfoList
                              where b.StudentNumber == studentID.ToString() && b.CourseType.Contains("교양")
                              select b;
            int count = studentinfo.Count();
            double mycredit = studentinfo.Sum(item => item.Credit);
            return mycredit;
        }
    }
}