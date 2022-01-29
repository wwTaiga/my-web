using MyWeb.Core.Controllers;
using Xunit;

namespace MyWeb.Core.Tests.Controllers
{
    public class AccountControllerTests
    {
        private AccountController _controller;

        public AccountControllerTests()
        {
            _controller = new AccountController(null, null, null, null, null, null);
        }

        [Fact]
        public void DoRegister()
        {
            var tets = new AccountController(null, null, null, null, null, null);
        }
    }
}
