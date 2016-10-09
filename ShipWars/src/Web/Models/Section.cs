using Web.Enums;

namespace Web.Models
{
    public class Section
    {
        public Section() { }
        public Section(int mark, SectionState state)
        {
            Mark = mark;
            State = state;
        }

        public int Mark { get; set; }
        public SectionState State { get; set; }

        public string CssClass { get; set; }
    }
}
