using Plugin.Maui.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotationApp.Extensions
{
    public static class AudioManagerExtensions
    {
        public static MauiAppBuilder UseAudioManager(this MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<IAudioManager>(AudioManager.Current);
            return builder;
        }
    }
}
