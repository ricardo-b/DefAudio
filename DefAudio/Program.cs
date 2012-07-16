using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Reflection;
using System.Runtime.InteropServices;
using CoreAudioApi;
using System.Threading;

namespace DefAudio
{
    static class Program
    {
        public static CPolicyConfigVistaClient configClient;
        public static NotifyIcon notifyIcon;
        public static ContextMenu contextMenu;

        public static MMDeviceEnumerator deviceEnumerator;
        public static MMDeviceCollection deviceColletion;

        [STAThread]
        static void Main()
        {
            bool onlyInstance = false;
            Mutex mutex = new Mutex(true, "DefAudio", out onlyInstance);
            if (!onlyInstance)
            {
                return;
            }

            deviceEnumerator = new MMDeviceEnumerator();
            deviceColletion = deviceEnumerator.EnumerateAudioEndPoints(EDataFlow.eRender, EDeviceState.DEVICE_STATE_ACTIVE);
            configClient = new CPolicyConfigVistaClient();
            contextMenu = new ContextMenu();

            MenuItem[] items = new MenuItem[deviceColletion.Count];
            EventHandler eventHandler = new EventHandler(menuItem_MouseClick);

           
            for (int i = 0; i < deviceColletion.Count; i++)
            {
                items[i] = new MenuItem(deviceColletion[i].FriendlyName, eventHandler)
                {
                    Name = deviceColletion[i].FriendlyName,
                    Tag = deviceColletion[i],

                };

            }

            items = items.OrderBy(i => i.Text).ToArray();
            contextMenu.MenuItems.AddRange(items);

            contextMenu.MenuItems.Add(new MenuItem("-"));
            contextMenu.MenuItems.Add(new MenuItem("Exit", new EventHandler( 
                     (sender, e) =>  Application.Exit()                
                )));


            notifyIcon = new NotifyIcon()
            {
                ContextMenu = contextMenu,
                Icon = Properties.Resources.icon_headphones_128,
                Text = "DefAudio",
                Visible = true
            };
            

            notifyIcon.MouseClick += new MouseEventHandler(notifyIcon_MouseClick);
            notifyIcon.MouseDown += new MouseEventHandler(notifyIcon_MouseDown);
           
                     
            Application.Run();

            notifyIcon.Visible = false;
            if (notifyIcon != null) notifyIcon.Dispose();

            foreach (var Item in items)
            {
                if (Item != null) Item.Dispose();
            }

        }

        static void notifyIcon_MouseDown(object sender, MouseEventArgs e)
        {
            MMDevice DefaultDevice = deviceEnumerator.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eConsole);

            foreach (MenuItem menuItem in contextMenu.MenuItems)
            {
                if (menuItem.Tag is MMDevice)
                {
                    menuItem.Checked = ((menuItem.Tag as MMDevice).ID == DefaultDevice.ID);
                }
            }
        }


        private static void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                typeof(NotifyIcon).GetMethod("ShowContextMenu", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(notifyIcon, null);
            }

        }

        private static void menuItem_MouseClick(object sender, EventArgs e)
        {            
            MenuItem clickedItem = (MenuItem)sender;
            MMDevice Device = (MMDevice)clickedItem.Tag;

            configClient.SetDefaultEndpoint(Device.ID, ERole.eConsole);

        }




    }
}
