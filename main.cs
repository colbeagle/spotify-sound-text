using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace SpotifyOthers {
    internal class Program {
        private static byte[] LastBuffer { get; set; }

        private static void WriteToFile(byte[] buffer) {
            if (buffer == null || buffer == LastBuffer) return;
            var hwid = File.Create("current_song.txt").SafeFileHandle;
            if (hwid == null) return;
            using (var stream = new FileStream(hwid, FileAccess.Write)) {
                if (buffer != LastBuffer) stream.Write(buffer, 0, buffer.Length);
            }
            LastBuffer = buffer;
        }

        private static string GetMediaPlayers() {
            var processes = Process.GetProcesses();
            var buffer = processes.FirstOrDefault(p => p.ProcessName.ToLower() == "spotify" &&
                                                       !string.IsNullOrEmpty(p.MainWindowTitle) &&
                                                       p.MainWindowTitle.ToLower() != "spotify");
            return buffer?.MainWindowTitle;
        }

        private static void Main(string[] args) {
            while (true) {
                WriteToFile(Encoding.UTF8.GetBytes($"Currently Playing: {GetMediaPlayers()}"));
                Thread.Sleep(10000);
            }
        }
    }
}
