using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolunteerApp
{
    class databaseClass
    {
    }
    public class ansrecord
    {
        public string studentid { get; set; }
        public bool trueorfalse { get; set; }
        public string Id { get; set; }
        public string questionid { get; set; }
        public string filename { get; set; }
    }
    public class children
    {
        public string Id { get; set; }
        public string username { get; set; }
    }
    public class corresponding
    {
        public string Id { get; set; }
        public string tutorusername { get; set; }
        public string studentusername { get; set; }
    }
    public class assignquestion
    {
        public string Id { get; set; }
        public string acceptchild { get; set; }
        public string tutor { get; set; }
        public string filename { get; set; }
    }
    public class questions
    {
        public string Id { get; set; }
        public string filename { get; set; }
        public string uploader { get; set; }
        public string type { get; set; }
    }
}
