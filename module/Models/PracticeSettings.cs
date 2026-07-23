using Blish_HUD.Settings;

namespace Maestro.Models
{
    public class PracticeSettings
    {
        public SettingEntry<float> LastUsedSpeed { get; }
        public SettingEntry<float> LookaheadSeconds { get; }
        public SettingEntry<int> CountdownLengthMs { get; }

        public PracticeSettings(SettingCollection settings)
        {
            LastUsedSpeed = settings.DefineSetting(
                "practice.lastUsedSpeed",
                1.0f,
                () => "上次使用的練習速度",
                () => "內部設定：記憶上次的練習速度倍率。");

            LookaheadSeconds = settings.DefineSetting(
                "practice.lookaheadSeconds",
                2.5f,
                () => "練習: 預覽時間 (秒)",
                () => "設定音樂軌道顯示歌曲前方多少秒的內容。\n數值越低，捲動速度越快，反應時間越短。");
            LookaheadSeconds.SetRange(1.0f, 5.0f);

            CountdownLengthMs = settings.DefineSetting(
                "practice.countdownLengthMs",
                3000,
                () => "練習: 倒數計時長度 (毫秒)",
                () => "設定練習開始前 3-2-1 倒數計時的持續時間。");
            CountdownLengthMs.SetRange(1000, 6000);
        }
    }
}
