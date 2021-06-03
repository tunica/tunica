using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.Win32.SafeHandles;

namespace Tunica
{
    public class Checker : IDisposable
    {
        private String _tempFileName = String.Empty;
        
        private Boolean _disposed = false;
        
        private SafeHandle _safeHandle = new SafeFileHandle(IntPtr.Zero, true);

        public Checker()
        {
            _tempFileName = Path.GetTempFileName();

            using (var stream = GetType().Assembly.GetManifestResourceStream(@"Tunica.askalono.exe"))
            {
                var bytes = new Byte[(Int32)stream.Length];
                stream.Read(bytes, 0, bytes.Length);
                File.WriteAllBytes(_tempFileName, bytes);
            }
        }

        public void Dispose() => Dispose(true);

        public void Dispose(Boolean disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                try
                {
                    File.Delete(_tempFileName);
                }
                catch
                { }

                _safeHandle?.Dispose();
            }

            _disposed = true;
        }

        public List<License> Crawl(String path)
        {
            var licenses = new List<License>();
            
            if (Directory.Exists(path))
            {
                var standardOutput = new StringBuilder();
                var standardError = new StringBuilder();

                using (var outputWaitHandle = new AutoResetEvent(false))
                using (var errorWaitHandle = new AutoResetEvent(false))
                {
                    using (var process = new Process())
                    {
                        process.StartInfo = new ProcessStartInfo(_tempFileName, "crawl \"" + path + "\"");
                        process.StartInfo.CreateNoWindow = true;
                        process.StartInfo.UseShellExecute = false;
                        process.StartInfo.RedirectStandardOutput = true;
                        process.StartInfo.RedirectStandardError = true;

                        process.OutputDataReceived += (sender, e) =>
                        {
                            if (e.Data == null)
                            {
                                outputWaitHandle.Set();
                            }
                            else
                            {
                                standardOutput.AppendLine(e.Data);
                            }
                        };

                        process.ErrorDataReceived += (sender, e) =>
                        {
                            if (e.Data == null)
                            {
                                errorWaitHandle.Set();
                            }
                            else
                            {
                                standardError.AppendLine(e.Data);
                            }
                        };

                        process.Start();

                        process.BeginOutputReadLine();
                        process.BeginErrorReadLine();

                        process.WaitForExit();
                    }
                }

                var currentLicense = new License(null, null, 0);

                foreach (var line in standardOutput.ToString().Split(new String[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (String.IsNullOrEmpty(currentLicense.Path))
                    {
                        if (line.StartsWith(path, StringComparison.OrdinalIgnoreCase))
                        {
                            currentLicense.Path = line;
                        }
                    }
                    else if (String.IsNullOrEmpty(currentLicense.Type))
                    {
                        var match = Regex.Match(line, @"License: (.+?) \(.+?\)");
                        if (match.Success && match.Groups.Count == 2)
                        {
                            currentLicense.Type = match.Groups[1].Value;
                        }
                        else
                        {
                            currentLicense = new License(null, null, 0);
                        }
                    }
                    else
                    {
                        var match = Regex.Match(line, @"Score: ([0|1]\.\d{1,3})");
                        if (match.Success && match.Groups.Count == 2)
                        {
                            try
                            {
                                currentLicense.Confidence = Double.Parse(match.Groups[1].Value);
                                licenses.Add(currentLicense);
                                currentLicense = new License(null, null, 0);
                            }
                            catch
                            {
                                currentLicense = new License(null, null, 0);
                            }
                        }
                        else
                        {
                            currentLicense = new License(null, null, 0);
                        }
                    }
                }
            }

            return licenses;
        }
    }
}
