using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AAR_Bot.Helper.StoredStringValues
{
    [Serializable]
    public class StoredStringValuesMaster
    {
        public string _printLine = "\n----------\n";
        // 기본 메뉴    welcome options
        public string _courseRegistration = "수강 신청";
        public string _courseInformation = "과목 정보";
        public string _credits = "학점 관리";
        public string _others = "기타 정보";
        public string _typeself = "직접 입력하기";
        public List<string> _welcomeOptionsList = new List<string>();

        // 수강 신청 선택시 메뉴     course registration options
        public string _howToDoIt = "수강신청 방법";             //웹 연결
        public string _schedule = "수강신청 일정";              //웹 연결
        public string _regulation = "수강신청 규정";            //웹 연결
        public string _terms = "수강신청 용어";                 //우리가 정의? 혹은 웹 연결?
        public List<string> _courseRegistrationOptions = new List<string>();

        // 과목 정보 선택시 메뉴     course info options
        public string _openedMajorCourses = "이번학기 전공개설과목";
        public string _openedLiberalArts = "이번학기 교양개설과목";   //myiweb에 있는데 우짜지..
        public string _syllabus = "강의계획서";                //이것도..
        public string _lecturerInfo = "강사 정보";             //이것도..
        public string _mandatorySubject = "필수과목 정보";      //이건 어떻게 가능
        public string _prerequisite = "선수과목 정보";          //이것도 탐색
        public List<string> _courseInfoOptions = new List<string>();
        //_liberalArtsCredits
        // 학점 관리 선택시 메뉴     credit options
        public string _currentCredits = "나의 이수학점";        //개인별 정보 필요
        public string _majorCredits = "전공 학점";
        public string _liberalArtsCredits = "교양 학점";
        public string _changeStuNum = "학번 재설정";
        public List<string> _creditsOptions = new List<string>();

        // 기타 정보 선택시 메뉴     others options
        public string _leaveOrReadmission = "휴학 및 복학";         //웹 연결
        public string _scholarship = "장학금 관련";            //웹 연결
        public string _restaurantMenu = "Restaurant Menu";
        public string _libraryInfo = "Library Info";
        public List<string> _othersOption = new List<string>();

        // 직접 입력 메뉴     typeself options
        public string _typePleaseWelcome = $"▶ 안녕하세요 AAR 챗봇서비스\n      입니다.\n" +
                                    $"▶ 문의 내용의 번호를 선택하시\n      거나 질문을 입력해주세요.\n\n" +
                                    $"▶ 1. 수강신청정보\n" +
                                    $"▶ 2. 과목관련정보\n" +
                                    $"▶ 3. 학점관리\n" +
                                    $"▶ 4. 기타정보\n" +
                                    $"▶ 5. 도움말\n" +

                                    $" ※ 명지대학교 홈페이지\n" +
                                    $" ■ https://www.mju.ac.kr \n" +
                                    $" ※ Github for AAR\n" +
                                    $" ■ https://github.com/MJUKJE/chatbot/blob/dev/README.md \n" +

                                    $"▶ 학점 관리항목은\n      학번입력이 필요합니다.\n" +
                                    $"▶ [처음으로]나 초기메뉴를 입력시\n      처음으로 돌아가실 수 있습니다.\n" +
                                    $"▶ Go to the [도움말] -> [English]\n      Language conversion is\n      possible :).\n" +
                                    $"▶ 버튼메뉴는 빠르고 편리합니다.\n" +
                                    $"▶ 추후 계속 업데이트 예정.\n";



        public string _typePleaseCourseRegistration = $"▶ 수강신청 메뉴 입니다.\n" +
                                    $"▶ 문의 내용의 번호를 선택하시\n      거나 질문을 입력해주세요.\n\n" +
                                    $"▶ 1. 수강신청 방법\n" +
                                    $"▶ 2. 수강신청 일정\n" +
                                    $"▶ 3. 수강신청 규정\n" +
                                    $"▶ 4. 수강신청 용어\n" +
                                    $"▶ 5. 처음으로\n" +
                                    $"▶ 6. 도움말\n\n" +

                                    $"▶ Go to the [도움말] -> [English]\n      Language conversion is\n      possible :).\n" +
                                    $"▶ 버튼메뉴는 빠르고 편리합니다.\n" +
                                    $"▶ 추후 계속 업데이트 예정.\n";


        public string _typePleaseCourseInfo = $"▶ 강의정보 메뉴 입니다.\n" +
                                    $"▶ 문의 내용의 번호를 선택하시\n      거나 질문을 입력해주세요.\n\n" +
                                    $"▶ 1. 이번학기 전공개설과목\n" +
                                    $"▶ 2. 이번학기 교양개설과목\n" +
                                    $"▶ 3. 강의계획서\n" +
                                    $"▶ 4. 강사 정보\n" +
                                    $"▶ 5. 필수과목 정보\n" +
                                    $"▶ 6. 선수과목 정보\n" +
                                    $"▶ 7. 처음으로\n" +
                                    $"▶ 8. 도움말\n\n" +

                                    $"▶ Go to the [도움말] -> [English]\n      Language conversion is\n      possible :).\n" +
                                    $"▶ 버튼메뉴는 빠르고 편리합니다.\n" +
                                    $"▶ 추후 계속 업데이트 예정.\n";



        public string _sorryMessage = $"▶말씀을 이해하지 못했습니다.\n" +
                                        $"▶문의하신 내용에 대해 다음에는\n" +
                                        $"▶안내드릴 수 있도록 열심히\n" +
                                        $"▶학습하겠습니다.\n\n" +
                                        $"※버튼메뉴를 이용하시면\n" +
                                        $"※빠르고 편리합니다 :)\n" +
                                        $"■ 각종 문의 및 상담\n";

        // 도움말 선택시 메뉴       help options
        public string _introduction = "AAR안내";
        public string _requestInformationCorrection = "정보수정요청";
        public string _contactMaster = "관리자 연결";
        public string _convertLanguage = "한국어";
        public List<string> _helpOptionsList = new List<string>();

        // 처음으로 혹은 도움말      goto start and help
        public string _gotostart = "처음으로";
        public string _help = "도움말";

        public string _goodByeMessage = "Thank you for Using AAR Service.\n The chat bot is Under development. \nPlease let us know your feedback.";



        //================================================================================================================================
        //For Fake LUIS

        //WelcomeOption
        public List<string> _courseRegistrationVoca = new List<string>();
        public List<string> _courseInfoVoca = new List<string>();
        public List<string> _creditVoca = new List<string>();
        public List<string> _othersVoca = new List<string>();
        public List<string> _helpVoca = new List<string>();
        public List<string> _gotoStartVoca = new List<string>();
        public List<string> _gotoButtonVoca = new List<string>();
        public List<string> _languageVoca = new List<string>();


        //CourseRegistration
        public List<string> _howToDoItVoca = new List<string>();
        public List<string> _scheduleVoca = new List<string>();
        public List<string> _regulationVoca = new List<string>();
        public List<string> _termsVoca = new List<string>();


        //CourseInfo
        public List<string> _openedMajorCoursesVoca = new List<string>();
        public List<string> _openedLiberalArtsVoca = new List<string>();
        public List<string> _syllabusVoca = new List<string>();
        public List<string> _lecturerInfoVoca = new List<string>();
        public List<string> _mandatorySubjectVoca = new List<string>();
        public List<string> _prerequisiteVoca = new List<string>();



        //list of list
        public List<List<string>> _welcomeOptionVocaList = new List<List<string>>();
        public List<List<string>> _courseRegistrationVocaList = new List<List<string>>();
        public List<List<string>> _courseInfoVocaList = new List<List<string>>();

        //================================================================================================================================


        public StoredStringValuesMaster()
        {
            _welcomeOptionsList = new List<string> { _courseRegistration, _courseInformation, _credits, _others, _typeself, _help };
            _courseRegistrationOptions = new List<string> { _howToDoIt, _schedule, _regulation, _terms, _gotostart, _help };
            _courseInfoOptions = new List<string> { _openedMajorCourses, _openedLiberalArts, _syllabus, _lecturerInfo, _mandatorySubject, _prerequisite, _gotostart, _help };
            _creditsOptions = new List<string> { _currentCredits, _majorCredits, _liberalArtsCredits, _changeStuNum, _gotostart, _help };
            _othersOption = new List<string> { _leaveOrReadmission, _scholarship, _restaurantMenu, _libraryInfo, _gotostart, _help };
            _helpOptionsList = new List<string> { _introduction, _requestInformationCorrection, _contactMaster, _convertLanguage, _gotostart };



            //================================================================================================================================
            //For Fake LUIS


            //WelcomeOption
            _courseRegistrationVoca = new List<string> { "수강신청", "수강 신청" };
            _courseInfoVoca = new List<string> { "과목정보", "과목 정보", "강의정보", "강의 정보", "과목관련", "강의관련", "전공과목", "교양과목", "전공개설", "교양개설", "강의계획서", "강사 정보", "교수 정보", "필수과목", "선수과목", "개설전공", "개설교양" };
            _creditVoca = new List<string> { "학점", "나의학점", "내학점" };
            _othersVoca = new List<string> { "기타", "그외" };
            _helpVoca = new List<string> { "도움", "help", "사용법", "쓰는법" };
            _gotoStartVoca = new List<string> { "처음으로", "초기", "처음", "시작" };
            _gotoButtonVoca = new List<string> { "버튼", "Button", "button", "기본메뉴" };
            _languageVoca = new List<string> { "한국어", "영어", "English", "Korean", "english", "korean" };


            //CourseRegistration
            _howToDoItVoca = new List<string> { "방법", "하는법", "하는 법" };
            _scheduleVoca = new List<string> { "일정", "기간", "날짜", "일자" };
            _regulationVoca = new List<string> { "규정", "규칙", "규율" };
            _termsVoca = new List<string> { "용어", "단어" };


            //CourseInfo
            _openedMajorCoursesVoca = new List<string> { "전공" };
            _openedLiberalArtsVoca = new List<string> { "교양" };
            _syllabusVoca = new List<string> { "계획서" };
            _lecturerInfoVoca = new List<string> { "교수", "강사" };
            _mandatorySubjectVoca = new List<string> { "필수" };
            _prerequisiteVoca = new List<string> { "선수" };









            //list of list
            _welcomeOptionVocaList = new List<List<string>> { _courseRegistrationVoca, _courseInfoVoca, _creditVoca, _othersVoca, _helpVoca, _gotoStartVoca, _gotoButtonVoca, _languageVoca };
            _courseRegistrationVocaList = new List<List<string>> { _howToDoItVoca, _scheduleVoca, _regulationVoca, _termsVoca, _helpVoca, _gotoStartVoca, _languageVoca };
            _courseInfoVocaList = new List<List<string>> { _openedMajorCoursesVoca, _openedLiberalArtsVoca, _syllabusVoca, _lecturerInfoVoca, _mandatorySubjectVoca, _prerequisiteVoca, _helpVoca, _gotoStartVoca, _languageVoca };
            //================================================================================================================================

        }

        //모든 정보를 언어에 따라 다르게 주기 위해서
        //for diffrent reply from language select

        //RootDialog + General
        public string _getStudentNumMessage = $"▶안녕하세요 명지대학교 AAR3입니다.\n" +
                                $"▶맞춤형 정보제공을 위해 학번을 입력해 주세요.\n" +
                            $"▶입력하신 학번은 저장되지 않습니다.\n" +
                            $"▶테스트용 학번 : 60131937.\n";
        public string _getStudentNumUpdateMessage = $"▶학번 정보가 변경되었습니다.\n" +
                            $"▶변경된 학번 : ";
        public string _getStudentNumFail = $"▶잘못된 형식입니다.\n" +
                                    $"▶학번을 다시 입력해 주세요.(e.g. '60131937')";
        public string _welcomeMessage = $"▶안녕하세요 명지대학교 AAR3입니다.\n" +
                                $"▶궁금하신 정보를 선택해 주세요.\n" +
                            $"▶직접 입력하기를 선택하시면 텍스트 입력이 가능합니다.\n" +
                            $"▶학점 관리항목은 학번입력이 필요합니다.\n" +
                            $"※Go to the [도움말] -> [English]\n" +
                            $"※Language conversion is possible :).\n";
        public string _invalidSelectionMessage = "※잘못된 옵션을 선택하셨어요ㅠㅠ 다시해주세요.";
        public string _goToButton = "정보로 이동";

        //aboutCourseInfo
        public string _courseInfoSelected = "▶강의 정보를 선택하셨습니다.\n세부항목을 선택해주세요.";


        public string _reply_OpenedMajorCourses = $"▶이번학기 개설전공강의에 대한 안내입니다.\n";


        public string _reply_openedLiberalArts = $"▶이번학기 개설강의에 대한 안내입니다.\n";


        public string _reply_Syllabus = $"강의계획서에 대한 안내입니다.\n" +
                            $"Myiweb내에 존재하는 정보이므로 로그인이 필요합니다.\n" +
                            $"모바일 페이지에 존재하지 않는 정보라 데스크탑 사이트로 연결됩니다.\n";


        public string _reply_LecturerInfo = $"강사 정보에 대한 안내입니다.\n" +
                            $"Eclass내에 존재하는 정보이므로 로그인이 필요합니다.\n" +
                            $"모바일 페이지에 존재하지 않는 정보라 데스크탑 사이트로 연결됩니다.\n";


        public string _reply_MandatorySubject = $"필수과목 정보에 대한 안내입니다.\n" +
                            $"추후 추가예정 입니다.\n";


        public string _reply_Prerequisite = $"선수과목 정보에 대한 안내입니다.\n" +
                            $"추후 추가예정 입니다.\n";


        //aboutCourseRegistration
        public string _courseRegistrationSelected = "수강 신청을 선택하셨습니다.\n세부항목을 선택해주세요.";


        public string _reply_HowToDoIt = $"수강신청 방법에 대한 안내입니다.\n" +
                            $"모바일 페이지에 존재하지 않는 정보라 데스크탑 사이트로 연결됩니다.\n";


        public string _reply_Schedule = $"수강신청 일정에 대한 안내입니다.\n" +
                            $"모바일 페이지에 존재하지 않는 정보라 데스크탑 사이트로 연결됩니다.\n";


        public string _reply_Regulation = $"수강신청 규정에 대한 안내입니다.\n" +
                            $"해당 페이지 3장 5절 제26조를 확인하십시오.\n" +
                            $"모바일 페이지에 존재하지 않는 정보라 데스크탑 사이트로 연결됩니다.\n";


        public string _reply_Terms = $"수강신청관련 용어정보에 대한 안내입니다.\n";


        //aboutCredits
        public string _creditsOptionSelected = "학점 관리를 선택하셨습니다.\n세부항목을 선택해주세요.";


        public string _reply_CurrentCredits = $"나의 이수학점에 대한 안내입니다.\n" +
                            $"이수하신 총 학점은 : ";


        public string _reply_MajorCredits = $"전공 학점에 대한 안내입니다.\n" +
                            $"이수하신 전공 학점은 : ";


        public string _reply_LiberalArtsCredits = $"교양 학점에 대한 안내입니다.\n" +
                            $"이수하신 교양 학점은 : ";


        public string _reply_ChangeStuNum = $"재설정을 원하시는 학번을 입력해주세요.\n" +
                            $"현재 설정되어 있는 학번은 : ";


        //aboutOthers
        public string _otherOptionSelected = "기타 정보를 선택하셨습니다.\n세부항목을 선택해주세요.";


        public string _reply_leaveOrReadmission = $"휴학 및 복학정보에 대한 안내입니다.\n";


        public string _reply_Scholarship = $"장학금 관련정보에 대한 안내입니다.\n";


        //aboutHelp
        public string _helpOptionSelected = "AAR3 도움말입니다. 무엇을 도와드릴까요?";


        public string _reply_Introduction = $"AAR3에 대한 안내입니다.\n" +
                            $"AAR3는 학생들의 수강신청 및 학점관리를 도울 수 있습니다..\n" +
                            $"궁금하신 정보를 선택하시면 해당 정보페이지로 연결됩니다.\n" +
                            $"선택 도중에 처음으로 돌아가고 싶으시면 처음으로를 눌러주세요.\n" +
                            $"추후 추가예정 입니다.\n";


        public string _reply_RequestInformationCorrection = $"정보수정요청을 하실수 있습니다.\n" +
                            $"추후 추가예정 입니다.\n";

        public string _reply_ContactMaster = $"관리자와 상담을 요청하실 수 있습니다.\n" +
                            $"추후 추가예정 입니다.\n";



        //=======================================================================================================================================

























    }
}