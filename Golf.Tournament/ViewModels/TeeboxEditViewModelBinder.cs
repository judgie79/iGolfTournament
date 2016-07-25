using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Golf.Tournament.Models;
using System.Web.Mvc;

namespace Golf.Tournament.ViewModels
{
    public class TeeboxEditViewModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext,
                                ModelBindingContext bindingContext)
        {
            HttpRequestBase request = controllerContext.HttpContext.Request;

            var form = request.Form;

            string id = form.Get("Teebox.Id");
            string Color = form.Get("Teebox.Color.Value");
            float CourseRating = float.Parse(form.Get("Teebox.CourseRating"));

            int Distance = Convert.ToInt32(form.Get("Teebox.Distance"));
            string Name = form.Get("Teebox.Name");
            int Par = Convert.ToInt32(form.Get("Teebox.Par"));
            float SlopeRating = float.Parse(form.Get("Teebox.SlopeRating"));

            var teeBox = new TeeBox()
            {
                Id = id,
                Color = new Color(Color),
                CourseRating = CourseRating,
                Distance = Distance,
                Holes = new HoleCollection(),
                Name = Name,
                Par = Par,
                SlopeRating = SlopeRating
            };

            int holeCounter = 0; //"Course.TeeBoxes[0].Holes[0].HoleId"

            while (true)
            {//Course.TeeBoxes[0].Holes[0].HoleId
                if (!string.IsNullOrEmpty(form["Course.TeeBoxes[0].Holes[" + holeCounter + "].Number"]) || !string.IsNullOrEmpty(form["TeeBox.Holes[" + holeCounter + "].Number"]))
                {


                    string holeStr = !string.IsNullOrEmpty(form["Course.TeeBoxes[0].Holes[" + holeCounter + "].Number"]) ? "Course.TeeBoxes[0].Holes[" + holeCounter + "]" : "TeeBox.Holes[" + holeCounter + "]";

                    string holeId = form[holeStr + ".HoleId"];
                    int holeNumber = Convert.ToInt32(form[holeStr + ".Number"]);
                    int holeHcp = Convert.ToInt32(form[holeStr + ".Hcp"]);
                    int holePar = Convert.ToInt32(form[holeStr + ".Par"]);
                    int holeDistance = Convert.ToInt32(form[holeStr + ".Distance"]);

                    var hole = new Hole()
                    {
                        Distance = holeDistance,
                        Hcp = holeHcp,
                        Par = holePar,
                        HoleId = holeId,
                        Number = holeNumber
                    };

                    teeBox.Holes.Add(hole);
                    holeCounter++;
                }
                else
                {
                    holeCounter = 0;
                    break;
                }


            }

            return new TeeboxEditViewModel
            {
                 Teebox = teeBox
            };
        }
    }
}