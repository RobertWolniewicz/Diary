using Diary.Models.Domains;
using Diary.Models.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Diary.Models.Converters;
using Diary.Models;

namespace Diary
{
    public class Repository
    {
        public List<Group> GetGroups()
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Groups.ToList();
            }
        }

        public List<StudentWrapper> GetStudents(int groupId)
        {
            using (var context = new ApplicationDbContext())
            {
                var students = context
                    .Students
                    .Include(s => s.Group)
                    .Include(s => s.Ratings)
                    .AsQueryable();

                if (groupId != 0)
                    students = students.Where(s => s.GroupId == groupId);

                return students.ToList().Select(s => s.ToWrapper()).ToList();
            }
        }

        public void DeleteStudent(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var studentToDelete = context.Students.Find(id);
                context.Students.Remove(studentToDelete);
                context.SaveChanges();
            }
        }

        public void AddStudent(StudentWrapper studentWrapper)
        {
            var student = studentWrapper.ToDao();
            var ratings = studentWrapper.ToRatingDao();

            using (var context = new ApplicationDbContext())
            {
                var dbStudent = context.Students.Add(student);

                ratings.ForEach(r =>
                {
                    r.StudentId = dbStudent.Id;
                    context.Ratings.Add(r);
                });
                context.SaveChanges();
            }
        }

        public void UpdateStudent(StudentWrapper studentWrapper)
        {
            var student = studentWrapper.ToDao();
            var ratings = studentWrapper.ToRatingDao();

            using (var context = new ApplicationDbContext())
            {

                UpdateStudentProperties(context, student);

                var studentRatings = GetStudentRatings(student, context);

                UpdateRate(student, ratings, context, studentRatings,
                    Subject.Math);
                UpdateRate(student, ratings, context, studentRatings,
                    Subject.Technology);
                UpdateRate(student, ratings, context, studentRatings,
                    Subject.Physics);
                UpdateRate(student, ratings, context, studentRatings,
                    Subject.PolishLang);
                UpdateRate(student, ratings, context, studentRatings,
                    Subject.ForeignLang);

                context.SaveChanges();
            }
        }

        static List<Rating> GetStudentRatings(Student student, ApplicationDbContext context)
        {
            return context
                   .Ratings
                   .Where(r => r.Student.Id == student.Id)
                   .ToList();
        }

        void UpdateStudentProperties(ApplicationDbContext context, Student student)
        {
            var studentToUpdate = context.Students.Find(student.Id);
            studentToUpdate.Activities = student.Activities;
            studentToUpdate.Comments = student.Comments;
            studentToUpdate.FirstName = student.FirstName;
            studentToUpdate.LastName = student.LastName;
            studentToUpdate.GroupId = student.GroupId;
        }

        static void UpdateRate(Student student, List<Rating> newRatings,
            ApplicationDbContext context, List<Rating> studentRatings, Subject subject)
        {
            var subRatings = studentRatings
                    .Where(sr => sr.SubjectId == (int)subject)
                    .Select(r => r.Rate);

            var newSubRatings = newRatings
                .Where(r => r.SubjectId == (int)subject)
                .Select(r => r.Rate);

            var subRatingsToDelete = GetSubRatingsToDelete(subRatings, newSubRatings);
            var subRatingsToAdd = GetSubRatingsToAdd(subRatings, newSubRatings);

            subRatingsToDelete.ForEach(srtd =>
                     {
                         var ratingToDelete = context.Ratings.First(r =>
                         r.Rate == srtd &&
                         r.StudentId == student.Id &&
                         r.SubjectId == (int)subject);

                         context.Ratings.Remove(ratingToDelete);

                     });

            subRatingsToAdd.ForEach(srta =>
            {
                var ratingToAdd = new Rating
                {
                    Rate = srta,
                    StudentId = student.Id,
                    SubjectId = (int)subject
                };
                context.Ratings.Add(ratingToAdd);
            });
        }
        private static List<int> GetSubRatingsToAdd(
            IEnumerable<int> oldSubRatings, IEnumerable<int> newSubRatings)
        {
            var subRatingsToAdd = new List<int>();
            var oldListCopy = new List<int>(oldSubRatings);
            foreach (var item in newSubRatings)
            {
                var itemInOldList = oldListCopy.FirstOrDefault(x => x == item);
                if (itemInOldList != 0)
                    oldListCopy.Remove(itemInOldList);
                else
                    subRatingsToAdd.Add(item);
            }
            return subRatingsToAdd;
        }

        private static List<int> GetSubRatingsToDelete(
            IEnumerable<int> oldSubRatings, IEnumerable<int> newSubRatings)
        {
            var subRatingsToDelete = new List<int>();
            var newListCopy = new List<int>(newSubRatings);
            foreach (var item in oldSubRatings)
            {
                var itemInNewList = newListCopy.FirstOrDefault(x => x == item);
                if (itemInNewList != 0)
                    newListCopy.Remove(itemInNewList);
                else
                    subRatingsToDelete.Add(item);
            }
            return subRatingsToDelete;
        }
    }
}
