using Diary.Models.Domains;
using Diary.Models.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diary.Models.Converters
{
    public static class StudentConverter
    {
        public static StudentWrapper ToWrapper(this Student model)
        {
            return new StudentWrapper
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Comments = model.Comments,
                Activities = model.Activities,
                Group = new GroupWrapper
                {
                    Id = model.Group.Id,
                    Name = model.Group.Name
                },
                Math = String.Join(", ", model.Ratings
                .Where(r => r.SubjectId == (int)Subject.Math)
                .Select(mr => mr.Rate)),
                Physics = String.Join(", ", model.Ratings
                .Where(r => r.SubjectId == (int)Subject.Physics)
                .Select(pr => pr.Rate)),
                Technology = String.Join(", ", model.Ratings
                .Where(r => r.SubjectId == (int)Subject.Technology)
                .Select(tr => tr.Rate)),
                PolishLang = String.Join(", ", model.Ratings
                .Where(r => r.SubjectId == (int)Subject.PolishLang)
                .Select(plr => plr.Rate)),
                ForeignLang = String.Join(", ", model.Ratings
                .Where(r => r.SubjectId == (int)Subject.ForeignLang)
                .Select(flr => flr.Rate))
            };
        }

        public static Student ToDao (this StudentWrapper model)
        {
            return new Student
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                GroupId = model.Group.Id,
                Comments = model.Comments,
                Activities = model.Activities
            };
        }
        public static List<Rating> ToRatingDao(this StudentWrapper model)
        {
            var ratings = new List<Rating>();

            if (!string.IsNullOrWhiteSpace(model.Math))
                model.Math.Split(',').ToList().ForEach(r =>
                ratings.Add(
                    new Rating
                    {
                        Rate = int.Parse(r),
                        StudentId = model.Id,
                        SubjectId = (int)Subject.Math
                    }));

            if (!string.IsNullOrWhiteSpace(model.Physics))
                model.Physics.Split(',').ToList().ForEach(r =>
                ratings.Add(
                    new Rating
                    {
                        Rate = int.Parse(r),
                        StudentId = model.Id,
                        SubjectId = (int)Subject.Physics
                    }));

            if (!string.IsNullOrWhiteSpace(model.PolishLang))
                model.PolishLang.Split(',').ToList().ForEach(r =>
                ratings.Add(
                    new Rating
                    {
                        Rate = int.Parse(r),
                        StudentId = model.Id,
                        SubjectId = (int)Subject.PolishLang
                    }));

            if (!string.IsNullOrWhiteSpace(model.Technology))
                model.Technology.Split(',').ToList().ForEach(r =>
                ratings.Add(
                    new Rating
                    {
                        Rate = int.Parse(r),
                        StudentId = model.Id,
                        SubjectId = (int)Subject.Technology
                    }));

            if (!string.IsNullOrWhiteSpace(model.ForeignLang))
                model.ForeignLang.Split(',').ToList().ForEach(r =>
                ratings.Add(
                    new Rating
                    {
                        Rate = int.Parse(r),
                        StudentId = model.Id,
                        SubjectId = (int)Subject.ForeignLang
                    }));


            return ratings;
        }
    }
}
