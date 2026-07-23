using System;
using System.Collections.Generic;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Maestro.Services;
using Microsoft.Xna.Framework.Input;

namespace Maestro.Settings
{
    public class ModuleSettings
    {
        public SettingEntry<KeyBinding> NoteC { get; private set; }
        public SettingEntry<KeyBinding> NoteD { get; private set; }
        public SettingEntry<KeyBinding> NoteE { get; private set; }
        public SettingEntry<KeyBinding> NoteF { get; private set; }
        public SettingEntry<KeyBinding> NoteG { get; private set; }
        public SettingEntry<KeyBinding> NoteA { get; private set; }
        public SettingEntry<KeyBinding> NoteB { get; private set; }
        public SettingEntry<KeyBinding> NoteCHigh { get; private set; }
        public SettingEntry<KeyBinding> OctaveUp { get; private set; }
        public SettingEntry<KeyBinding> OctaveDown { get; private set; }

        public SettingEntry<KeyBinding> SharpC { get; private set; }
        public SettingEntry<KeyBinding> SharpD { get; private set; }
        public SettingEntry<KeyBinding> SharpF { get; private set; }
        public SettingEntry<KeyBinding> SharpG { get; private set; }
        public SettingEntry<KeyBinding> SharpA { get; private set; }

        public string ClientId { get; private set; }
        public SettingEntry<RepeatMode> Repeat { get; private set; }
        public SettingEntry<bool> ShuffleEnabled { get; private set; }

        public ModuleSettings(SettingCollection settings)
        {
            DefineInstrumentKeys(settings);
            DefinePianoSharps(settings);
            DefineClientId(settings);
            DefinePlaybackSettings(settings);
        }

        private void DefineInstrumentKeys(SettingCollection settings)
        {
            var instrumentKeys = settings.AddSubCollection("InstrumentKeys", true, () => "樂器按鍵");

            NoteC = instrumentKeys.DefineSetting("KeyNoteC",
                new KeyBinding(Keys.NumPad1),
                () => "音符 C",
                () => "對應至武器技能 1");

            NoteD = instrumentKeys.DefineSetting("KeyNoteD",
                new KeyBinding(Keys.NumPad2),
                () => "音符 D",
                () => "對應至武器技能 2");

            NoteE = instrumentKeys.DefineSetting("KeyNoteE",
                new KeyBinding(Keys.NumPad3),
                () => "音符 E",
                () => "對應至武器技能 3");

            NoteF = instrumentKeys.DefineSetting("KeyNoteF",
                new KeyBinding(Keys.NumPad4),
                () => "音符 F",
                () => "對應至武器技能 4");

            NoteG = instrumentKeys.DefineSetting("KeyNoteG",
                new KeyBinding(Keys.NumPad5),
                () => "音符 G",
                () => "對應至武器技能 5");

            NoteA = instrumentKeys.DefineSetting("KeyNoteA",
                new KeyBinding(Keys.NumPad6),
                () => "音符 A",
                () => "對應至治療技能");

            NoteB = instrumentKeys.DefineSetting("KeyNoteB",
                new KeyBinding(Keys.NumPad7),
                () => "音符 B",
                () => "對應至通用技能 1");

            NoteCHigh = instrumentKeys.DefineSetting("KeyNoteCHigh",
                new KeyBinding(Keys.NumPad8),
                () => "音符 高音 C",
                () => "對應至通用技能 2");

            OctaveDown = instrumentKeys.DefineSetting("KeyOctaveDown",
                new KeyBinding(Keys.NumPad0),
                () => "降低音階",
                () => "對應至通用技能 3");

            OctaveUp = instrumentKeys.DefineSetting("KeyOctaveUp",
                new KeyBinding(Keys.NumPad9),
                () => "提高音階",
                () => "對應至菁英技能");
        }

        private void DefinePianoSharps(SettingCollection settings)
        {
            var pianoSharps = settings.AddSubCollection("PianoSharps", true, () => "僅限鋼琴 - 升記號音符 - 按鍵不得與自然音符的按鍵綁定衝突");

            SharpC = pianoSharps.DefineSetting("KeySharpC",
                new KeyBinding(ModifierKeys.Alt, Keys.D1),
                () => "升 C#",
                () => "對應至職業技能 1");

            SharpD = pianoSharps.DefineSetting("KeySharpD",
                new KeyBinding(ModifierKeys.Alt, Keys.D2),
                () => "升 D#",
                () => "對應至職業技能 2");

            SharpF = pianoSharps.DefineSetting("KeySharpF",
                new KeyBinding(ModifierKeys.Alt, Keys.D3),
                () => "升 F#",
                () => "對應至職業技能 3");

            SharpG = pianoSharps.DefineSetting("KeySharpG",
                new KeyBinding(ModifierKeys.Alt, Keys.D4),
                () => "升 G#",
                () => "對應至職業技能 4");

            SharpA = pianoSharps.DefineSetting("KeySharpA",
                new KeyBinding(ModifierKeys.Alt, Keys.D5),
                () => "升 A#",
                () => "對應至職業技能 5");
        }

        private void DefineClientId(SettingCollection settings)
        {
            var hiddenSettings = settings.AddSubCollection("Internal", false);
            var clientIdSetting = hiddenSettings.DefineSetting("ClientId", "");

            if (string.IsNullOrEmpty(clientIdSetting.Value))
            {
                clientIdSetting.Value = Guid.NewGuid().ToString();
            }

            ClientId = clientIdSetting.Value;
        }

        private void DefinePlaybackSettings(SettingCollection settings)
        {
            // Hidden sub-collection: toggled from the queue drawer, not the settings UI.
            var playback = settings.AddSubCollection("Playback", false);
            Repeat = playback.DefineSetting("RepeatMode", RepeatMode.Off);
            ShuffleEnabled = playback.DefineSetting("ShuffleEnabled", false);
        }

        public Dictionary<Keys, SettingEntry<KeyBinding>> GetKeyMappings()
        {
            return new Dictionary<Keys, SettingEntry<KeyBinding>>
            {
                { Keys.NumPad1, NoteC },
                { Keys.NumPad2, NoteD },
                { Keys.NumPad3, NoteE },
                { Keys.NumPad4, NoteF },
                { Keys.NumPad5, NoteG },
                { Keys.NumPad6, NoteA },
                { Keys.NumPad7, NoteB },
                { Keys.NumPad8, NoteCHigh },
                { Keys.NumPad9, OctaveUp },
                { Keys.NumPad0, OctaveDown }
            };
        }

        public Dictionary<Keys, SettingEntry<KeyBinding>> GetSharpMappings()
        {
            return new Dictionary<Keys, SettingEntry<KeyBinding>>
            {
                { Keys.NumPad1, SharpC },  // Alt+1 = C#
                { Keys.NumPad2, SharpD },  // Alt+2 = D#
                { Keys.NumPad3, SharpF },  // Alt+3 = F#
                { Keys.NumPad4, SharpG },  // Alt+4 = G#
                { Keys.NumPad5, SharpA }   // Alt+5 = A#
            };
        }
    }
}
