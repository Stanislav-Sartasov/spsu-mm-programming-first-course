using System;
using InterfaceLibrary;


namespace PartyOfUSA
{
    class Republican : IParty
    {
        public int TrustRating()
        {
            return 51;
        }
    }
}
