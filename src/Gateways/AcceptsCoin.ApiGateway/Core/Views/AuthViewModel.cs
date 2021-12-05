using System;
using System.Collections.Generic;

namespace AcceptsCoin.ApiGateway.Core.Views
{
    public class AuthenticateViewModel
    {
        public Guid UserId { get; set; }

        public string UserName { get; set; }

        public string Name { get; set; }

        public IEnumerable<AuthenticateViewModel> Roles { get; set; }

        public string Token { get; set; }

        public string Email { get; set; }

    }
    public class AuthUserViewModel
    {
        public Guid UserId { get; set; }

        public string UserName { get; set; }

        public string Name { get; set; }

    }
}
