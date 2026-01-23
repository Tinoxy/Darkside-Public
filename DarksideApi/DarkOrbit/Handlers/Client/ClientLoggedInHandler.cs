using DarksideApi.DarkOrbit.Commands.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarksideApi.DarkOrbit.Handlers.Client
{
    internal class ClientLoggedInHandler : ICommandHandler
    {
        public void Handle(ByteArray @byte)
        {
            ClientLoggedInCommand clientLoggedIn = new ClientLoggedInCommand();
            clientLoggedIn.Read(@byte);

            Debug.WriteLine("Client logged in!");
        }
    }
}
