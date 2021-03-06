﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Grapevine.Server;
using Grapevine.Shared.Loggers;
using Shouldly;
using Xunit;

namespace Grapevine.Tests.Server
{
    public class ServerSettingsFacts
    {
        public class Constructors
        {
            [Fact]
            public void DefaultConfiguration()
            {
                var options = new ServerSettings();

                options.Logger.ShouldNotBeNull();
                options.Logger.ShouldBeOfType<NullLogger>();

                options.Router.ShouldNotBeNull();
                options.Router.ShouldBeOfType<Router>();

                options.PublicFolder.ShouldNotBeNull();
                options.PublicFolder.ShouldBeOfType<PublicFolder>();

                options.UseHttps.ShouldBeFalse();
                options.Host.ShouldBe("localhost");
                options.Port.ShouldBe("1234");
                options.Connections.ShouldBe(50);
                options.EnableThrowingExceptions.ShouldBeFalse();

                options.OnAfterStart.ShouldBeNull();
                options.OnBeforeStart.ShouldBeNull();

                options.OnAfterStop.ShouldBeNull();
                options.OnBeforeStop.ShouldBeNull();

                options.OnStart.ShouldBeNull();
                options.OnStop.ShouldBeNull();
            }
        }

        public class OnStartProperty
        {
            [Fact]
            public void IsSynonymForOnAfterStart()
            {
                var options = new ServerSettings();

                Action action1 = () => { };
                Action action2 = () => { };

                options.OnStart = action1;

                options.OnBeforeStart.ShouldBeNull();
                options.OnAfterStart.ShouldBe(action1);
                options.OnStart.ShouldBe(action1);

                options.OnAfterStart = action2;

                options.OnBeforeStart.ShouldBeNull();
                options.OnAfterStart.ShouldBe(action2);
                options.OnStart.ShouldBe(action2);
            }
        }

        public class OnStopProperty
        {
            [Fact]
            public void IsSynonymForOnAfterStop()
            {
                var options = new ServerSettings();

                Action action1 = () => { };
                Action action2 = () => { };

                options.OnStop = action1;

                options.OnBeforeStop.ShouldBeNull();
                options.OnAfterStop.ShouldBe(action1);
                options.OnStop.ShouldBe(action1);

                options.OnAfterStop = action2;

                options.OnBeforeStop.ShouldBeNull();
                options.OnAfterStop.ShouldBe(action2);
                options.OnStop.ShouldBe(action2);
            }
        }

        public class CloneEventHandlersMethod
        {
            [Fact]
            public void ClonesBeforeStartHandlersInCorrectOrder()
            {
                var settings = new ServerSettings();
                var server = new RestServer();
                var order = new List<string>();

                settings.BeforeStarting += rs => { order.Add("1"); };
                settings.BeforeStarting += rs => { order.Add("2"); };
                settings.CloneEventHandlers(server);
                server.GetType().GetMethod("OnBeforeStarting", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(server, null);

                order.Count.ShouldBe(2);
                order[0].ShouldBe("1");
                order[1].ShouldBe("2");
            }

            [Fact]
            public void ClonesAfterStartHandlersInCorrectOrder()
            {
                var settings = new ServerSettings();
                var server = new RestServer();
                var order = new List<string>();

                settings.AfterStarting += rs => { order.Add("1"); };
                settings.AfterStarting += rs => { order.Add("2"); };
                settings.CloneEventHandlers(server);
                server.GetType().GetMethod("OnAfterStarting", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(server, null);

                order.Count.ShouldBe(2);
                order[0].ShouldBe("1");
                order[1].ShouldBe("2");
            }

            [Fact]
            public void ClonesBeforeStopHandlersInCorrectOrder()
            {
                var settings = new ServerSettings();
                var server = new RestServer();
                var order = new List<string>();

                settings.BeforeStopping += rs => { order.Add("1"); };
                settings.BeforeStopping += rs => { order.Add("2"); };
                settings.CloneEventHandlers(server);
                server.GetType().GetMethod("OnBeforeStopping", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(server, null);

                order.Count.ShouldBe(2);
                order[0].ShouldBe("1");
                order[1].ShouldBe("2");
            }

            [Fact]
            public void ClonesAfterStopHandlersInCorrectOrder()
            {
                var settings = new ServerSettings();
                var server = new RestServer();
                var order = new List<string>();

                settings.AfterStopping += rs => { order.Add("1"); };
                settings.AfterStopping += rs => { order.Add("2"); };
                settings.CloneEventHandlers(server);
                server.GetType().GetMethod("OnAfterStopping", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(server, null);

                order.Count.ShouldBe(2);
                order[0].ShouldBe("1");
                order[1].ShouldBe("2");
            }
        }
    }
}
