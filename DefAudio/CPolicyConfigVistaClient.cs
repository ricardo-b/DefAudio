using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using CoreAudioApi;

namespace DefAudio
{
    [ComImport, Guid("294935CE-F637-4E7C-A41B-AB255460B862")]
    internal class _CPolicyConfigVistaClient
    {
    }

    public class CPolicyConfigVistaClient
    {
        private IPolicyConfigVista _realClient = new _CPolicyConfigVistaClient() as IPolicyConfigVista;

        public CPolicyConfigVistaClient()
        {
        }

        public int SetDefaultEndpoint(String wszDeviceId, ERole eRole)
        {
            return _realClient.SetDefaultEndpoint(wszDeviceId, eRole);
        }
    }
}
