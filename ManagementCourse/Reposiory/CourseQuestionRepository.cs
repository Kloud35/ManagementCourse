using ManagementCourse.Common;
using ManagementCourse.Models;
using ManagementCourse.Models.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ManagementCourse.Reposiory
{
    public class CourseQuestionRepository:GenericRepository<CourseQuestion>
    {

        public List<ExamQuestionDTO> ListExamQuestion(int courseId, int courseExamResultID)
        {
            List<ExamQuestionDTO> listExamQuestions = new List<ExamQuestionDTO>();

            DataSet dataSet = LoadDataFromSP.GetDataSetSP("spCourseQuestion",
                                                            new string[] { "@CourseID" , "@CourseExamResultID" },
                                                            new object[] { courseId ,courseExamResultID });

            DataTable dtQuestion = dataSet.Tables[0];
            DataTable dtAnswer = dataSet.Tables[1];

            for (int i = 0; i < dtQuestion.Rows.Count; i++)
            {
                ExamQuestionDTO examQuestion = new ExamQuestionDTO();

                examQuestion.ID = TextUtils.ToInt(dtQuestion.Rows[i]["ID"]);
                examQuestion.QuestionText = TextUtils.ToString(dtQuestion.Rows[i]["QuestionText"]);
                examQuestion.QuestionChosenID = TextUtils.ToInt(dtQuestion.Rows[i]["QuestionChosenID"]);
                examQuestion.ExamAnswers = ListExamAnswer(dtAnswer, examQuestion.ID);

                listExamQuestions.Add(examQuestion);
            }

            return listExamQuestions;
        }


        private List<ExamAnswerDTO> ListExamAnswer(DataTable dt, int questionId)
        {
            List<ExamAnswerDTO> listAnswers = TextUtils.ConvertDataTable<ExamAnswerDTO>(dt);
            List<ExamAnswerDTO> list = listAnswers.Where(x => x.CourseQuestionId == questionId).ToList();
            return list;
        }
    }
}
