using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Maestro.Models
{
    /// <summary>
    /// Central registry of every supported instrument. Add a new instrument by adding a
    /// row here (and a value to <see cref="InstrumentType"/>); all pickers, colors, and
    /// Creator/playback octave logic read from this table.
    /// </summary>
    public static class InstrumentCatalog
    {
        private static readonly string[] ThreeOctaveLabels = { "低音 (-)", "中音", "高音 (+)" };

        private static readonly IReadOnlyList<InstrumentInfo> _all = new List<InstrumentInfo>
        {
            new InstrumentInfo(InstrumentType.Piano, "鋼琴",
                new Color(79, 155, 224), new Color(53, 122, 192),   // sapphire
                sharpsEnabled: true, minOctave: -1, maxOctave: 1, octaveLabels: ThreeOctaveLabels),

            new InstrumentInfo(InstrumentType.Harp, "豎琴",
                new Color(107, 194, 136), new Color(62, 154, 99),   // emerald
                sharpsEnabled: false, minOctave: -1, maxOctave: 1, octaveLabels: ThreeOctaveLabels),

            new InstrumentInfo(InstrumentType.Lute, "魯特琴",
                new Color(227, 165, 58), new Color(190, 132, 32),   // amber
                sharpsEnabled: false, minOctave: -1, maxOctave: 1, octaveLabels: ThreeOctaveLabels),

            new InstrumentInfo(InstrumentType.Bass, "貝斯",
                new Color(224, 106, 124), new Color(184, 72, 94),   // garnet rose
                sharpsEnabled: false, minOctave: 0, maxOctave: 1,
                octaveLabels: new[] { "Low", "High" }),

            new InstrumentInfo(InstrumentType.Flute, "長笛",
                new Color(165, 121, 224), new Color(126, 84, 190),  // amethyst
                sharpsEnabled: false, minOctave: -1, maxOctave: 0,
                octaveLabels: new[] { "Low", "Middle" }),

            new InstrumentInfo(InstrumentType.Bell, "鈴鐺 (3 個八度)",
                new Color(63, 194, 178), new Color(42, 148, 136),   // turquoise
                sharpsEnabled: false, minOctave: -1, maxOctave: 1, octaveLabels: ThreeOctaveLabels),

            new InstrumentInfo(InstrumentType.BellMagnanimous, "鈴鐺 (2 個八度)",
                new Color(116, 214, 190), new Color(73, 174, 151),  // mint (bell family)
                sharpsEnabled: false, minOctave: 0, maxOctave: 1,
                octaveLabels: new[] { "Middle", "High" }),

            new InstrumentInfo(InstrumentType.DrumSet, "鼓組",
                new Color(198, 110, 64), new Color(160, 82, 45),    // copper/bronze
                sharpsEnabled: false, minOctave: 0, maxOctave: 0,
                octaveLabels: new[] { "Kit" },
                listedInPickers: true, isPercussion: true),
        };

        private static readonly Dictionary<InstrumentType, InstrumentInfo> _byType =
            _all.ToDictionary(i => i.Type);

        private static readonly IReadOnlyList<InstrumentInfo> _pickable =
            _all.Where(i => i.ListedInPickers).ToList().AsReadOnly();

        static InstrumentCatalog()
        {
            foreach (InstrumentType type in System.Enum.GetValues(typeof(InstrumentType)))
            {
                if (!_byType.ContainsKey(type))
                {
                    throw new System.InvalidOperationException(
                        $"InstrumentCatalog is missing a row for InstrumentType.{type}");
                }
            }
        }

        /// <summary>All instruments, in enum order.</summary>
        public static IReadOnlyList<InstrumentInfo> All => _all;

        /// <summary>Instruments that appear in pickers, in enum order.</summary>
        public static IReadOnlyList<InstrumentInfo> Pickable => _pickable;

        /// <summary>Descriptor for a type. Every InstrumentType value has a row.</summary>
        public static InstrumentInfo Get(InstrumentType type) => _byType[type];

        /// <summary>Maps a display name (e.g. "Bell (2 octaves)") back to its type.</summary>
        public static bool TryFromDisplayName(string displayName, out InstrumentType type)
        {
            foreach (var info in _all)
            {
                if (info.DisplayName == displayName)
                {
                    type = info.Type;
                    return true;
                }
            }

            type = default(InstrumentType);
            return false;
        }
    }
}
