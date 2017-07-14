using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MyCourseWork.ViewModels
{
    static class ClassForAudio
    {
        public static void playScan()
        {
            MediaPlayer player = new MediaPlayer();
            player.Open(new Uri("D:/kpi/Мультимедійні технології/Курсова робота/MyCourseWork/output/Debug/sounds/pic.mp3", UriKind.Absolute));
            player.Play();
        }
        public static void playNoScan()
        {
            MediaPlayer player = new MediaPlayer();
            player.Open(new Uri("D:/kpi/Мультимедійні технології/Курсова робота/MyCourseWork/output/Debug/sounds/fail.mp3", UriKind.Absolute));
            player.Play();
        }
        public static bool playPrint()
        {
            MediaPlayer player = new MediaPlayer();
            player.Open(new Uri("D:/kpi/Мультимедійні технології/Курсова робота/MyCourseWork/output/Debug/sounds/print.mp3", UriKind.Absolute));
            player.Play();
            return true;
        }
    }
}
