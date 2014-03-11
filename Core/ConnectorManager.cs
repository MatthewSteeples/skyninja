﻿using System;
using System.Collections.Generic;

using SkyNinja.Core.Classes.Factories;
using SkyNinja.Core.Enums;
using SkyNinja.Core.Inputs;
using SkyNinja.Core.Outputs;

namespace SkyNinja.Core
{
    public static class ConnectorManager
    {
        private static readonly IDictionary<ConnectorType, KeyConnectorDictionary> All =
            new Dictionary<ConnectorType, KeyConnectorDictionary>
        {
            {
                ConnectorType.Input, new KeyConnectorDictionary()
                {
                    {"skypeid", new SkypeIdInputFactory()},
                    {"skypedb", new SkypeDbInputFactory()}
                }
            },
            {
                ConnectorType.Output, new KeyConnectorDictionary()
                {
                    {"plain", new PlainTextOutputFactory()},
                }
            }
        };

        public static KeyConnectorDictionary GetFactories(ConnectorType connectorType)
        {
            return All[connectorType];
        }

        /// <summary>
        /// Shortcut for <code>Dictionary&lt;String, Connector&gt;</code>.
        /// </summary>
        public class KeyConnectorDictionary : Dictionary<String, ConnectorFactory>
        {
            // Nothing.
        };
    }
}
