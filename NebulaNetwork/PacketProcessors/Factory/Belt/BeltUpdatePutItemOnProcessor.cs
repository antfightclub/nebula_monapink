﻿using NebulaModel.Attributes;
using NebulaModel.Networking;
using NebulaModel.Packets;
using NebulaModel.Packets.Belt;
using NebulaWorld;
using NebulaWorld.Factory;

namespace NebulaNetwork.PacketProcessors.Factory.Belt
{
    [RegisterPacketProcessor]
    class BeltUpdatePutItemOnProcessor : PacketProcessor<BeltUpdatePutItemOnPacket>
    {
        public override void ProcessPacket(BeltUpdatePutItemOnPacket packet, NebulaConnection conn)
        {
            using (Multiplayer.Session.Factories.IsIncomingRequest.On())
            {
                GameMain.galaxy.PlanetById(packet.PlanetId)?.factory?.cargoTraffic?.PutItemOnBelt(packet.BeltId, packet.ItemId);
            }
        }
    }
}