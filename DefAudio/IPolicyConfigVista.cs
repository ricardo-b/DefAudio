/*
 * Based on PolicyConfig.h by "EreTIk" 
 */


using System;
using System.Runtime.InteropServices;
using CoreAudioApi;

namespace DefAudio
{
    [Guid("568b9108-44bf-40b4-9006-86afe5b5a620"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IPolicyConfigVista
    {
        int GetMixFormat();  // not available on Windows 7, use method from IPolicyConfig
        int GetDeviceFormat();
        int SetDeviceFormat();
        int GetProcessingPeriod();  // not available on Windows 7, use method from IPolicyConfig
        int SetProcessingPeriod();  // not available on Windows 7, use method from IPolicyConfig
        int GetShareMode();  // not available on Windows 7, use method from IPolicyConfig
        int SetShareMode();  // not available on Windows 7, use method from IPolicyConfig
        int GetPropertyValue();
        int SetPropertyValue();

        [PreserveSig]
        int SetDefaultEndpoint(String wszDeviceId, ERole eRole );

        int SetEndpointVisibility();  // not available on Windows 7, use method from IPolicyConfig

    };
}
