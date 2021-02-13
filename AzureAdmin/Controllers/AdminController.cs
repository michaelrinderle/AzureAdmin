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

using AzureAdmin.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace AzureAdmin.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly ILogger<AdminController> _logger;

        public AdminController(ILogger<AdminController> logger)
        {
            _logger = logger;
        }

        [HttpGet("RemoveAppxPackage")]
        public IActionResult RemoveAppxPackage(string appId)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                var proc = Powershell.RemoveAppxPackage(appId);

                if(!proc.ExitCode)
                    return Conflict(proc);

                return Ok(proc);
            }
            catch (Exception ex)
            {
                _logger.LogError("1", ex, "AdminController");
                return StatusCode(500);
            }
        }

        [HttpGet("RemoveAppxPackages")]
        public IActionResult RemoveAppxPackages(List<string> appIds)
        {
            try
            {
                var results = new List<ShellInvokeRequest>();
                
                foreach(var appId in appIds)
                    results.Add(Powershell.RemoveAppxPackage(appId));

                var fail = results.Any(x => !x.ExitCode);
                if(fail)
                    return Conflict(results);
                
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(500);
            }
        }
    }
}
