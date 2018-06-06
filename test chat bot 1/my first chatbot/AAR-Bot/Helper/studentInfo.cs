namespace AAR_Bot.Helper
{
    public class StudentInfo
    {
            private string _fullName { get; set; }                  //이름
            private string _studentNumber { get; set; }             //학번
            private string _yearOfRegistration { get; set; }        //입학년도
            private string _yearCourseTaken { get; set; }           //해당강의 수강년도
            private string _semesterCourseTake { get; set; }        //해당강의 수강학기
            private string _courseType { get; set; }                //강의 구분
            private string _courseName { get; set; }                //강의명
            private string _koreanCourseNumber { get; set; }        //한국어 강의코드
            private string _courseNumber { get; set; }              //강의코드
            private double _credit { get; set; }                    //강의학점
            private string _isStuding { get; set; }                 //재학 휴학 여부

            public StudentInfo() { }


            public string FullName { get { return _fullName; } set { _fullName = value; } }
            public string StudentNumber { get { return _studentNumber; } set { _studentNumber = value; } }
            public string YearOfRegistration { get { return _yearOfRegistration; } set { _yearOfRegistration = value; } }
            public string YearCourseTaken { get { return _yearCourseTaken; } set { _yearCourseTaken = value; } }
            public string SemesterCourseTake { get { return _semesterCourseTake; } set { _semesterCourseTake = value; } }
            public string CourseType { get { return _courseType; } set { _courseType = value; } }
            public string CourseName { get { return _courseName; } set { _courseName = value; } }
            public string KoreanCourseNumber { get { return _koreanCourseNumber; } set { _koreanCourseNumber = value; } }
            public string CourseNumber { get { return _courseNumber; } set { _courseNumber = value; } }
            public double Credit { get { return _credit; } set { _credit = value; } }
            public string IsStuding { get { return _isStuding; } set { _isStuding = value; } }
        }
    
}
