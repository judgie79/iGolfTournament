using Golf.Tournament.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Golf.Tournament.ViewModels
{
    public class CourseViewModelModelBinder<TCourseViewModel> : IModelBinder
        where TCourseViewModel : CourseViewModel, new()
    {
        public object BindModel(ControllerContext controllerContext,
                                ModelBindingContext bindingContext)
        {
            HttpRequestBase request = controllerContext.HttpContext.Request;
            
            var form = request.Form;

            string clubId = form.Get("Club.Id") ?? form.Get("Course.ClubId");

            var Course = new Course()
            {
                ClubId = clubId,
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
                        Holes = new CourseHoles()
                        {
                            Front = new CourseHoleCollection(),
                            Back = new CourseHoleCollection()
                        }
                    };

                    while (true)
                    {
                        if (!string.IsNullOrEmpty(form["Course.TeeBoxes[" + teeBoxCounter + "].Holes.Front[" + holeCounter + "].Number"]))
                        {
                            string holeId = form["Course.TeeBoxes[" + teeBoxCounter + "].Holes.Front[" + holeCounter + "].HoleId"];
                            string holePar = form["Course.TeeBoxes[" + teeBoxCounter + "].Holes.Front[" + holeCounter + "].Par"];
                            string holeDistance = form["Course.TeeBoxes[" + teeBoxCounter + "].Holes.Front[" + holeCounter + "].Distance"];
                            string holeNumber = form["Course.TeeBoxes[" + teeBoxCounter + "].Holes.Front[" + holeCounter + "].Number"];
                            string holeHcp = form["Course.TeeBoxes[" + teeBoxCounter + "].Holes.Front[" + holeCounter + "].Hcp"];

                            var Hole = new CourseHole()
                            {
                                Par = Convert.ToInt32(holePar),
                                Distance = Convert.ToInt32(holeDistance),
                                Hcp = Convert.ToInt32(holeHcp),
                                Id = holeId,
                                Number = Convert.ToInt32(holeNumber)
                            };

                            TeeBox.Holes.Front.Add(Hole);
                            holeCounter++;
                        } else if (!string.IsNullOrEmpty(form["Course.TeeBoxes[" + teeBoxCounter + "].Holes.Back[" + holeCounter + "].Number"]))
                        {
                            string holeId = form["Course.TeeBoxes[" + teeBoxCounter + "].Holes.Back[" + holeCounter + "].HoleId"];
                            string holePar = form["Course.TeeBoxes[" + teeBoxCounter + "].Holes.Back[" + holeCounter + "].Par"];
                            string holeDistance = form["Course.TeeBoxes[" + teeBoxCounter + "].Holes.Back[" + holeCounter + "].Distance"];
                            string holeNumber = form["Course.TeeBoxes[" + teeBoxCounter + "].Holes.Back[" + holeCounter + "].Number"];
                            string holeHcp = form["Course.TeeBoxes[" + teeBoxCounter + "].Holes.Back[" + holeCounter + "].Hcp"];

                            var Hole = new CourseHole()
                            {
                                Par = Convert.ToInt32(holePar),
                                Distance = Convert.ToInt32(holeDistance),
                                Hcp = Convert.ToInt32(holeHcp),
                                Id = holeId,
                                Number = Convert.ToInt32(holeNumber)
                            };

                            TeeBox.Holes.Back.Add(Hole);
                            holeCounter++;
                        }
                        else
                        {
                            TeeBox.Holes.Front = TeeBox.Holes.Front.OrderBy(h => h.Number).ToCourseHoleCollection();
                            TeeBox.Holes.Back = TeeBox.Holes.Back.OrderBy(h => h.Number).ToCourseHoleCollection();
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

            return new TCourseViewModel()
            {
                Course = Course
            };
        }
    }
}