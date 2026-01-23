using DarksideApi.Util;
using System.Diagnostics;
using System.Text;

namespace DarksideApi.Logging
{
    public partial class Logging : IDisposable
    {
        private FileStream? _stream;
        private readonly string _apiName;
        private List<object>? _backlog;

        public Logging(string apiName)
        {
            this._apiName = HelpTools.ToValidFilename(apiName);
            this._backlog = [];
            this.Log("Logger init for: " + this._apiName);
        }

        public void Open()
        {
            this._stream = File.OpenWrite(Directory.GetCurrentDirectory() + $"\\{this._apiName}-log-{DateTime.UtcNow.ToShortDateString()}.log");

            var copy = this._backlog?.ToList();
            this._backlog = null;

            this.Log($"Beginning log: {DateTime.UtcNow.ToShortDateString()} {DateTime.UtcNow.ToShortTimeString()}");

            foreach (var item in copy ?? [])
            {
                this.Log(item.ToString());
            }
        }

        public void Close()
        {
            this.Log($"Stopping log: {DateTime.UtcNow.ToShortDateString()} {DateTime.UtcNow.ToShortTimeString()}");
            this._stream?.Close();
            this._stream = null;
            this._backlog = [];
        }

        public void Dispose()
        {
            this._stream?.Dispose();
        }

        public void Log(object? value)
        {
            Debug.WriteLine(value);

            if (this._backlog?.Count > 50)
            {
                this._backlog?.RemoveAt(0);
            }

            this._backlog?.Add(value);

            if (this._stream != null && value != null)
            {
                var buffer = Encoding.UTF8.GetBytes($"[{DateTime.UtcNow.ToShortTimeString()}] {value}\n");
                this._stream.Write(buffer);
                this._stream.Flush();
            }
        }
    }
}
