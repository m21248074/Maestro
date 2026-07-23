namespace Maestro.Models
{
    public enum NoteType
    {
        Whole,      // 𝅝 - 4 beats
        Half,       // 𝅗𝅥 - 2 beats
        Quarter,    // ♩ - 1 beat
        Eighth,     // ♪ - 1/2 beat
        Sixteenth   // 𝅘𝅥𝅯 - 1/4 beat
    }

    public static class NoteTypeExtensions
    {
        public static int GetDurationMs(this NoteType noteType, int bpm)
        {
            double quarterNoteMs = 60000.0 / bpm;
            switch (noteType)
            {
                case NoteType.Whole:
                    return (int)(quarterNoteMs * 4);
                case NoteType.Half:
                    return (int)(quarterNoteMs * 2);
                case NoteType.Quarter:
                    return (int)quarterNoteMs;
                case NoteType.Eighth:
                    return (int)(quarterNoteMs / 2);
                case NoteType.Sixteenth:
                    return (int)(quarterNoteMs / 4);
                default:
                    return (int)quarterNoteMs;
            }
        }

        public static string GetDisplayName(this NoteType noteType)
        {
            switch (noteType)
            {
                case NoteType.Whole:
                    return "全";
                case NoteType.Half:
                    return "二分";
                case NoteType.Quarter:
                    return "四分";
                case NoteType.Eighth:
                    return "八分";
                case NoteType.Sixteenth:
                    return "十六分";
                default:
                    return "四分";
            }
        }
    }
}
