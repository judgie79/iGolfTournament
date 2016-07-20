using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Golf.Tournament.Models
{
    public class CourseEditViewModel
    {
        public Course Course { get; set; }

        public IEnumerable<Club> Clubs { get; set; }
    }

    public class CourseEditViewModelModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext,
                                ModelBindingContext bindingContext)
        {
            HttpRequestBase request = controllerContext.HttpContext.Request;
            
            var form = request.Form;

            var Course = new Course()
            {
                ClubId = form.Get("Course.ClubId"),
                Id = form.Get("Course.Id"),
                Name = form.Get("Course.Name"),
                TeeBoxes = new TeeboxCollection()
            };

            
            int teeBoxCounter = 0; //"Course.TeeBoxes[0].Holes[0].HoleId"
            int holeCounter = 0;

            while (true)
            {
                if (!string.IsNullOrEmpty(form["Course.TeeBoxes[" + teeBoxCounter + "].Name"]))
                {
                    string Name = form["Course.TeeBoxes[" + teeBoxCounter + "].Name"];
                    string Color = form["Course.TeeBoxes[" + teeBoxCounter + "].Color.Value"];
                    string CourseRating = form["Course.TeeBoxes[" + teeBoxCounter + "].CourseRating"];
                    string Distance = form["Course.TeeBoxes[" + teeBoxCounter + "].Distance"];
                    string Par = form["Course.TeeBoxes[" + teeBoxCounter + "].Par"];
                    string SlopeRating = form["Course.TeeBoxes[" + teeBoxCounter + "].SlopeRating"];

                    var TeeBox = new TeeBox()
                    {
                        Color = new Color(Color),
                        CourseRating = float.Parse(CourseRating),
                        Distance = Convert.ToInt32(Distance),
                        Name = Name,
                        Par = Convert.ToInt32(Par),
                        SlopeRating = float.Parse(SlopeRating),
                        Holes = new HoleCollection()
                    };

                    while (true)
                    {
                        if (!string.IsNullOrEmpty(form["Course.TeeBoxes[" + teeBoxCounter + "].Holes[" + holeCounter + "].Number"]))
                        {
                            string holeId = form["Course.TeeBoxes[" + teeBoxCounter + "].Holes[" + holeCounter + "].HoleId"];
                            string holePar = form["Course.TeeBoxes[" + teeBoxCounter + "].Holes[" + holeCounter + "].Par"];
                            string holeDistance = form["Course.TeeBoxes[" + teeBoxCounter + "].Holes[" + holeCounter + "].Distance"];
                            string holeNumber = form["Course.TeeBoxes[" + teeBoxCounter + "].Holes[" + holeCounter + "].Number"];
                            string holeHcp = form["Course.TeeBoxes[" + teeBoxCounter + "].Holes[" + holeCounter + "].Hcp"];

                            var Hole = new Hole()
                            {
                                Par = Convert.ToInt32(holePar),
                                Distance = Convert.ToInt32(holeDistance),
                                Hcp = Convert.ToInt32(holeHcp),
                                HoleId = holeId,
                                Number = Convert.ToInt32(holeNumber)
                            };

                            TeeBox.Holes.Add(Hole);
                            holeCounter++;
                        } else
                        {
                            TeeBox.Holes = TeeBox.Holes.OrderBy(h => h.Number).ToHoleCollection();
                            holeCounter = 0;
                            break;
                        }
                    }

                    Course.TeeBoxes.Add(TeeBox);
                    teeBoxCounter++;
                }
                else
                {
                    teeBoxCounter = 0;
                    break;
                }

                
            }

            return new CourseEditViewModel()
            {
                Course = Course
            };
        }
    }

    public class CourseCreateViewModel
    {
        public Course Course { get; set; }

        public IEnumerable<Club> Clubs { get; set; }
    }
}