//    __ _/| _/. _  ._/__ /
// _\/_// /_///_// / /_|/
//            _/
// sof digital 2021
// written by michael rinderle <michael@sofdigital.net>

// mit license
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Diagnostics;

namespace AzureAdmin.Data
{
    public static class Powershell
    {
        public static ShellInvokeRequest RemoveAppxPackage(string appId)
        {
            return LaunchCommand("powershell.exe", $"Get-AppxPackage -all *{appId}* | Remove-AppxPackage -AllUsers");
        }

        private static ShellInvokeRequest LaunchCommand(string uri, string args)
        {
            var pr = new ShellInvokeRequest();
            var psi = new ProcessStartInfo();
            psi.UseShellExecute = false;
            psi.CreateNoWindow = false;
            psi.Arguments = args;
            psi.RedirectStandardOutput = true;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            psi.FileName = uri;

            try
            {
                var proc = Process.Start(psi);
                proc.WaitForExit();
                pr.ExitCode = (proc.ExitCode == 0) ? true : false;
                pr.Output = proc.StandardOutput.ReadToEnd();
            }
            catch (Exception ex)
            {
                pr.ExitCode = false;
                pr.Output = string.Empty;
            }

            return pr;
        }
    }
}
